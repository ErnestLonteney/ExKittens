using System;
using System.Windows.Forms;

namespace ExploadingKittens
{
    public partial class BackCardForm : Form
    {
        public BackPlace response { get; set; } 
        public BackCardForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            response = BackPlace.Top;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            response = BackPlace.Second;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            response = BackPlace.Thirth;
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            response = BackPlace.Fourth;
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            response = BackPlace.Bottom;
            Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            response = BackPlace.Random;
            Close();
        }
    }
}
