using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Media;
using System.Reflection;

namespace WPBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
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
        Bitmap screenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        Singleton singleton = new Singleton(); 
        private void Form1_Load(object sender, EventArgs e)
        {
            GetSmsGrup();

            notify_Icon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
        }

        private void GetSmsGrup()
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
                    comboBox1.Items.Add(new ComboBoxItem(record.GrupAdi, record.ID));
                }
            }
        } 
        private bool IsInCapture(Bitmap searchFor, Bitmap searchIn)
        {
            for (int x = 0; x < searchIn.Width; x++)
            {
                for (int y = 0; y < searchIn.Height; y++)
                {
                    bool invalid = false;
                    int k = x, l = y;
                    for (int a = 0; a < searchFor.Width; a++)
                    {
                        l = y;
                        for (int b = 0; b < searchFor.Height; b++)
                        {
                            if (searchFor.GetPixel(a, b) != searchIn.GetPixel(k, l))
                            {
                                invalid = true;
                                break;
                            }
                            else
                                l++;
                        }
                        if (invalid)
                            break;
                        else
                            k++;
                    }
                    if (!invalid)
                        return true;
                }
            }
            return false;
        } 
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            /* DialogResult dialogResult = MessageBox.Show("Programı sonlandırmak istiyor musunuz?", "Program Sonlandırılıyor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
         
        public void TelefonDeaktif(string tel)
        {

            using (WebClient client = new WebClient())
            {
                string postUrl = singleton._url + "whatsappkontrol";
                var gelenYanit = client.UploadValues(postUrl, new NameValueCollection()
               {
                   { "Telefon", tel }
               });
                string result = System.Text.Encoding.UTF8.GetString(gelenYanit);

                if (result != "1")
                { 
                    MessageBox.Show("Hata Algılandı!", "Tekrar Deneniyor!");
                    TelefonDeaktif(tel);
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear(); 
            string hValue = ((ComboBoxItem)comboBox1.SelectedItem).HiddenValue;
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
            }
            for (int i = 0; i < Singleton.sayfaSayisi; i++)
            {
                siteUrl = singleton._url + "smsliste/" + hValue + "/" + i; 
                httpWebRequest = (HttpWebRequest)WebRequest.Create(siteUrl);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader okuyucu = new StreamReader(httpResponse.GetResponseStream());
                    string veri = okuyucu.ReadToEnd();
                    List<Singleton.Uygulama> records = JsonConvert.DeserializeObject<List<Singleton.Uygulama>>(veri);
                    for (int j = 0; j < records.Count; j++)
                    {
                        var record = records[j];
                        if(record.Durum=="1")listBox1.Items.Add("+90"+record.Telefon);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string tel = listBox1.Items[i].ToString();
                Process.Start("whatsapp://send?phone=" + tel);
                Thread.Sleep(2000);
                Bitmap ImgToFind1 = new Bitmap(@"img.png");

                Graphics g = Graphics.FromImage(screenCapture);

                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                 Screen.PrimaryScreen.Bounds.Y,
                                 0, 0,
                                 screenCapture.Size,
                                 CopyPixelOperation.SourceCopy);

                Bitmap myPic = ImgToFind1;

                if (IsInCapture(myPic, screenCapture))
                {
                    listBox1.Items.Remove(tel);
                    TelefonDeaktif(tel.Replace("+90", ""));
                }
                else
                { 
                }
            }
            NotifyIcon();
        }
        NotifyIcon notify_Icon = new NotifyIcon();
        void NotifyIcon()
        {
            notify_Icon.Visible = true;
            notify_Icon.Text = "Kontrol Tamamlandı!";
            notify_Icon.BalloonTipTitle = "Kontrol";
            notify_Icon.BalloonTipText = "Kontrol Tamamlandı!";
            notify_Icon.BalloonTipIcon = ToolTipIcon.Info;
            notify_Icon.ShowBalloonTip(2000);
            SystemSounds.Beep.Play();
        }
         
        private void mesajGönderToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.ShowDialog();
        }
    }
}
