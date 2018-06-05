using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Type;
using PreUpdateSample.Models;

namespace PreUpdateSample
{
    internal class AbpNHibernateInterceptor : EmptyInterceptor
    {
        public AbpNHibernateInterceptor()
        {
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            var updated = false;
            //Set modification audits
            if (entity is IHasModificationTime)
            {
                for (var i = 0; i < propertyNames.Length; i++)
                {
                    if (propertyNames[i] == "LastModificationTime")
                    {
                        currentState[i] = (entity as IHasModificationTime).LastModificationTime = Clock.Now;
                        //updated = true;
                    }
                }
            }

            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types) || updated;
        }

        public override int[] FindDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            return base.FindDirty(entity, id, currentState, previousState, propertyNames, types);
        }

        public override bool OnLoad(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            NormalizeDateTimePropertiesForEntity(state, types);
            return true;
        }

        private static void NormalizeDateTimePropertiesForEntity(object[] state, IList<IType> types)
        {
            for (var i = 0; i < types.Count; i++)
            {
                if (types[i].IsComponentType)
                {
                    NormalizeDateTimePropertiesForComponentType(state[i], types[i]);
                }

                if (types[i].ReturnedClass != typeof(DateTime) && types[i].ReturnedClass != typeof(DateTime?))
                {
                    continue;
                }

                var dateTime = state[i] as DateTime?;

                if (!dateTime.HasValue)
                {
                    continue;
                }

                state[i] = Clock.Normalize(dateTime.Value);
            }
        }

        private static void NormalizeDateTimePropertiesForComponentType(object componentObject, IType type)
        {
            if (componentObject == null)
            {
                return;
            }

            var componentType = type as ComponentType;
            if (componentType == null)
            {
                return;
            }

            for (int i = 0; i < componentType.PropertyNames.Length; i++)
            {
                var propertyName = componentType.PropertyNames[i];
                if (componentType.Subtypes[i].IsComponentType)
                {
                    var value = componentObject.GetType().GetProperty(propertyName).GetValue(componentObject, null);
                    NormalizeDateTimePropertiesForComponentType(value, componentType.Subtypes[i]);
                }

                if (componentType.Subtypes[i].ReturnedClass != typeof(DateTime) && componentType.Subtypes[i].ReturnedClass != typeof(DateTime?))
                {
                    continue;
                }

                var dateTime = componentObject.GetType().GetProperty(propertyName).GetValue(componentObject) as DateTime?;

                if (!dateTime.HasValue)
                {
                    continue;
                }

                componentObject.GetType().GetProperty(propertyName).SetValue(componentObject, Clock.Normalize(dateTime.Value));
            }
        }
    }
}
