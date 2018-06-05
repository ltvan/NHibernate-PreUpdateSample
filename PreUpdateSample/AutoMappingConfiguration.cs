using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate.Tool.hbm2ddl;
using PreUpdateSample.Models;

namespace PreUpdateSample
{

    internal class AutoMappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type.GetInterface(typeof(IEntity).FullName) != null;
        }

        public override bool IsId(Member member)
        {
            return member.Name == "Id";
        }

        public override bool IsComponent(Type type)
        {
            return false;
        }

        public override string GetComponentColumnPrefix(Member member)
        {
            return member.PropertyType.Name;
        }

        public override bool IsVersion(Member member)
        {
            return member.Name == "Version";
        }
    }

}
