using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bolt
{
    public static class Settings
    {
        static Settings()
        {
            Logger.Debug($"Settings at version: {Version}");
        }

        //Major, minor, and build number
        public static Tuple<int, int, int> Version = new Tuple<int, int, int>(0, 0, Assembly.GetExecutingAssembly().GetName().Version.Build);
        public static string OutputDirectory = @".\";

        public static string MakeMKV_Location = @"C:\Program Files\Handbrake\HandBrakeCLI.exe";
        public static bool MakeMKV_Enabled = false;
        public static int MakeMKV_Min_Seconds = 600;

        public static string HandBrake_Location = @"C:\Program Files (x86)\MakeMKV\makemkvcon64.exe";
        public static bool HandBrake_Enabled = false;
        public static bool HandBrake_ConvertAllFiles = false;
        public static bool HandBrake_DeleteOriginalFile = false;

        public static void LoadSettings()
        {
            Dictionary<string, string> values = null;
            try
            {
                TextReader reader = new StreamReader("./Bolt.Config");
                values = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.ReadToEnd());
                reader.Close();
            }
            catch (Exception ex)
            {
                Logger.Error($"Could not load configuration file: {ex}", true);
            }

            try
            {
                if (values.ContainsKey("MakeMKV_Location"))
                {
                    MakeMKV_Location = values["MakeMKV_Location"];
                }
                if (values.ContainsKey("MakeMKV_Enabled"))
                {
                    MakeMKV_Enabled = Convert.ToBoolean(values["MakeMKV_Enabled"]);
                }
                if (values.ContainsKey("MakeMKV_Min_Seconds"))
                {
                    MakeMKV_Min_Seconds = Convert.ToInt32(values["MakeMKV_Min_Seconds"]);
                }

                if (values.ContainsKey("HandBrake_Location"))
                {
                    HandBrake_Location = values["HandBrake_Location"];
                }
                if (values.ContainsKey("HandBrake_Enabled"))
                {
                    HandBrake_Enabled = Convert.ToBoolean(values["HandBrake_Enabled"]);
                }
                if (values.ContainsKey("HandBrake_ConvertAllFiles"))
                {
                    HandBrake_ConvertAllFiles = Convert.ToBoolean(values["HandBrake_ConvertAllFiles"]);
                }
                if (values.ContainsKey("HandBrake_DeleteOriginalFile"))
                {
                    HandBrake_DeleteOriginalFile = Convert.ToBoolean(values["HandBrake_DeleteOriginalFile"]);
                }

                if (values.ContainsKey("OutputDirectory"))
                {
                    OutputDirectory = values["OutputDirectory"];
                }
            }
            catch(Exception ex)
            {
                Logger.Error($"Could not interpret configuration value. {ex}", true);
            }
        }
    }
}
