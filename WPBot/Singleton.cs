﻿using Newtonsoft.Json;
using System.Diagnostics;

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
            [JsonProperty(PropertyName = "IssFiltre")]
            public string IssFiltre { get; set; }  
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
        public static int toplamKayit { get; set; }
        public static int sayfaSayisi { get; set; }
        public static int sure { get; set; }
        public static int beklemeSuresi { get; set; }
        public static int limit { get; set; }

        public string _url = "http://orbilsis.com/smsapi/servis/";

        public static void Exit()
        {
            foreach (var process in Process.GetProcessesByName("WPBot"))
            {
                process.Kill();
            }
        }
    }
}
