using Newtonsoft.Json;
using System;
using Sayac = System.Timers;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace WPBot
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            sayac = new System.Timers.Timer();
            sayac.Interval = 1000;
            sayac.Elapsed += OnTimedEvent;
            sayac.AutoReset = true;
        }
        List<string> hitap = new List<string>();
        List<string> icerik = new List<string>();
        Singleton singleton = new Singleton();
        Sayac.Timer sayac;
        NotifyIcon notify_Icon = new NotifyIcon(); 
        bool gonderimDurdur = false;
        int _sure = 0;
        int sure = 0;
        public class ComboBoxItem
        {
            string displayValue;
            string hiddenValue;
            public ComboBoxItem(string d, string h)
            {
                displayValue = d;
                hiddenValue = h;
            }
            public string HiddenValue
            {
                get { return hiddenValue; }
            }
            public override string ToString()
            {
                return displayValue;
            }
        }
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
            notify_Icon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
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
                            if (record.Durum == "1" && record.IssFiltre == "1") listBox1.Items.Add("+90" + record.Telefon);
                        }
                    }
                }
            }
            else
            {
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
            menuStrip1.Items.Clear();
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
        private void Gönder(string tel)
        {

            SendKeys.Send("~");
            Random random = new Random();
            Process.Start("whatsapp://send?phone=" + tel + "&text=" + icerik[random.Next(0, icerik.Count)] + hitap[random.Next(hitap.Count)]);
            Thread.Sleep(2000);
            SendKeys.Send("~");
        }
        public void Gonderildi(string tel)
        {
            using (WebClient client = new WebClient())
            {
                string postUrl = singleton._url + "mesajgonderildi";
                client.UploadValues(postUrl, new NameValueCollection()
                {
                    { "Telefon", tel }
                });
            }
        }
        public void LabelDegis(string text)
        {
            label1.Invoke(new Action(() =>
            {
                molaSure.Text = text;
            }));
        }
        private void OnTimedEvent(object obj, Sayac.ElapsedEventArgs e)
        {
            if (_sure == sure)
            {
                _sure = 0;
                sure = 0; 
                sayac.Enabled = false;
                LabelDegis("Mola Bekleniyor!");
            }
            else
            { 
                _sure++;
                LabelDegis(_sure.ToString() + ". saniye/" + sure.ToString());
            }

        } 
        public async Task Bekle(int sure)
        {
            await Task.Delay(sure * 1000);
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            try{ int mesaj = 0;
            for (int sayfa = 0; sayfa < Singleton.sayfaSayisi; sayfa++)
            {
                if (listBox1.Items.Count < Singleton.limit)
                {
                    for (int mesajSay = 0; mesajSay < listBox1.Items.Count; mesajSay++)
                    {
                        if (!gonderimDurdur)
                        {
                            mesajIndex.Text = (mesajSay + 1).ToString() + ". mesaj gönderiliyor!";
                            listBox1.SelectedIndex = mesajSay;
                            Gönder(listBox1.Items[mesajSay].ToString());
                            mesaj++;
                            mesajIndex.Text = (mesajSay + 1).ToString() + ". mesaj gönderildi!";
                            //gonderildi info 
                            if (mesaj % Singleton.limit == 0)
                            {
                                sure = Singleton.beklemeSuresi;
                                sayac.Enabled = true;

                                await Bekle(Singleton.beklemeSuresi);
                            }
                            else
                            {
                                sure = Singleton.sure;
                                sayac.Enabled = true;

                                await Bekle(Singleton.sure);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Gönderim durduruluyor. " + mesajIndex.Text.Split('.')[0] + ". mesajda durduruldu.");
                            LabelDegis("Gönderim " + mesajIndex.Text.Split('.')[0] + ". sırada durduruldu.");
                        }
                    }
                }
                else if (listBox1.Items.Count >= Singleton.limit)
                {
                    for (int limit = sayfa * Singleton.limit; limit <= Singleton.limit + (sayfa * Singleton.limit); limit++)
                    {
                        if (!gonderimDurdur)
                        {
                            mesajIndex.Text = (limit + 1).ToString() + ". mesaj gönderiliyor!";
                            listBox1.SelectedIndex = limit;
                            Gönder
                                (listBox1.Items[limit].ToString());
                            mesaj++;
                            mesajIndex.Text = (limit + 1).ToString() + ". mesaj gönderildi!";

                            if (mesaj % Singleton.limit == 0)
                            {
                                sure = Singleton.beklemeSuresi;
                                sayac.Enabled = true;

                                await Bekle(Singleton.beklemeSuresi);
                            }
                            else
                            {
                                sure = Singleton.sure;
                                sayac.Enabled = true;

                                await Bekle(Singleton.sure);
                            }

                            if (mesaj == Singleton.toplamKayit)
                            {
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Gönderim durduruluyor. " + mesajIndex.Text.Split('.')[0] + ". mesajda durduruldu.");
                            LabelDegis("Gönderim " + mesajIndex.Text.Split('.')[0] + ". sırada durduruldu.");
                        }
                    }
                }

                if (mesaj == listBox1.Items.Count)
                {
                    NotifyIcon();
                    break;
                }
            }

            }
            catch(Exception ex) { MessageBox.Show("Hata oluştu." + ex.Message); }
        }
        void NotifyIcon()
        {
            notify_Icon.Visible = true;
            notify_Icon.Text = "Gönderim Tamamlandı!";
            notify_Icon.BalloonTipTitle = "Mesaj Gönderimi";
            notify_Icon.BalloonTipText = "Gönderim Tamamlandı!";
            notify_Icon.BalloonTipIcon = ToolTipIcon.Info;
            notify_Icon.ShowBalloonTip(2000);
            SystemSounds.Beep.Play();
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Programı sonlandırmak istiyor musunuz?", "Program Sonlandırılıyor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                Singleton.Exit();
                //Kapanıyor
            }
            else if (dialogResult == DialogResult.No)
            {
                //Kapanmadı
                e.Cancel = true;
            }
        }
        private void telefonDefteriFiltreleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.ShowDialog();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                gonderimDurdur = !gonderimDurdur; 
                if (!gonderimDurdur)
                {
                    button2.BackColor = Form.DefaultBackColor;
                    button2.BackgroundImage = WPBot.Properties.Resources.play;
                    LabelDegis("Gönderim devam ettiriliyor!");

                    sure = 3;
                    sayac.Enabled = true;

                    await Bekle(3);
                    int _mesajIndex = Convert.ToInt32(mesajIndex.Text.Split('.')[0]);
                    _mesajIndex--;
                    for (int i = _mesajIndex; i >= 0; i--)
                    {
                        listBox1.Items.RemoveAt(i);
                    }
                }
                else
                {
                    button2.BackColor = Color.Gray;
                    button2.BackgroundImage = WPBot.Properties.Resources.pause;
                }
            }
            catch (Exception)
            {
                gonderimDurdur = true;
                LabelDegis("Hata oluştu. Gönderim durduruldu. "+ mesajIndex.Text.Split('.')[0]+". sırada durduruldu.");
            }
            
        }

    }
}
