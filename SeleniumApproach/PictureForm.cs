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
        private Form1 form;
        private IWebElement ele;

        public PictureForm(IWebElement ele, Form1 form)
        {
            InitializeComponent();
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

            this.form = form;
            this.ele = ele;
            Program.EyeXHost.Connect(behaviorMap1);
            behaviorMap1.Add(button1, new ActivatableBehavior(button1_Click));
            behaviorMap1.Add(button2, new ActivatableBehavior(button2_Click));
            behaviorMap1.Add(button3, new ActivatableBehavior(button3_Click));

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("test3");

            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("test3");
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("test3");
            this.Hide();
        }

    }
}
