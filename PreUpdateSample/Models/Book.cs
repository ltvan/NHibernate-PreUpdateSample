using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreUpdateSample.Models
{
    public class Book : IEntity, IHasModificationTime
    {

        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
    }
}
