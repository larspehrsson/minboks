using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using RestSharp;

namespace MinBoks.Eboks
{
    internal class Api
    {
        private const string BaseUrl = "https://rest.e-boks.dk/mobile/1/xml.svc/en-gb";
        private static Session _session = new Session();
        private MainForm _mainForm = new MainForm();

        public void setguireference(MainForm mf)
        {
            _mainForm = mf;
        }

        public void GetSessionForAccountRest(Account account)
        {
            var client = new RestClient(BaseUrl)
            {
                UserAgent = "eboks/35 CFNetwork/672.1.15 Darwin/14.0.0"
            };

            var request = new RestRequest("/session", Method.PUT)
            {
                RequestFormat = DataFormat.Xml
            };

            _session.DeviceId = account.DeviceId;

            request.AddHeader("X-EBOKS-AUTHENTICATE", GetAuthHeader(account, _session));
            request.AddHeader("Content-Type", "application/xml");
            request.AddHeader("Accept", "*/*");

            var xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                      "<Logon xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"urn:eboks:mobile:1.0.0\">" +
                      "<App version=\"1.4.1\" os=\"iOS\" osVersion=\"9.0.0\" Device=\"iPhone\" />" +
                      "<User identity=\"" + account.UserId + "\" identityType=\"P\" nationality=\"DK\" pincode=\"" +
                      account.Password + "\"/>" +
                      "</Logon>";

            request.AddParameter("application/xml", xml, ParameterType.RequestBody);

            var response = client.Execute(request);

            var sessionid =
                response.Headers.Where(c => c.Name == "X-EBOKS-AUTHENTICATE")
                    .Select(c => c.Value)
                    .FirstOrDefault()
                    .ToString()
                    .Split(',');

            _session.SessionId = sessionid[0].Substring(11, sessionid[0].Length - 12);
            _session.Nonce = sessionid[1].Substring(8, sessionid[1].Length - 9);

            var doc = RemoveAllNameSpaces(response.Content);

            _session.Name = doc.XPathSelectElement("Session/User").Attribute("name").Value;
            _session.InternalUserId = doc.XPathSelectElement("Session/User").Attribute("userId").Value;
        }

        public void DownloadAll(Account account)
        {

            var xdoc = getxml(account, _session.InternalUserId + "/0/mail/folders");

            var queryFolders = (from t in xdoc.Descendants("FolderInfo") where t.Attribute("id") != null select t).ToList();

            foreach (var folder in queryFolders)
            {
                var folderid = folder.Attribute("id").Value;
                var foldername = folder.Attribute("name").Value;

                GetSessionForAccountRest(account);

                var messages = getxml(account, _session.InternalUserId + "/0/mail/folder/" + folderid + "?skip=0&take=100");

                var queryMessages = (from t in messages.Descendants("MessageInfo") where t.Attribute("id") != null select t).ToList();
                foreach (var message in queryMessages)
                {
                    var messageId = message.Attribute("id").Value;
                    var messageName = message.Attribute("name").Value;
                    var format = message.Attribute("format").Value;

                    GetSessionForAccountRest(account);

                    var xmessages = getxml(account, _session.InternalUserId + "/0/mail/folder/" + folderid + "/message/" + messageId);
                    GetSessionForAccountRest(account);

                    getContent(account, _session.InternalUserId + "/0/mail/folder/" + folderid + "/message/" + messageId + "/content", messageName + " - " + messageId + "." + format);

                }
            }

        }

        public XDocument getxml(Account account, string url)
        {
            var client = new RestClient(BaseUrl)
            {
                UserAgent = "eboks/35 CFNetwork/672.1.15 Darwin/14.0.0"
            };

            var request = new RestRequest(url, Method.GET);
            request.AddHeader("X-EBOKS-AUTHENTICATE", GetSessionHeader(account));
            request.AddHeader("Accept", "*/*");

            var response = client.Execute(request);
            var responsedoc = RemoveAllNameSpaces(response.Content);

            return responsedoc;
        }


        public XDocument RemoveAllNameSpaces(string content)
        {
            // TODO: Remove this ugly replace
            var responsedoc = XDocument.Parse(content.Replace("xmlns=\"urn:eboks:mobile:1.0.0\"", ""));
            // Remove all namespaces from document
            responsedoc.Descendants().Attributes().Where(a => a.IsNamespaceDeclaration).Remove();

            return responsedoc;
        }

        public bool getContent(Account account, string url, string filename)
        {
            if (File.Exists(filename))
                return true;

            var client = new RestClient(BaseUrl)
            {
                UserAgent = "eboks/35 CFNetwork/672.1.15 Darwin/14.0.0"
            };

            var request = new RestRequest(url, Method.GET);
            request.AddHeader("X-EBOKS-AUTHENTICATE", GetSessionHeader(account));
            request.AddHeader("Accept", "*/*");

            filename = Path.GetInvalidFileNameChars().Aggregate(filename, (current, c) => current.Replace(c, '_'));

            _mainForm.Log("Downloader " + filename);

            var filedata = client.DownloadData(request);
            File.WriteAllBytes(filename, filedata);
            return true;
        }

        private string GetAuthHeader(Account account, Session session)
        {
            var date = DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss");

            string input =
                $"{account.ActivationCode}:{session.DeviceId}:P:{account.UserId}:DK:{account.Password}:{date}";

            var challenge = Sha256Hash(input);
            challenge = Sha256Hash(challenge);

            return $"logon deviceid=\"{session.DeviceId}\",datetime=\"{date}\",challenge=\"{challenge}\"";
        }

        private string GetSessionHeader(Account account)
        {
            string input = $"deviceid={_session.DeviceId},nonce={_session.Nonce},sessionid={_session.SessionId},response={account.response}";
            return input;
        }

        private string Sha256Hash(string value)
        {
            var sb = new StringBuilder();

            using (var hasher = SHA256.Create())
            {
                var result = hasher.ComputeHash(Encoding.UTF8.GetBytes(value));
                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}