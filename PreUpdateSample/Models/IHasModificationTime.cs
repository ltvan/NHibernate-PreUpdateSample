using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreUpdateSample.Models
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}
