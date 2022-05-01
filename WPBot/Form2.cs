﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        List<string> hitap = new List<string>();
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
            if (Singleton.limit != 0)
            {
                for (int i = 0; i < Singleton.sayfaSayisi; i++)
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
                            if (record.Durum == "1"&&record.IssFiltre=="1") listBox1.Items.Add("+90" + record.Telefon);
                        }
                    }
                }
            }
            else
            { 
                //Config çekilcek
                listBox1.Items.Clear();
                string siteUrl = singleton._url + "SmsListeToplamLimit/" + hValue;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(siteUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader okuyucu = new StreamReader(httpResponse.GetResponseStream());
                    string veri = okuyucu.ReadToEnd();
                    Singleton.Uygulama record = JsonConvert.DeserializeObject<Singleton.Uygulama>(veri);
                    Singleton.sayfaSayisi = Int32.Parse(record.SayfaSayisi);
                    Singleton.limit = Int32.Parse(record.Limit);
                    Singleton.toplamKayit = Int32.Parse(record.ToplamKayit);
                    Singleton.sure = Int32.Parse(record.Sure);
                    Singleton.beklemeSuresi = Int32.Parse(record.BeklemeSuresi);
                    telefonCek();
                }
            }

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string grupId = ((ComboBoxItem)comboBox1.SelectedItem).HiddenValue;
            string siteUrl = singleton._url + "hazirsablonliste/" + grupId;
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
                    icerik.Add(record.Icerik);
                    item.Tag = record.ID + ":" + record.Icerik;
                    //item.BackColor = menuStrip1.BackColor;
                }
            }
        }
        List<string> icerik=new List<string>();
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
        private void Gönder(string tel)
        {
            string mesaj = textBox1.Text;
            Random random = new Random(); 
            Process.Start("whatsapp://send?phone=" + tel + "&text=" + icerik[random.Next(0,icerik.Count)] + hitap[random.Next(hitap.Count)]);
            Thread.Sleep(1000);
            SendKeys.Send("~");
        }
        public void Gonderildi(string tel)
        {
            using (WebClient client = new WebClient())
            {
                string postUrl = singleton._url + "mesajgonderildi";
                var gelenYanit = client.UploadValues(postUrl, new NameValueCollection()
               {
                   { "Telefon", tel }
               }); 
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {  
            int mesaj = 0;
            for (int sayfa = 0; sayfa < Singleton.sayfaSayisi; sayfa++)
            {
                if (listBox1.Items.Count < Singleton.limit)
                {
                    for (int mesajSay = 0; mesajSay < listBox1.Items.Count; mesajSay++)
                    {
                        mesajIndex.Text = (mesajSay + 1).ToString() + ". mesaj gönderiliyor!";
                        Thread.Sleep(100);
                        Gönder(listBox1.Items[mesajSay].ToString());
                        mesaj++;
                        mesajIndex.Text = (mesajSay + 1).ToString() + ". mesaj gönderildi!";
                        //gonderildi info
                        Thread.Sleep(100);
                        Thread.Sleep(Singleton.sure * 1000);
                    }
                }
                else
                {
                    for (int limit = sayfa * Singleton.limit; limit <= Singleton.limit + (sayfa * Singleton.limit); limit++)
                    {
                        mesajIndex.Text = (limit + 1).ToString() + ". mesaj gönderiliyor!";
                        Thread.Sleep(100);
                        Gönder(listBox1.Items[limit].ToString());
                        mesaj++;
                        mesajIndex.Text = (limit + 1).ToString() + ". mesaj gönderildi!";
                        //gonderildi info
                        Thread.Sleep(100);
                        Thread.Sleep(Singleton.sure * 1000);
                        if (mesaj == Singleton.toplamKayit)
                        {
                            break;
                        }
                    }
                }
                //molaSure.Text = "Mola verildi!";
                Thread.Sleep(Singleton.beklemeSuresi * 1000);//*60
                //molaSure.Text = "Mola Bitti!";
                Thread.Sleep(100);
                //molaSure.Text = "Mola Bitti!";
                if (mesaj == listBox1.Items.Count)
                {
                    break;
                }
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            /*DialogResult dialogResult = MessageBox.Show("Programı sonlandırmak istiyor musunuz?", "Program Sonlandırılıyor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
                //Kapanıyor
            }
            else if (dialogResult == DialogResult.No)
            {
                //Kapanmadı
                e.Cancel = true;
            }*/
        }

        private void telefonDefteriFiltreleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.ShowDialog();
        }
    }
}
