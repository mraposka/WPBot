﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
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
        Bitmap screenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

        private void button3_Click(object sender, EventArgs e)
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

            if(IsInCapture(myPic, screenCapture))
            {
                MessageBox.Show("Numara Aktif");
            }
            else
            {
                MessageBox.Show("Numara aktif değil");
            }
        }
    }
}