using CloudHelper.Properties;
using KeyAuth;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Cloud_loader_release
{
    public partial class Login : Form
    {
        [DllImport("user32.dll")] private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")] private static extern uint GetPixel(IntPtr hdc, int x, int y);
        [DllImport("user32.dll")] private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);
        [DllImport("user32.dll")] private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)] public static extern void mouse_event(MouseEvent dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int KEYEVENTF_KEYDOWN = 0x0000; private const int KEYEVENTF_KEYUP = 0x0002;
        private const byte VK_E = 0x45; // Код клавиши 'E'

        private bool ert = false;
        private int num = 1;
        private Bitmap Image;

        public Login()
        {
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
            this.Hide();
            main_pic.Image = null;
            main_pic.Image = null;
            settings.Image = null;
            settings1.Image = null;
            open_pass.Image = null;
            close_pass.Image = null;
            open_key.Image = null;
            close_key.Image = null;
            pic_id.Image = null;
            pic_undetect.Image = null;
            pic_detect.Image = null;
            pic_on_update.Image = null;
            pic_beta.Image = null;
            pic_upd.Image = null;
            pic_sub.Image = null;
            start_load.Image = null;
            Button_list_loader.Image = null;
            disc.Image = null;
            github.Image = null;
            Environment.Exit(0);
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
            api.KeyAuthApp.register(user_name.Text, user_pass.Text, user_key.Text = "HUI_" + user_key.Text);
            status_num(api.KeyAuthApp.response.message);
        }
        private void Button_upgrade_Click(object sender, EventArgs e)
        {
            api.KeyAuthApp.init();
            api.KeyAuthApp.upgrade(user_name.Text, user_key.Text = "HUI_" + user_key.Text);
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
            panel2.Enabled = false; panel2.Visible = false;


            disc.Image = Image = Resources.concours_discord_cartes_voeux_fortnite_france_6;
            github.Image = Image = Resources.telegram;
            Button_list_loader.Image = Image = Resources.list;


            pic_id.Image = null; pic_sub.Image = null; pic_upd.Image = null;

            panel_settings.Visible = true; panel_settings.Enabled = true; settings1.Enabled = true; settings1.Visible = true; settings.Enabled = false; settings.Visible = false;
        }
        private void settings1_Click(object sender, EventArgs e)
        {
            choose_work.Enabled = true; choose_work.Visible = true;


            disc.Image = null; github.Image = null; Button_list_loader.Image = null;


            pic_id.Image = Image = Resources.icon_user;
            pic_sub.Image = Image = Resources.list;
            pic_upd.Image = Image = Resources.calendar;

            panel2.Enabled = true; panel2.Visible = true;
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
                pic_detect.Image = null; pic_on_update.Image = null; pic_beta.Image = null;
            }
            if (detect.Contains("1"))
            {
                status_cheat.Text = "DETECT"; status_cheat.ForeColor = System.Drawing.Color.Red;
                pic_detect.Visible = true; pic_detect.Enabled = true;
                pic_undetect.Image = null; pic_on_update.Image = null; pic_beta.Image = null;
            }
            if (detect.Contains("2"))
            {
                status_cheat.Text = "on update"; status_cheat.ForeColor = System.Drawing.Color.Blue;
                pic_on_update.Visible = true; pic_on_update.Enabled = true;
                pic_undetect.Image = null; pic_detect.Image = null; pic_beta.Image = null;
            }
            if (detect.Contains("3"))
            {
                status_cheat.Text = "</> beta"; status_cheat.ForeColor = System.Drawing.Color.CornflowerBlue;
                pic_beta.Visible = true; pic_on_update.Enabled = true; start_load.Enabled = true;
                pic_undetect.Image = null; pic_detect.Image = null; pic_on_update.Image = null;
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
            close_pass.Image = null;open_pass.Image = null;close_key.Image = null;open_key.Image = null;
            main_pic.Image = null;

            panel1.Visible = false; panel1.Enabled = false;
            panel2.Visible = false; panel2.Enabled = false;
            main_pic.Visible = false; main_pic.Enabled = false;

            Timer timer = new Timer();
            timer.Interval = 1500;
            timer.Tick += Timer_Tick;
            main_gif.Enabled = true; main_gif.Visible = true; main_gif.Size = new System.Drawing.Size(224, 182); main_gif.Location = new System.Drawing.Point(109, 80);
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            data_upd(); check_detect(); penis();
            main_gif.Image = null; // Очищаем PictureBox
            ((Timer)sender).Stop();
            main_pic.Image = Image = Resources.main_logo;
            main_gif.Enabled = false;
            main_gif.Visible = false;

            main_pic.Visible = true;
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
                if (selectedIndex == 6) { Taxi(); }
            }
        }
        private void Fishing() { 
            start_load.Text = "Launched | Fishing";
            start_load.ImageOffsetX = 999999;
        }
        private void Mushroom() {
            start_load.Text = "Launched | Mushroom";
            start_load.ImageOffsetX = 999999;
            Thread thread = new Thread(Mushroom1);
            thread.Start();
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
        private void Mushroom1()
        {
            if (!ert) return;
            int targetX = 123; int targetX1 = 785; int targetX2 = 781;
            int targetY = 48; int targetY1 = 1029; int targetY2 = 1029;
            Color targetColor1 = Color.FromArgb(248, 248, 248); // Первый целевой цвет
            Color targetColor2 = Color.FromArgb(50, 190, 131); // Второй целевой цвет
            Color targetColor3 = Color.FromArgb(224, 79, 95); // 3 целевой цвет

            // Шаг 1: Поиск первого цвета
            while (true)
            {
                if (!ert) break;
                Color pixelColor = GetPixelColor(targetX, targetY);

                if (pixelColor.R == targetColor1.R && pixelColor.G == targetColor1.G && pixelColor.B == targetColor1.B)
                {
                    // Нажимаем клавишу 'E' (код 0x45)
                    PressKey(VK_E);
                    break; // Переходим ко второму шагу
                }

                Thread.Sleep(100); // Задержка для уменьшения нагрузки на процессор
            }
            Thread.Sleep(8000);
            // Шаг 2: Поиск второго цвета
            while (true)
            {
                if (!ert) break;
                Color pixelColor = GetPixelColor(targetX1, targetY1);
                Color pixelColor2 = GetPixelColor(targetX2, targetY2);

                if (pixelColor.R == targetColor2.R && pixelColor.G == targetColor2.G && pixelColor.B == targetColor2.B)
                {
                    Mushroom1();
                    break;
                }

                if (pixelColor.R == targetColor2.R && pixelColor.G == targetColor2.G && pixelColor.B == targetColor2.B)
                {
                    Mushroom1();
                    break;
                }
                else
                {
                    Thread.Sleep(2000);
                    Mushroom1();
                    break;
                }
            }
        }
        private void Sawmill1()
        {
            if (!ert) return;
            int targetX = 123; int targetX1 = 1864; int targetX2 = 784;
            int targetY = 48; int targetY1 = 1003; int targetY2 = 1027;
            Color targetColor1 = Color.FromArgb(248, 248, 248); // Первый целевой цвет
            Color targetColor2 = Color.FromArgb(79, 184, 255); // Второй целевой цвет
            Color targetColor3 = Color.FromArgb(50, 190, 131); // Третий целевой цвет

            // Шаг 1: Поиск первого цвета
            while (true)
            {
                if (!ert) return;
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
                if (!ert) return;
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


            // Шаг 3: Поиск 3 цвета
            while (true)
            {
                if (!ert) return;
                close_app.Text = "2";
                Color pixelColor = GetPixelColor(targetX2, targetY2);

                if (pixelColor.R == targetColor3.R && pixelColor.G == targetColor3.G && pixelColor.B == targetColor3.B)
                {
                    break;
                }
                else
                {
                    close_app.Text = "3";
                    Thread.Sleep(2200);
                    // Если цвет не найден, нажимаем ЛКМ каждую секунду

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(736 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(419 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(736 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(419 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(808 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(402 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(808 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(402 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(858 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(381 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(858 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(381 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(920 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(364 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(920 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(364 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(952 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(359 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(952 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(359 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(973 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(368 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(973 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(368 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(986 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(366 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(986 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(366 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1016 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(352 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1016 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(352 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1025 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(346 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1025 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(346 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1030 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(346 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1030 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(346 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1098 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(365 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1098 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(365 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1010 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(398 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1010 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(398 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(993 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(404 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(993 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(404 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(976 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(413 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(976 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(413 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(912 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(442 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(912 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(442 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(906 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(448 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(906 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(448 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(796 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(526 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(796 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(526 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(893 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(487 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(893 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(487 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(964 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(477 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(964 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(477 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(990 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(473 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(990 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(473 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1030 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(457 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1030 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(457 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1053 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(450 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1053 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(450 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1061 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(442 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1061 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(442 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1073 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(442 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1073 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(442 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1116 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(427 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1116 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(427 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1130 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(415 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1130 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(415 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);

                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTDOWN, (uint)(1070 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(416 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    mouse_event(MouseEvent.MOUSEEVENTF_LEFTUP, (uint)(1070 * 65535 / Screen.PrimaryScreen.Bounds.Width), (uint)(416 * 65535 / Screen.PrimaryScreen.Bounds.Height), 0, 0);
                    Thread.Sleep(500);
                    break;
                }
            }



            close_app.Text = "4";
        }
        public enum MouseEvent{MOUSEEVENTF_LEFTDOWN = 0x02,MOUSEEVENTF_LEFTUP = 0x04}
        private void Career()
        {
            start_load.Text = "Launched | Career";
            start_load.ImageOffsetX = 999999;
        }
        private void Taxi()
        {
            start_load.Text = "Launched | Taxi";
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

        private void github_Click(object sender, EventArgs e)
        {
            string url = "https://t.me/cloud_helper";
            Process.Start(url);
        }

        private void disc_Click(object sender, EventArgs e)
        {
            string url = "https://discord.gg/uMw7bpYGuw";
            Process.Start(url);
        }
    }
}
