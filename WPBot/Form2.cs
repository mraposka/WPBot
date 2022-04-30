using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WPBot
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        } 
        List<string> hitap=new List<string>(); 
        public class ComboBoxItem
        {
            string displayValue;
            string hiddenValue;

            // Constructor
            public ComboBoxItem(string d, string h)
            {
                displayValue = d;
                hiddenValue = h;
            }

            // Accessor
            public string HiddenValue
            {
                get
                {
                    return hiddenValue;
                }
            }

            // Override ToString method
            public override string ToString()
            {
                return displayValue;
            }
        }
        Singleton singleton = new Singleton();
        public void getSmsGrup()
        {
            string siteUrl = singleton._url + "smsgrup";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(siteUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader okuyucu = new StreamReader(httpResponse.GetResponseStream());
                string veri = okuyucu.ReadToEnd();
                List<Singleton.Uygulama> records = JsonConvert.DeserializeObject<List<Singleton.Uygulama>>(veri);
                for (int i = 0; i < records.Count; i++)
                {
                    var record = records[i];
                    comboBox2.Items.Add(new ComboBoxItem(record.GrupAdi, record.ID));
                }
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            getSmsGrup();
            string siteUrl = singleton._url + "hazirsablongrup";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(siteUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader okuyucu = new StreamReader(httpResponse.GetResponseStream());
                string veri = okuyucu.ReadToEnd();
                List<Singleton.Uygulama> records = JsonConvert.DeserializeObject<List<Singleton.Uygulama>>(veri);
                for (int i = 0; i < records.Count; i++)
                {
                    var record = records[i];
                    comboBox1.Items.Add(new ComboBoxItem(record.GrupAdi, record.ID));
                }
            }

            siteUrl = singleton._url + "hitapliste";
            httpWebRequest = (HttpWebRequest)WebRequest.Create(siteUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader okuyucu = new StreamReader(httpResponse.GetResponseStream());
                string veri = okuyucu.ReadToEnd();
                List<Singleton.Uygulama> records = JsonConvert.DeserializeObject<List<Singleton.Uygulama>>(veri);
                for (int i = 0; i < records.Count; i++)
                {
                    var record = records[i];
                    hitap.Add(record.Icerik);
                }
            }
        } 
        public void telefonCek()
        { 
            string hValue = ((ComboBoxItem)comboBox2.SelectedItem).HiddenValue;
            if (Singleton.limit != 0) { 
                 for(int i=0; i<Singleton.sayfaSayisi; i++)
                {
                    string siteUrl = singleton._url + "smsliste/" + hValue + "/" + i; 
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(siteUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "GET";
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        StreamReader okuyucu = new StreamReader(httpResponse.GetResponseStream());
                        string veri = okuyucu.ReadToEnd();
                        List<Singleton.Uygulama> records = JsonConvert.DeserializeObject<List<Singleton.Uygulama>>(veri);
                        for (int j = 0; j < records.Count; j++)
                        {
                            var record = records[j];
                            if (record.Durum == "1") listBox1.Items.Add(record.Telefon);
                        }
                    }
                }
            }
            else { 
                MessageBox.Show("Limit yok");
                //Config çekilcek
            }
            
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        { 
            string grupId = ((ComboBoxItem)comboBox1.SelectedItem).HiddenValue;
            string siteUrl = singleton._url + "hazirsablonliste/"+grupId;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(siteUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                StreamReader okuyucu = new StreamReader(httpResponse.GetResponseStream());
                string veri = okuyucu.ReadToEnd();
                List<Singleton.Uygulama> records = JsonConvert.DeserializeObject<List<Singleton.Uygulama>>(veri);
                for (int i = 0; i < records.Count; i++)
                {
                    var record = records[i];
                    menuStrip1.Items.Add(record.Baslik);
                    ToolStripItem item = menuStrip1.Items[menuStrip1.Items.Count - 1];
                    item.Tag = record.ID+":"+record.Icerik;
                    //item.BackColor = menuStrip1.BackColor;
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        { 
            textBox1.Text = e.ClickedItem.Tag.ToString().Split(':')[1];
            Random rnd = new Random();
            int r = rnd.Next(hitap.Count);
            textBox1.Text += " " + hitap[r];
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            telefonCek();
        }
    }
}
