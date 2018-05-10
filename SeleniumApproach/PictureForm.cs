using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EyeXFramework.Forms;
using EyeXFramework;
using OpenQA.Selenium;

namespace SeleniumApproach
{
    public partial class PictureForm : Form
    {
        public PictureForm()
        {
            InitializeComponent();
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

            Program.EyeXHost.Connect(behaviorMap1);
            ActivatableBehavior beh1 = new ActivatableBehavior(button1_Click);
            behaviorMap1.Add(button1, beh1);
            behaviorMap1.Add(button2, new ActivatableBehavior(button2_Click));
            behaviorMap1.Add(button3, new ActivatableBehavior(button3_Click));

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("test1");
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("test2");
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("test3");
            this.Dispose();
        }

    }
}
