using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinBoks.Eboks
{
    public class App
    {
        [SerializeAs(Name = "version")]
        public string version { get; set; }
        [SerializeAs(Name = "os")]
        public string os { get; set; }
        [SerializeAs(Name = "osVersion")]
        public string osVersion { get; set; }
        [SerializeAs(Name = "Device")]
        public string Device { get; set; }
    }


    public class User
    {
        [SerializeAs(Name = "identity")]
        public string identity { get; set; }
        [SerializeAs(Name = "identityType")]
        public string identityType { get; set; }
        [SerializeAs(Name = "nationality")]
        public string nationality { get; set; }
        [SerializeAs(Name = "pincode")]
        public string pincode { get; set; }
    }


    public class Logon
    {
        public App App { get; set; }
        public User User { get; set; }
    }
}
