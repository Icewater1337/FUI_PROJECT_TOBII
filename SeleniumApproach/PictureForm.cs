using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeleniumApproach
{
    public partial class PictureForm : Form
    {
        public PictureForm()
        {
            InitializeComponent();
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This the shit2");
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("This the shit2");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_2(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click_3(object sender, EventArgs e)
        {
            MessageBox.Show("test");
            Application.Exit();
        }
    }
}
