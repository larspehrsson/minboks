﻿using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using MinBoks.Eboks;

namespace MinBoks
{
    public partial class EboksServer : ServiceBase
    {
        private static readonly Random Random = new Random();
        private static Configuration _config;

        public EboksServer()
        {
            ServiceName = "EBoks Service";
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            RunServer();
        }


        public void ConsoleRun()
        {
            RunServer();
        }


        private void RunServer()
        {
            var eboksWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };
            eboksWorker.DoWork += startPolling;
            eboksWorker.RunWorkerAsync();
        }

        protected override void OnStop()
        {
        }


        private void startPolling(object sender, DoWorkEventArgs e)
        {
            if (!loadconfig())
                return;

            var deviceid = getValue("deviceid");
            if (string.IsNullOrEmpty(deviceid))
                deviceid = Guid.NewGuid().ToString();

            var response = getValue("response");
            if (string.IsNullOrEmpty(response))
                response = GetRandomHexNumber(64);

            setValue("deviceid", deviceid);
            setValue("response", response);

            var account = new Account
            {
                UserId = getValue("brugernavn"),
                Password = getValue("password"),
                ActivationCode = getValue("aktiveringskode"),
                DeviceId = deviceid,
                response = response
            };


            var api = new Api();
            api.LoadHentetList();

            while (true)
            {
                api.GetSessionForAccountRest(account);
                api.DownloadAll(account);
                Thread.Sleep(10*60*1000);
            }
        }

        public static string getValue(string key)
        {
            var element = _config.AppSettings.Settings[key];
            if (element != null)
            {
                var value = element.Value;
                if (!string.IsNullOrEmpty(value))
                    return value;
            }
            return string.Empty;
        }


        public static void setValue(string felt, string val)
        {
            _config.AppSettings.Settings[felt].Value = val;
            _config.Save(ConfigurationSaveMode.Modified);
        }

        private static bool loadconfig()
        {
            var exeConfigPath = Assembly.GetExecutingAssembly().Location;
            // For debugging purposes only
            if (exeConfigPath.EndsWith("MinBoks.exe"))
                exeConfigPath = exeConfigPath.Replace("eBoksConsole", "MinBoks");

            Console.WriteLine("Åbner configfilen " + exeConfigPath);
            AppGlobals.logMessage("Åbner configfilen " + exeConfigPath);

            try
            {
                _config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Config fil kunne ikke findes " + ex.Message);
                AppGlobals.logMessage("Config fil kunne ikke findes " + ex.Message);
                return false;
            }

            if (!_config.HasFile)
            {
                Console.WriteLine("Config fil kunne ikke findes " + _config.FilePath);
                AppGlobals.logMessage("Config fil kunne ikke findes " + _config.FilePath);
                return false;
            }

            return true;
        }


        private static string GetRandomHexNumber(int digits)
        {
            var buffer = new byte[digits/2];
            Random.NextBytes(buffer);
            var result = string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (digits%2 == 0)
                return result.ToLower();
            return (result + Random.Next(16).ToString("x")).ToLower();
        }
    }
}