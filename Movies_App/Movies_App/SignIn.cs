using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Movies_App
{
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            SignUp signUp = new SignUp();
            signUp.Show();
            this.Hide();
        }
    }
}
