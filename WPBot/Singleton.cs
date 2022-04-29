using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPBot
{
    internal class Singleton
    {
        public class Uygulama
        {
            [JsonProperty(PropertyName = "ID")]
            public string ID { get; set; }

            [JsonProperty(PropertyName = "Mesaj")]
            public string Mesaj { get; set; }

            [JsonProperty(PropertyName = "GrupAdi")]
            public string GrupAdi { get; set; }
        }
        public string _url = "http://orbilsis.com/smsapi/servis/";
    }
}
