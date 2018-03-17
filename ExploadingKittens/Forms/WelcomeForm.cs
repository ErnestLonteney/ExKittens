using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExploadingKittens
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label2.Visible = true;
            textBox2.Visible = true; 
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = true;
            textBox3.Visible = true;
            button1.Enabled = true; 
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label4.Visible = true;
            textBox4.Visible = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label5.Visible = true;
            textBox5.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> names = new List<string>();
            if (textBox1.Text != String.Empty) names.Add(textBox1.Text);
            if (textBox2.Text != String.Empty) names.Add(textBox2.Text);
            if (textBox3.Text != String.Empty) names.Add(textBox3.Text);
            if (textBox4.Text != String.Empty) names.Add(textBox4.Text);
            if (textBox5.Text != String.Empty) names.Add(textBox5.Text);
            Game g = new Game();
            DeckType dt = radioButton1.Checked ? DeckType.Standart : radioButton2.Checked ? DeckType.Impladings : DeckType.Party;
            if (dt == DeckType.Party)
            {
                MessageBox.Show("Цей варіант гри ще не реалізовано!");
                return; 
            }
            g.CreateGame(names.ToArray(), dt, this);
            g.Text = radioButton1.Checked ? radioButton1.Text : radioButton2.Checked ? radioButton2.Text : radioButton1.Text;
            g.Text = "Вибухові кошенята-" + g.Text;
            g.Show();
            this.Visible = false; 
        }
    }
}
