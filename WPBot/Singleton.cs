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

            [JsonProperty(PropertyName = "Mesaj")]
            public string Mesaj { get; set; } 
            [JsonProperty(PropertyName = "ID")]
            public string ID { get; set; } 
            [JsonProperty(PropertyName = "GrupAdi")]
            public string GrupAdi { get; set; } 
            [JsonProperty(PropertyName = "sayac")]
            public string Sayac { get; set; } 
            [JsonProperty(PropertyName = "Adi")]
            public string Adi { get; set; } 
            [JsonProperty(PropertyName = "Soyadi")]
            public string Soyadi { get; set; }
            [JsonProperty(PropertyName = "Telefon")]
            public string Telefon { get; set; }
            [JsonProperty(PropertyName = "GrupID")]
            public string GrupID { get; set; }
            [JsonProperty(PropertyName = "Durum")]
            public string Durum { get; set; }
            [JsonProperty(PropertyName = "Cinsiyet")]
            public string Cinsiyet { get; set; }
             
        }
        public string _url = "http://orbilsis.com/smsapi/servis/";
    }
}
