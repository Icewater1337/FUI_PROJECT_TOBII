//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

namespace SeleniumApproach
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using EyeXFramework;
    using OpenQA.Selenium;

    public partial class ActivatableButtonsForm : Form
    {
        const float HueStep = 0.075f;
        const float BrightnessStep = 0.1f;
        private IWebElement ele;
        private Form1 form1;

        public ActivatableButtonsForm(Form1 form1, OpenQA.Selenium.IWebElement ele)
        {
            this.ele = ele;
            this.form1 = form1;
            KeyPreview = true;
            InitializeComponent();
            this.BackColor = Color.LimeGreen;
            this.TransparencyKey = Color.LimeGreen;

            // Make the buttons on the form direct clickable using the Activatable behavior.
            Program.EyeXHost.Connect(behaviorMap1);
            behaviorMap1.Add(button1, new ActivatableBehavior(OnButtonActivated));
            behaviorMap1.Add(button2, new ActivatableBehavior(OnButtonActivated));
            behaviorMap1.Add(button3, new ActivatableBehavior(OnButtonActivated));

            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            Console.WriteLine("OnKeyUp: " + keyEventArgs.KeyCode);

            if (keyEventArgs.KeyCode == System.Windows.Forms.Keys.ShiftKey)
            {
                Console.WriteLine("TriggerActivation");
                Program.EyeXHost.TriggerActivation();
            }
            keyEventArgs.Handled = false;
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            // See PannableForms sample for an example how to disregard repeated KeyDown events.
            // We don't bother to do it in this example since most users do not press and hold down
            // the key for long, when clicking.
            Console.WriteLine("OnKeyDown: " + keyEventArgs.KeyCode);
            if (keyEventArgs.KeyCode == System.Windows.Forms.Keys.ShiftKey)
            {
                Console.WriteLine("TriggerActivationModeOn");
                Program.EyeXHost.TriggerActivationModeOn();
            }
            keyEventArgs.Handled = false;
        }

        /// <summary>
        /// Event handler invoked when a button is activated.
        /// </summary>
        /// <param name="sender">The control that received the gaze click.</param>
        private void OnButtonActivated(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                Console.WriteLine("OnButtonActivated");
                button.PerformClick();
            }
        }

        private void copyImageToClipboard(object sender, EventArgs e)
        {
            form1.CopyImageToClipboard(ele);

        }

        private void downloadImage(object sender, EventArgs e)
        {
            form1.DownloadImage(ele);
        }

        private void shareImage(object sender, EventArgs e)
        {
            form1.ShareImage(ele);
        }

     
    }
}
