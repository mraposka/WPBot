using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WPBot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string[] hitap = { "İyi Günler!", "Sağlıklı Günler!", "Kendinize İyi Bakın!", "Sevgilerle!" };
        private void Form1_Load(object sender, EventArgs e)
        {

        } 
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string numara = listBox1.SelectedItem.ToString();
            string mesaj = textBox2.Text;
            Random random = new Random();
            string _hitap = hitap[random.Next(0, hitap.Length)];
            Process.Start("whatsapp://send?phone=" + numara + "&text=" + mesaj + " " + _hitap);
            Thread.Sleep(1000);
            SendKeys.Send("~");
        }
       
    }
}
