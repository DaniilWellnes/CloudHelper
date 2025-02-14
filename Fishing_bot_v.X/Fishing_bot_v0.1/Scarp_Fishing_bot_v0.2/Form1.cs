using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scarp_Fishing_bot_v0._2
{
    public partial class Form1 : Form
    {
        // Импорт функций Windows API для работы с экраном
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hdc, int x, int y);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        private const byte VK_SPACE = 0x20;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        public Form1()
        {
            InitializeComponent();
            this.MouseDown += new MouseEventHandler((o, e) =>
            {
                base.Capture = false;
                Message message = Message.Create(base.Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref message);
            });
            this.TopMost = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }




        // Метод для получения цвета пикселя
        private static Color GetPixelColor(IntPtr hdc, int x, int y)
        {
            uint pixel = GetPixel(hdc, x, y);
            int r = (int)(pixel & 0x000000FF);
            int g = (int)((pixel & 0x0000FF00) >> 8);
            int b = (int)((pixel & 0x00FF0000) >> 16);
            return Color.FromArgb(r, g, b);
        }

        // Метод для проверки цвета пикселя
        private static bool IsPixelColor(Color color, int r, int g, int b)
        {
            return color.R == r && color.G == g && color.B == b;
        }

        // Метод для нажатия пробела
        private static void PressSpace()
        {
            keybd_event(VK_SPACE, 0, 0, 0);
            keybd_event(VK_SPACE, 0, KEYEVENTF_KEYUP, 0);
            //Console.WriteLine("Пробел нажат!");

        }




        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked=true)
            {
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
