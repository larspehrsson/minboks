using RestSharp.Serializers;

namespace MinBoks.Eboks
{
    public class App
    {
        [SerializeAs(Name = "version", Attribute = true)]
        public string Version { get; set; }
        [SerializeAs(Name = "os", Attribute = true)]
        public string OS { get; set; }
        [SerializeAs(Name = "osVersion", Attribute = true)]
        public string OSVersion { get; set; }
        [SerializeAs(Name = "Device", Attribute = true)]
        public string Device { get; set; }
    }


    public class User
    {
        [SerializeAs(Name = "identity", Attribute = true)]
        public string Identity { get; set; }
        [SerializeAs(Name = "identityType", Attribute = true)]
        public string IdentityType { get; set; }
        [SerializeAs(Name = "nationality", Attribute = true)]
        public string Nationality { get; set; }
        [SerializeAs(Name = "pincode", Attribute = true)]
        public string Pincode { get; set; }
    }


    public class Logon
    {
        public App App { get; set; }
        public User User { get; set; }
    }
}
