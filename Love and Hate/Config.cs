using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Love_and_Hate
{
    class Config
    {
        static Config m_instance = new Config();

        private Dictionary<string, string> mProps;

        public Config()
        {
            mProps = new Dictionary<string,string>();

            mProps.Add("PlayerMergeBtn","b");
            mProps.Add("PlayerBreakMergeBtn","x");
            mProps.Add("PlayerSpeed","10");
            mProps.Add("PlayerFrameRate","15");
            mProps.Add("PlayerBoundingRadius","35");
            mProps.Add("Friction","0.90");
            mProps.Add("MaxEnemies","10");
            mProps.Add("ScreenWidth","1280");
            mProps.Add("ScreenHeight","800");
            mProps.Add("TimeLimit","90");        
        }

        public static Config Instance
        {
            get { return m_instance; }
        }

        public String GetAsString(String value)
        {
            String val = mProps[value]; //ConfigurationSettings.AppSettings[value];
            return val == null ? "" : val;
        }

        public int GetAsInt(String value)
        {
            String val = GetAsString(value);
            return System.Convert.ToInt32(val);
        }

        public double GetAsDouble(String value)
        {
            String val = GetAsString(value);
            return System.Convert.ToDouble(val);
        }
    }
}
