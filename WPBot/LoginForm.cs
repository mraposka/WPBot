using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WPBot
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        Singleton singleton = new Singleton();
        private void button1_Click(object sender, EventArgs e)
        {
            string KullaniciAdi = textBox1.Text;
            string Sifre = textBox2.Text;

            using (WebClient client = new WebClient())
            {
                string postUrl = singleton._url + "KullaniciGiris";
                var gelenYanit = client.UploadValues(postUrl, new NameValueCollection()
               {
                   { "Kullanici", KullaniciAdi },
                   { "Sifre", Sifre }
               });
                string result = System.Text.Encoding.UTF8.GetString(gelenYanit);

                Singleton.Uygulama app = JsonConvert.DeserializeObject<Singleton.Uygulama>(result);
                if (app.Mesaj == "Ok")
                {
                    Form1 form1=new Form1();
                    this.Hide();
                    form1.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre yanlış!");
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Process.Start("whatsapp://");
        }
    }
}
