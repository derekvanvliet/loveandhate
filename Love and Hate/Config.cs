using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Love_and_Hate
{
    class Config
    {
        static Config m_instance = new Config();

        public Config()
        {
        
        }

        public static Config Instance
        {
            get { return m_instance; }
        }

        public String GetAsString(String value)
        {
            String val = ConfigurationSettings.AppSettings[value];
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
