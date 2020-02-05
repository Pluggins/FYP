using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class GenerateCaptureInput
    {
        public string SessionId { get; set; }
        public string SessionKey { get; set; }
    }

    public class GenerateCaptureOutput
    {
        public string CaptureId { get; set; }
        public string CaptureCode { get; set; }
        public string Result { get; set; }
    }
}
