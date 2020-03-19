using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Kiosky.Settings
{
    class YAMLSettings
    {       
        
        public static Settings FromFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                var deserializer = new DeserializerBuilder().Build();
                var settings = deserializer.Deserialize<Settings>(System.IO.File.OpenText(path));
                return settings;
            } else
            {
                throw new System.IO.FileNotFoundException();
            }
        }

        public static Settings FromURL(string URL)
        {
            var settingsURL = new Uri(URL);
            if(settingsURL.Scheme.Equals("http") || settingsURL.Scheme.Equals("https"))
            {
                //Only load over http or https
                var webRequest = System.Net.HttpWebRequest.CreateHttp(URL);
                var webResponse = webRequest.GetResponse();
                if(((System.Net.HttpWebResponse)webResponse).StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var deserializer = new DeserializerBuilder().Build();
                    var settings = deserializer.Deserialize<Settings>(new System.IO.StreamReader(webResponse.GetResponseStream()));
                    return settings;

                }
                else
                {
                    throw new System.Net.WebException("Could not find config");
                }
            }
            else
            {
                throw new System.Net.ProtocolViolationException("Not allowed protocol");
            }
         
        }

        public static bool ToFile(string path, Settings settings)
        {
            var serializer = new SerializerBuilder().Build();
            string settingsYAML = serializer.Serialize(settings);
            System.IO.File.WriteAllText(path, settingsYAML);
            return System.IO.File.Exists(path);
           
        }
    }
}
