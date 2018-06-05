using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Type;

namespace PreUpdateSample
{

    /// <summary>
    /// Used to perform some common date-time operations.
    /// </summary>
    public static class Clock
    {
        static Clock()
        {
        }

        public static DateTime Now => DateTime.UtcNow;

        /// <summary>
        /// Normalizes given <see cref="DateTime"/> using current <see cref="Provider"/>.
        /// </summary>
        /// <param name="dateTime">DateTime to be normalized.</param>
        /// <returns>Normalized DateTime</returns>
        public static DateTime Normalize(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }

            if (dateTime.Kind == DateTimeKind.Local)
            {
                return dateTime.ToUniversalTime();
            }
            return dateTime;
        }
    }

}
