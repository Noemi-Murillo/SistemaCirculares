using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Reply
{
    public class Reply<T>
    {
        public bool Ok { get; set; }
        public string? Message { get; set; }
        public T? Result { get; set; }


    }
}
