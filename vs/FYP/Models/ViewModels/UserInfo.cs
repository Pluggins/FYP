using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class UserInfoInput
    {
        public string SessionId { get; set; }
        public string SessionKey { get; set; }
    }
    public class UserInfoOutput
    {
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string DateJoined { get; set; }
        public List<OrderPreviousItem> Orders { get; set; }
        public string Result { get; set; }
    }
}
