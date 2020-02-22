using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class CaptureInfoInput
    {
        public string CaptureId { get; set; }
    }

    public class CaptureInfoOutput
    {
        public string SessionId { get; set; }
        public string SessionKey { get; set; }
        public string Status { get; set; }
    }
}
