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
        private void Form2_Load(object sender, EventArgs e)
        {
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
        int _sayac = 1;
        public void telefonCek(int sayac)
        {
            string hValue = ((ComboBoxItem)comboBox1.SelectedItem).HiddenValue;
            string siteUrl = singleton._url + "smsliste/" + hValue+"/"+sayac;
            MessageBox.Show(siteUrl);
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
                    listBox1.Items.Add(record.Telefon);
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            telefonCek(_sayac);

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
                }
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            MessageBox.Show(e.ClickedItem.Tag.ToString());
            textBox1.Text = e.ClickedItem.Tag.ToString().Split(':')[1];
            Random rnd = new Random();
            int r = rnd.Next(hitap.Count);
            textBox1.Text += " " + hitap[r];
        }
    }
}
