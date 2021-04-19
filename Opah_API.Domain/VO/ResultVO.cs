using System;
using System.Collections.Generic;
using System.Text;

namespace Opah_API.Domain.VO
{
    public class ResultVO
    {
        public bool error { get; set; }
        public string message { get; set; }
        public object objectResult { get; set; }
    }
}
