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
        /*
         * Type
         * 1 - Use user's session instead of temporary session before order begins
         * 2 - Update session to existing user (occurs when order is done and has not logged in)
         */
        public int Type { get; set; }
    }

    public class GenerateCaptureOutput
    {
        public string CaptureId { get; set; }
        public string CaptureCode { get; set; }
        public string CaptureUrl { get; set; }
        public string CaptureQR { get; set; }
        public string Result { get; set; }
    }
}
