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
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void label8_Click(object sender, EventArgs e)
        {
            SignIn signIn = new SignIn();
            signIn.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Movies movies = new Movies();
            movies.Show();
            this.Hide();
        }
    }
}
