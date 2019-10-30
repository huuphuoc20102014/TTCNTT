using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTCNTT.Models
{
    public class TTJsonResult
    {
        public TTJsonResult(bool isOk, object payload)
        {
            IsOk = isOk;
            Payload = payload;
        }
        public bool IsOk { get; set; }
        public object Payload { get; set; }
    }
}
