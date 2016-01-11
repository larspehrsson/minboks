using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinBoks.Eboks
{
    internal class Session
    {
        public Account Account { get; set; }

        public string Name { get; set; }

        public string InternalUserId { get; set; }

        public string DeviceId { get; set; }

        public string SessionId { get; set; }

        public string Nonce { get; set; }
    }
}
