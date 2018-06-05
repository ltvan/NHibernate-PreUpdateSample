using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreUpdateSample.Models
{
    public class BookMapOverride : IAutoMappingOverride<Book>
    {
        public void Override(AutoMapping<Book> mapping)
        {
        }
    }
}
