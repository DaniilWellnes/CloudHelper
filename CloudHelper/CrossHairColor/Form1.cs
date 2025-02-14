using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrossHairColor
{
    public partial class Form1 : Form
    {
        private Label coordinatesLabel;
        private Label colorLabel;
        private Timer timer;
        public Form1()
        {
            InitializeComponent();

            this.MouseDown += new MouseEventHandler((o, e) =>
            {
                base.Capture = false;
                Message message = Message.Create(base.Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref message);
            });

            this.FormBorderStyle = FormBorderStyle.None; // Убираем рамку окна
            this.TopMost = true; // Окно всегда поверх других окон
            this.DoubleBuffered = true;

            // Настройка формы
            this.Text = "Mouse Coordinates and Color";
            this.Size = new Size(300, 100);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Инициализация Label для координат
            coordinatesLabel = new Label
            {
                AutoSize = true,
                Location = new Point(10, 10),
                Text = "Coordinates: (0, 0)"
            };
            this.Controls.Add(coordinatesLabel);

            // Инициализация Label для цвета пикселя
            colorLabel = new Label
            {
                ForeColor = Color.Black,
                AutoSize = true,
                Location = new Point(10, 40),
                Text = "Color: RGB(0, 0, 0)"
            };
            this.Controls.Add(colorLabel);

            // Инициализация и настройка таймера
            timer = new Timer
            {
                Interval = 1000 // 1 секунда
            };
            timer.Tick += Timer_Tick; // Подписываемся на событие Tick
            timer.Start(); // Запускаем таймер
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            // Получаем текущие координаты мыши относительно формы
            Point mousePos = this.PointToClient(Cursor.Position);

            // Обновляем Label с координатами
            coordinatesLabel.Text = $"Coordinates: ({mousePos.X}, {mousePos.Y})";

            // Получаем цвет пикселя под курсором
            using (Bitmap bmp = new Bitmap(1, 1))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Копируем цвет пикселя с экрана
                    g.CopyFromScreen(Cursor.Position, Point.Empty, new Size(1, 1));
                }

                // Получаем цвет пикселя
                Color pixelColor = bmp.GetPixel(0, 0);

                // Обновляем Label с цветом в формате RGB
                colorLabel.Text = $"Color: RGB({pixelColor.R}, {pixelColor.G}, {pixelColor.B})";
                colorLabel.BackColor = pixelColor; // Опционально: меняем фон Label на цвет пикселя
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Получаем текущие координаты мыши относительно экрана
            Point mousePos = Cursor.Position;

            // Обновляем Label с координатами
            coordinatesLabel.Text = $"Coordinates: ({mousePos.X}, {mousePos.Y})";

            // Получаем цвет пикселя под курсором
            using (Bitmap bmp = new Bitmap(1, 1))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Копируем цвет пикселя с экрана
                    g.CopyFromScreen(mousePos, Point.Empty, new Size(1, 1));
                }

                // Получаем цвет пикселя
                Color pixelColor = bmp.GetPixel(0, 0);

                // Обновляем Label с цветом в формате RGB
                colorLabel.Text = $"Color: RGB({pixelColor.R}, {pixelColor.G}, {pixelColor.B})";
                colorLabel.BackColor = pixelColor; // Опционально: меняем фон Label на цвет пикселя
            }
        }
    }
}
