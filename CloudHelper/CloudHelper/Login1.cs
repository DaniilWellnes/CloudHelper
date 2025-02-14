using KeyAuth;
using System;
using System.Windows.Forms;

namespace Cloud_loader_release
{
    public partial class Login : Form
    {
        
        public Login()
        {
            //KeyAuthApp.init();
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
            user_key.UseSystemPasswordChar = true;
            user_pass.UseSystemPasswordChar = true;
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void close_app_Click(object sender, EventArgs e)
        {

            Application.Exit();
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
        private void Button_login_Click(object sender, EventArgs e)
        {
            api.KeyAuthApp.init();
            api.KeyAuthApp.login(user_name.Text, user_pass.Text);
            status.Text = api.KeyAuthApp.response.message;
        }
        private void Button_register_Click(object sender, EventArgs e)
        {
            api.KeyAuthApp.init();
            api.KeyAuthApp.register(user_name.Text, user_pass.Text, user_key.Text);
            status.Text = "Status: " + api.KeyAuthApp.response.message;

/*            KeyAuthApp.register(user_name.Text, " ", user_key.Text);
            if (KeyAuthApp.response.success)
            {
                KeyAuthApp.login(user_name.Text, " ", user_key.Text);
                if (KeyAuthApp.response.success)
                {
                    zatupok.reg_bin(user_name.Text, user_key.Text);

                    menu menu = new menu();
                    menu.Show();
                    this.Hide();
                }
                else
                {
                    status.Visible = true;
                    status.Text = "Status: " + KeyAuthApp.response.message;
                }
            }
            else
            {
                KeyAuthApp.login(user_name.Text, " ", user_key.Text);
                if (KeyAuthApp.response.success)
                {
                    keys.reg_bin(user_name.Text, user_key.Text);
                    menu menu = new menu();
                    menu.Show();
                    this.Hide();
                }
                else
                {
                    status.Visible = true;
                    status.Text = "Status: " + KeyAuthApp.response.message;
                }
            }*/
        }

        private void Button_switch_Click(object sender, EventArgs e) //register
        {
            Button_register.Enabled = true;
            Button_register.Visible = true;
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


    }
}
