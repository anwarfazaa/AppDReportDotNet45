using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AppdReporter
{
    static class Configuration
    {

        public static string ControllerUrl { get; set; }

        public static string ControllerAccount { get; set; }

        public static string ApiId { get; set; }

        public static string SecretKey { get; set; }

        public static string ReportStyle { get; set; }

        public static DateTime StartTime { get; set; }

        public static DateTime EndTime { get; set; }

        public static int TimeMachineValue { get; set; }

        public static void SaveConfiguration()
        {
            JObject ConfigurationObject = new JObject(
                new JProperty("ControllerUrl", ControllerUrl),
                new JProperty("ControllerAccount", ControllerAccount),
                new JProperty("ApiId", ApiId),
                new JProperty("SecretKey", SecretKey)
                );

            File.WriteAllText(Path.Combine(Application.StartupPath, "settings.json"), ConfigurationObject.ToString());
        }

        public static void LoadConfiguration()
        {
            try { 
                JObject ConfigurationObj = JObject.Parse(File.ReadAllText(Path.Combine(Application.StartupPath, "settings.json")));
                ControllerUrl = ConfigurationObj.SelectToken("ControllerUrl").ToString();
                ControllerAccount = ConfigurationObj.SelectToken("ControllerAccount").ToString();
                ApiId = ConfigurationObj.SelectToken("ApiId").ToString();
                SecretKey = ConfigurationObj.SelectToken("SecretKey").ToString();
            } catch (Exception)
            {
                // do nothing
            }
        }

    }
}
