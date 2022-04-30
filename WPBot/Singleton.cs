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
        [JsonProperty(PropertyName = "ToplamKayit")]
        public string ToplamKayit { get; set; } 
        [JsonProperty(PropertyName = "Limit")]
        public string Limit { get; set; }


        [JsonProperty(PropertyName = "SayfaSayisi")]
        public string SayfaSayisi { get; set; }


        [JsonProperty(PropertyName = "Sure")]
        public string Sure { get; set; }


        [JsonProperty(PropertyName = "BeklemeSuresi")]
        public string BeklemeSuresi { get; set; }

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

            [JsonProperty(PropertyName = "Icerik")]
            public string Icerik { get; set; }
            [JsonProperty(PropertyName = "Baslik")]
            public string Baslik { get; set; }
            [JsonProperty(PropertyName = "Cinsiyet")]
            public string Cinsiyet { get; set; }

            [JsonProperty(PropertyName = "Hitap")]
            public string Hitap { get; set; }

        }
        public int toplamKayit = 0;
        public int sayfaSayisi = 0;
        public int sure = 0;
        public int beklemeSuresi = 0;
        public int limit = 0;

        public string _url = "http://orbilsis.com/smsapi/servis/";
    }
}
