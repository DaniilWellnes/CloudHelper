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

namespace WindowsFormsApp1
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
            this.TopMost = true;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const int xStart = 686, yStart = 895;
            const int xEnd = 1221;
            const int searchColorR1 = 74, searchColorG1 = 223, searchColorB1 = 57;
            const int searchColorR2 = 248, searchColorG2 = 248, searchColorB2 = 248;
            const int searchColorR3 = 166, searchColorG3 = 166, searchColorB3 = 166;
            const int x2 = 1432, y2 = 959;

            IntPtr hdc = GetDC(IntPtr.Zero); // Получение контекста устройства экрана

            while (true)
            {
                // Поиск первого пикселя с цветом RGB = 74, 223, 57
                int foundX = -1;
                for (int x = xStart; x <= xEnd; x++)
                {
                    Color color = GetPixelColor(hdc, x, yStart);
                    if (IsPixelColor(color, searchColorR1, searchColorG1, searchColorB1))
                    {
                        foundX = x;
                        Console.WriteLine($"Пиксель найден на координате x: {foundX}");
                        break;
                    }
                }

                if (foundX != -1)
                {
                    // Проверка двух точек (x+40, y) и (x-40, y)
                    while (true)
                    {
                        Color color1 = GetPixelColor(hdc, foundX + 40, yStart);
                        Color color2 = GetPixelColor(hdc, foundX - 40, yStart);

                        if (IsPixelColor(color1, searchColorR2, searchColorG2, searchColorB2) ||
                            IsPixelColor(color2, searchColorR2, searchColorG2, searchColorB2))
                        {
                            PressSpace();
                            break;
                        }

                        Thread.Sleep(100); // Задержка перед следующей проверкой
                    }
                }

                // Задержка перед началом следующего поиска
                Thread.Sleep(3000);

                // Поиск второго пикселя с цветом RGB = 166, 166, 166
                while (true)
                {
                    Color color = GetPixelColor(hdc, x2, y2);
                    if (IsPixelColor(color, searchColorR3, searchColorG3, searchColorB3))
                    {
                        PressSpace();
                        break;
                    }

                    Thread.Sleep(100); // Задержка перед следующей проверкой
                }

                // Задержка перед новой итерацией
                Thread.Sleep(3000);
            }

            ReleaseDC(IntPtr.Zero, hdc); // Освобождение контекста устройства
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
            Console.WriteLine("Пробел нажат!");
        }

    }
}
