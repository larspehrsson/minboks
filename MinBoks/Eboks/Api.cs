using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MinBoks.Eboks
{
    internal class Api
    {
        public Api()
        {
        }


        public Session GetSessionForAccount(Account account)
        {
            RestClient client = new RestClient(BaseUrl);
            client.UserAgent = "eboks/35 CFNetwork/672.1.15 Darwin/14.0.0";

            var request = new RestRequest("/session", Method.PUT);

            Session session = new Session();
            session.DeviceId = Guid.NewGuid().ToString();

            request.AddHeader("X-EBOKS-AUTHENTICATE", GetAuthHeader(account, session));
            //request.AddHeader("Content-Type", "application/xml");
            //request.AddHeader("Accept", "*/*");

            var logon = new Logon
            {
                App = new App
                {
                    version = "1.4.1",
                    os = "iOS",
                    osVersion = "9.0.0",
                    Device = "iPhone"
                },
                User = new User
                {
                    identity = account.UserId,
                    identityType = "P",
                    nationality = "DK",
                    pincode = account.Password
                }
            };

            request.AddBody(logon, "urn:eboks:mobile:1.0.0");

            IRestResponse response;

            response = client.Execute(request);

            return null;
        }


        public List<string> GetFolders(Account account)
        {
            return null;
        }


        public void GetFolderId(Session session, string folderId)
        {
        }


        public void GetFileDataForMessageId(Session session, string messageId)
        {
        }


        private string GetAuthHeader(Account account, Session session)
        {
            string date = DateTime.Now.ToString("R");

            string input = string.Format("{0}:{1}:P:{2}:DK:{3}:{4}",
                account.ActivationCode,
                session.DeviceId,
                account.UserId,
                account.Password,
                date);

            string challenge = Sha256Hash(input);
            challenge = Sha256Hash(challenge);

            return string.Format("logon deviceid=\"{0}\",datetime=\"{1}\",challenge=\"{2}\"", session.DeviceId, date, challenge);
        }


        private string Sha256Hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hasher = SHA256Managed.Create()) {
                Byte[] result = hasher.ComputeHash(Encoding.UTF8.GetBytes(value));
                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
              }
              return Sb.ToString();
        }



        private const string BaseUrl = "https://rest.e-boks.dk/mobile/1/xml.svc/en-gb";
    }
}
