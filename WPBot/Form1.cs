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
        string[] hitap = { "İyi Günler!", "Sağlıklı Günler!", "Kendinize İyi Bakın!", "Sevgilerle!" };
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

        private void button2_Click(object sender, EventArgs e)
        {
            string numara = listBox1.SelectedItem.ToString();
            string mesaj = "asd";
            Random random = new Random();
            string _hitap = hitap[random.Next(0, hitap.Length)];
            Process.Start("whatsapp://send?phone=" + numara + "&text=" + mesaj + " " + _hitap);
            Thread.Sleep(1000);
            SendKeys.Send("~");
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

        private void Test()
        {
            //Process.Start("whatsapp://send?phone=" + textBox3.Text);
            Thread.Sleep(2000);
            Bitmap ImgToFind1 = new Bitmap(@"C:\Users\Can\Desktop\img.png");

            Graphics g = Graphics.FromImage(screenCapture);

            g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                             Screen.PrimaryScreen.Bounds.Y,
                             0, 0,
                             screenCapture.Size,
                             CopyPixelOperation.SourceCopy);

            Bitmap myPic = ImgToFind1;

            if (IsInCapture(myPic, screenCapture))
            {
                MessageBox.Show("Numara Aktif");
            }
            else
            {
                MessageBox.Show("Numara aktif değil");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Programı sonlandırmak istiyor musunuz?", "Program Sonlandırılıyor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                //Kapanıyor
            }
            else if (dialogResult == DialogResult.No)
            {
                //Kapanmadı
                e.Cancel = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
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
                 
                if (result == "1")
                {
                     
                }
                else  
                {
                    TelefonDeaktif(tel);
                    MessageBox.Show("Hata Algılandı!","Tekrar Deneniyor!");
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string hValue = ((ComboBoxItem)comboBox1.SelectedItem).HiddenValue;
            string siteUrl = singleton._url + "smsliste/" + hValue;
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

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                string tel = listBox1.Items[i].ToString();
                Process.Start("whatsapp://send?phone=" + tel);
                Thread.Sleep(2000);
                Bitmap ImgToFind1 = new Bitmap(@"C:\Users\Can\Desktop\img.png");

                Graphics g = Graphics.FromImage(screenCapture);

                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                 Screen.PrimaryScreen.Bounds.Y,
                                 0, 0,
                                 screenCapture.Size,
                                 CopyPixelOperation.SourceCopy);

                Bitmap myPic = ImgToFind1;

                if (IsInCapture(myPic, screenCapture))
                {
                    listBox3.Items.Add(tel); 
                }
                else
                {
                    listBox2.Items.Add(tel);
                    TelefonDeaktif(tel);
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

        private void button3_Click(object sender, EventArgs e)
        {
            Test();
        }
    }
}
