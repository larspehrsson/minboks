using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinBoks.Eboks
{
    internal class Account
    {
        public string uid { get; set; }
        public string UserId { get; set; }

        public string Password { get; set; }

        public string ActivationCode { get; set; }

        public string OwnerName { get; set; }

        public bool FailedLoading { get; set; }
        public string DeviceId { get; set; }
        public string response { get; set; }
    }
}
