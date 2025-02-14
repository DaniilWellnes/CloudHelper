using KeyAuth;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Timer = System.Windows.Forms.Timer;

namespace Cloud_loader_release
{
    public partial class Login : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr hdc, int x, int y);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        // Константы для нажатия клавиш и мыши
        private const int KEYEVENTF_KEYDOWN = 0x0000;
        private const int KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(MouseEvent dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const byte VK_E = 0x45; // Код клавиши 'E'

        private bool ert = false;
        private int num = 1;

        public Login()
        {
            api.KeyAuthApp.init();
            InitializeComponent();
            this.MouseDown += new MouseEventHandler((o, e) =>
            {
                base.Capture = false;
                Message message = Message.Create(base.Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
                this.WndProc(ref message);
            });
            Opacity = 0;
            Timer timer = new Timer();
            timer.Tick += new EventHandler((sender, e) =>
            {
                if ((Opacity += 0.1d) == 1) timer.Stop();
            });
            timer.Interval = 20;
            timer.Start();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            user_key.UseSystemPasswordChar = true;
            user_pass.UseSystemPasswordChar = true;
            this.Size = new System.Drawing.Size(440, 350);

            settings.Location = new Point(344, 14);
            settings1.Location = new Point(344, 14);

            panel1.Location = new Point(14, 95);
            panel2.Location = new Point(14, 90);
            panel_settings.Location = new Point(12, 90);

            panel2.Enabled = false; panel2.Visible = false;
            panel_settings.Enabled = false; panel_settings.Visible = false;

            start_load.Enabled = false;

            user_name.Text = "admin";
            user_pass.Text = "admin";
        }

        private void close_app_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
/*            Application.ExitThread();
            Application.Exit();*/
        }

        private void open_pass_Click(object sender, EventArgs e)
        {
            user_pass.UseSystemPasswordChar = false;
            open_pass.Visible = false; open_pass.Enabled = false;
            close_pass.Visible = true; close_pass.Enabled = true;
        }

        private void close_pass_Click(object sender, EventArgs e)
        {
            user_pass.UseSystemPasswordChar = true;
            open_pass.Visible = true; open_pass.Enabled = true;
            close_pass.Visible = false; close_pass.Enabled = false;
        }
        private void open_key_Click(object sender, EventArgs e)
        {
            user_key.UseSystemPasswordChar = false;
            open_key.Visible = false; open_key.Enabled = false;
            close_key.Visible = true; close_key.Enabled = true;
        }

        private void close_key_Click(object sender, EventArgs e)
        {
            user_key.UseSystemPasswordChar = true;
            open_key.Visible = true; open_key.Enabled = true;
            close_key.Visible = false; close_key.Enabled = false;
        }
        private void status_num(string st)
        {
            status.Text = "Status " + num + ": " + st;
            num = num + 1;
        }
        private void Button_login_Click(object sender, EventArgs e)
        {
            api.KeyAuthApp.init();
            api.KeyAuthApp.login(user_name.Text, user_pass.Text);
            status_num(api.KeyAuthApp.response.message);
            if (api.KeyAuthApp.response.success) { gif(); }
        }
        private void Button_register_Click(object sender, EventArgs e)
        {
            api.KeyAuthApp.init();
            api.KeyAuthApp.register(user_name.Text, user_pass.Text, user_key.Text);
            status_num(api.KeyAuthApp.response.message);
        }
        private void Button_upgrade_Click(object sender, EventArgs e)
        {
            api.KeyAuthApp.init();
            api.KeyAuthApp.upgrade(user_name.Text, user_key.Text);
            status_num(api.KeyAuthApp.response.message);
        }
        private void Button_switch_Click(object sender, EventArgs e) //register
        {
            Button_register.Enabled = true;
            Button_register.Visible = true;
            Button_upgrade.Enabled = true;
            Button_upgrade.Visible = true;
            Button_login.Enabled = false;
            Button_login.Visible = false;
            Button_switch_login.Enabled = true;
            Button_switch_login.Visible = true;
            Button_switch_register.Visible = false;
            Button_switch_register.Enabled = false;

            user_key.Visible = true;
            user_key.Enabled = true;

            open_key.Visible = true;
            open_key.Enabled = true;

            close_key.Visible = true;
            close_key.Enabled = true;

            label3.Visible = true;
            label3.Enabled = true;
        }

        private void Button_switch_login_Click(object sender, EventArgs e)
        {
            Button_register.Enabled = false;
            Button_register.Visible = false;
            Button_upgrade.Enabled = false;
            Button_upgrade.Visible = false;
            Button_login.Enabled = true;
            Button_login.Visible = true;
            Button_switch_login.Enabled = false;
            Button_switch_login.Visible = false;
            Button_switch_register.Visible = true;
            Button_switch_register.Enabled = true;

            user_key.Visible = false;
            user_key.Enabled = false;

            open_key.Visible = false;
            open_key.Enabled = false;

            close_key.Visible = false;
            close_key.Enabled = false;

            label3.Visible = false;
            label3.Enabled = false;
            user_key.Text = "";
        }

        private void choose_work_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void panel_ID_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel_status_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel_last_upd_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel_sub_Paint(object sender, PaintEventArgs e)
        {

        }
        private void settings_Click(object sender, EventArgs e)
        {
            choose_work.Enabled = false; choose_work.Visible = false;
            panel1.Enabled = false; panel1.Visible = false;
            panel_settings.Visible = true; panel_settings.Enabled = true; settings1.Enabled = true; settings1.Visible = true; settings.Enabled = false; settings.Visible = false;
        }
        private void settings1_Click(object sender, EventArgs e)
        {
            choose_work.Enabled = true; choose_work.Visible = true;
            panel1.Enabled = true; panel1.Visible = true;
            panel_settings.Visible = false; panel_settings.Enabled = false; settings1.Enabled = false; settings1.Visible = false; settings.Enabled = true; settings.Visible = true;
        }
        private async void check_detect()
        {
            api.KeyAuthApp.var("detect_helper");
            string detect = api.KeyAuthApp.response.message;
            if (detect.Contains("0"))
            {
                status_cheat.Text = "UNDETECT"; status_cheat.ForeColor = System.Drawing.Color.LimeGreen;
                pic_undetect.Visible = true; pic_undetect.Enabled = true;
                start_load.Enabled = true;
            }
            if (detect.Contains("1"))
            {
                status_cheat.Text = "DETECT"; status_cheat.ForeColor = System.Drawing.Color.Red;
                pic_detect.Visible = true; pic_detect.Enabled = true;
            }
            if (detect.Contains("2"))
            {
                status_cheat.Text = "on update"; status_cheat.ForeColor = System.Drawing.Color.Blue;
                pic_on_update.Visible = true; pic_on_update.Enabled = true;
            }
            if (detect.Contains("3"))
            {
                status_cheat.Text = "</> beta"; status_cheat.ForeColor = System.Drawing.Color.CornflowerBlue;
                pic_beta.Visible = true; pic_on_update.Enabled = true; start_load.Enabled = true;
            }
        }
        private async void penis()
        {
            your_id.Text = api.KeyAuthApp.user_data.username;
            label_version.Text = "Program version: " + api.KeyAuthApp.var("version");
            name_sub.Text = "Subscription: " + api.KeyAuthApp.user_data.subscriptions[0].subscription;
            num_days.Text = UnixTimeToDateTime().ToString();
        }
        public string UnixTimeToDateTime()
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local); try{dtDateTime = dtDateTime.AddSeconds(long.Parse(api.KeyAuthApp.user_data.subscriptions[0].expiry)).ToLocalTime();}catch{dtDateTime = DateTime.MaxValue;}return dtDateTime.ToString("dd.MM.yyyy");
        }
        private async void data_upd()
        {
            api.KeyAuthApp.var("data_upd"); data_update.Text = api.KeyAuthApp.response.message;
        }
        private async void gif()
        {
            data_upd(); check_detect(); penis();
            panel1.Visible = false; panel1.Enabled = false;
            panel2.Visible = false; panel2.Enabled = false;

            main_gif.Enabled = false; main_gif.Visible = false;
            panel2.Visible = true; panel2.Enabled = true;
            choose_work.Enabled = true; choose_work.Visible = true;
            settings.Visible = true; settings.Enabled = true; settings1.Visible = false; settings1.Enabled = false;
        }
        private void start_load_Click(object sender, EventArgs e)
        {
            int selectedIndex = choose_work.SelectedIndex;
            if (!ert){ start_load.Text = "Stop"; ert = true; choose_work.Enabled = false; } else{ start_load.Text = "Start"; ert = false; choose_work.Enabled = true; start_load.ImageOffsetX = 147; }
            if (ert) {
                if (selectedIndex == -1) { start_load.Text = "error | make the assistant choice higher"; start_load.ImageOffsetX = 999999; }
                if (selectedIndex == 0) { Fishing(); }
                if (selectedIndex == 1) { Mushroom(); }
                if (selectedIndex == 2) { Captcha(); }
                if (selectedIndex == 3) { Farm(); }
                if (selectedIndex == 4) { Sawmill();  }
                if (selectedIndex == 5) { Career(); }
            }
        }
        private void Fishing() { 
            start_load.Text = "Launched | Fishing";
            start_load.ImageOffsetX = 999999;
        }
        private void Mushroom() {
            start_load.Text = "Launched | Mushroom";
            start_load.ImageOffsetX = 999999;
        }
        private void Captcha() {
            start_load.Text = "Launched | Captcha";
            start_load.ImageOffsetX = 999999;
        }
        private void Farm() {
            start_load.Text = "Launched | Farm";
            start_load.ImageOffsetX = 999999;
        }
        private void Sawmill()
        {
            start_load.Text = "Launched | Sawmill";
            start_load.ImageOffsetX = 999999;
            Thread thread = new Thread(Sawmill1);
            thread.Start();
        }
        private void Sawmill1()
        {
            int targetX = 123; int targetX1 = 1864;
            int targetY = 48; int targetY1 = 1003;
            Color targetColor1 = Color.FromArgb(248, 248, 248); // Первый целевой цвет
            Color targetColor2 = Color.FromArgb(79, 184, 255); // Второй целевой цвет

            // Шаг 1: Поиск первого цвета
            while (true)
            {
                close_app.Text = "1";
                Color pixelColor = GetPixelColor(targetX, targetY);

                if (pixelColor.R == targetColor1.R && pixelColor.G == targetColor1.G && pixelColor.B == targetColor1.B)
                {
                    // Нажимаем клавишу 'E' (код 0x45)
                    PressKey(VK_E);
                    break; // Переходим ко второму шагу
                }

                Thread.Sleep(100); // Задержка для уменьшения нагрузки на процессор
            }

            // Шаг 2: Поиск второго цвета
            while (true)
            {
                close_app.Text = "2";
                Color pixelColor = GetPixelColor(targetX1, targetY1);

                if (pixelColor.R == targetColor2.R && pixelColor.G == targetColor2.G && pixelColor.B == targetColor2.B)
                {
                    close_app.Text = "3";
                    // Нажимаем ЛКМ и завершаем скрипт
                    //ClickLeftMouseButton();
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN,System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.Y,0,0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP,System.Windows.Forms.Cursor.Position.X,System.Windows.Forms.Cursor.Position.Y,0,0);
                    break;
                }
                else
                {
                    // Если цвет не найден, нажимаем ЛКМ каждую секунду
                    //ClickLeftMouseButton();
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y, 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y, 0, 0);
                    Thread.Sleep(500); // Задержка 1 секунда
                }
            }
            close_app.Text = "4";
        }
        public enum MouseEvent
        {
            MOUSEEVENTF_LEFTDOWN = 0x02,
            MOUSEEVENTF_LEFTUP = 0x04,
        }
        private void Career()
        {
            start_load.Text = "Launched | Career";
            start_load.ImageOffsetX = 999999;
        }

        private Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero); // Получаем контекст устройства
            uint pixel = GetPixel(hdc, x, y); // Получаем цвет пикселя
            ReleaseDC(IntPtr.Zero, hdc); // Освобождаем контекст устройства

            // Преобразуем значение пикселя в Color
            return Color.FromArgb(
                (int)(pixel & 0x000000FF),         // Красный
                (int)(pixel & 0x0000FF00) >> 8,    // Зеленый
                (int)(pixel & 0x00FF0000) >> 16   // Синий
            );
        }

        private void PressKey(byte keyCode)
        {
            // Нажатие клавиши
            keybd_event(keyCode, 0, KEYEVENTF_KEYDOWN, UIntPtr.Zero);
            keybd_event(keyCode, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
/*        private void ClickLeftMouseButton()
        {
            // Выполняем нажатие левой кнопки мыши
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            // Отпускаем левую кнопку мыши
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }*/
    }
}
