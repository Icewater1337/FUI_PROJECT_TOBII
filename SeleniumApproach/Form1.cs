﻿using SeleniumApproach;
using EyeXFramework.Forms;
using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tobii.Interaction;

namespace SeleniumApproach
{
    public partial class Form1 : Form
    {
        IWebDriver driver;
        Actions builder;
        Host host;
        Boolean isExecutingAction = false;

        public Form1()
        {
            InitializeComponent();
           
            this.driver = new ChromeDriver();
            this.builder = new Actions(driver);
            this.host = new Host();

            driver.Url = "https://www.google.ch/search?q=tree&dcr=0&source=lnms&tbm=isch&sa=X&ved=0ahUKEwiew6yFm6PaAhWFkywKHdgCBvYQ_AUICigB&biw=1536&bih=720";
            list = GetImageHrefsOnScreen();
            Thread.Sleep(2000);

            
            new Thread(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                double ellapsedSecs = 0;
                Boolean alreadyClicked = false;
                IWebElement previous = null;

                Thread.CurrentThread.IsBackground = true;
                while (true)
                {


                    IWebElement ele = GetElementLookingAtRectangle();
                    stopwatch.Start();
                    ellapsedSecs = stopwatch.ElapsedMilliseconds / 1000;

                    if (ele != null && previous != null && !previous.Equals(ele) && !isExecutingAction)
                    {
                        alreadyClicked = true;
                        HoverOverElement(ele);
                    } 

                    previous = ele;

                }
            }).Start();
            
            while (true)
            {
                if (IsKeyPushedDown(System.Windows.Forms.Keys.LControlKey))
                {
                    executeAction();
                }

            }

        }

        [DllImport("user32.dll")]
        static extern ushort GetAsyncKeyState(int vKey);

        public static bool IsKeyPushedDown(System.Windows.Forms.Keys vKey)
        {
            return 0 != (GetAsyncKeyState((int)vKey) & 0x8000);
        }


        public void executeAction()
        {
            this.Hide();
            //IWebElement ele = GetElementLookingAt();
            IWebElement ele = GetElementLookingAtRectangle();
            isExecutingAction = true;
            ShowButtonsForm(ele);
            isExecutingAction = false;

          
        }

        public bool IsExecutingAction { get => isExecutingAction; set => isExecutingAction = value; }

        private void ShowButtonsForm(IWebElement ele)
        {
            // PictureForm picForm = new PictureForm();
            //picForm.ShowDialog();
            // picForm = null;

            ActivatableButtonsForm form = new ActivatableButtonsForm(this, ele);
            Form backGorm = ShowBackgroundForm();
            backGorm.StartPosition = FormStartPosition.CenterScreen;
            Form picForm = CreateImageShowSuccessForm(ele);
            backGorm.Location = this.Location;
            backGorm.Opacity = 0.75;
            picForm.Opacity = 0.75;
            backGorm.Show();

            picForm.Show();
            form.ShowDialog();

            picForm.Dispose();
            backGorm.Dispose();
        }


        private void HoverOverElement(IWebElement ele)
        {
            if (ele != null)
            {
               // Thread.Sleep(100);
                builder.MoveToElement(ele).Perform();

            }
        }

        private IWebElement GetParentElement ( IWebElement elt)
        {
            if (driver is IJavaScriptExecutor)
            {
                return (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
                                   "return arguments[0].parentNode;", elt);
            } else
            {
                throw new InvalidOperationException("Javascript executor should not be null");
            }
        }

        internal void ShareImage(IWebElement ele)
        {
            ele.Click();
            IWebElement parent = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
            "return arguments[0].parentNode;", ele);

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> children = parent.FindElements(By.TagName("div"));

            IWebElement srcElt = children[2];

            String html1 = srcElt.GetAttribute("innerHTML"); //(?=ou\":\")(.*)(?=\\",\\"ow)

            String propReg = @"http.+?(?=\"",)";

            var match = Regex.Match(html1, propReg).Value.Replace(@"""","");

            var imgSrc = @"//img [@src="""+match +@"""]";

            IWebElement elts = null;


            while (elts == null)
            {
                try
                {
                    elts = driver.FindElement(By.XPath(imgSrc));
                }
                catch (Exception)
                {
                    elts = driver.FindElement(By.XPath(imgSrc));
                }
            }

            

            IWebElement el = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
                      "return arguments[0].parentNode;", elts);

            el = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
                     "return arguments[0].parentNode;", el);

            el = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
                   "return arguments[0].parentNode;", el);

            el = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
                   "return arguments[0].parentNode;", el);

            el = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
                   "return arguments[0].parentNode;", el);

            IReadOnlyCollection<IWebElement> tagZ = (IReadOnlyCollection<IWebElement>)el.FindElements(By.TagName("a"));
            IWebElement shareLink = tagZ.FirstOrDefault(x => ((IWebElement)x).Text == "Share");
            shareLink.Click();



        }

        private Form ShowBackgroundForm()
        {
            Form form = new Form();
            form.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            form.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            form.AutoSize = true;
            form.BackColor = System.Drawing.SystemColors.Menu;
            form.ClientSize = new System.Drawing.Size(2700, 700);
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Name = "ActivatableButtonsForm";
            form.Text = "Form1";
            form.TopMost = true;
            form.ResumeLayout(false);
            System.IntPtr ptr = CreateRoundRectRgn(0, 0, form.Width, form.Height, 100, 100); // _BoarderRaduis can be adjusted to your needs, try 15 to start.
            form.Region = System.Drawing.Region.FromHrgn(ptr);
            return form;
        }

        private Form CreateImageShowSuccessForm(IWebElement elt)
        {
            PictureBoxForm form = new PictureBoxForm();
            form.Opacity = 0.75;
            System.IntPtr ptr = CreateRoundRectRgn(0, 0, form.Width, form.Height, 100, 100); // _BoarderRaduis can be adjusted to your needs, try 15 to start.
            form.Region = System.Drawing.Region.FromHrgn(ptr);
            IWebElement parent = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
           "return arguments[0].parentNode;", elt);

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> children = parent.FindElements(By.TagName("div"));

            IWebElement srcElt = children[2];

            String html1 = srcElt.GetAttribute("innerHTML"); //(?=ou\":\")(.*)(?=\\",\\"ow)

            String propReg = @"http.+?(?=\"",)";

            var match = Regex.Match(html1, propReg).Value;

            String fileEnding = match.Substring(match.Length - 4);

            var wClient = new WebClient();
            System.IO.Directory.CreateDirectory("C:/tmp1");
            wClient.DownloadFile(match, "C:/tmp1/tmp" + fileEnding);

            Bitmap source = new Bitmap(@"C:/tmp1/tmp" + fileEnding);

          
            form.pictureBox1.Image = (Image)source;
            form.StartPosition = FormStartPosition.Manual;
            form.FormBorderStyle = FormBorderStyle.None;
           
            form.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - form.Width) / 2, 50);


            //form.BackgroundImage = source;
            // form.CreateGraphics().DrawImage(source, 10, 10);
            return form;

        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
       (
           int nLeftRect,     // x-coordinate of upper-left corner
           int nTopRect,      // y-coordinate of upper-left corner
           int nRightRect,    // x-coordinate of lower-right corner
           int nBottomRect,   // y-coordinate of lower-right corner
           int nWidthEllipse, // height of ellipse
           int nHeightEllipse // width of ellipse
       );

      


        private IWebElement GetElementByPoint(IWebDriver driver, int locationX, int locationY)
        {
            if (driver is IJavaScriptExecutor)
            {
                return (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
            "return document.elementFromPoint(arguments[0], arguments[1])",
            locationX, locationY);
            } else
            {
                throw new InvalidOperationException("Javascript executor should not be null");
            }

            
        }

        private IWebElement GetElementLookingAt()
        {

            var gazePointDataStream = this.host.Streams.CreateGazePointDataStream();
            float scale = GetDPIScale();
            int gazePointX = 0;
            int gazePointY = 0;

            //Thread.Sleep(1000);
         
           gazePointDataStream.Next += (a, gaze) =>
            
            {
                gazePointX = Convert.ToInt32(Math.Floor(gaze.Data.X / scale));
                gazePointY = Convert.ToInt32(((Math.Floor(gaze.Data.Y / scale))/10)*9);

              
                gazePointDataStream.IsEnabled = false;
            };


            while ( gazePointDataStream.IsEnabled)
            {

            }


            // var  locationX = Cursor.Position.X;
            // var locationY = Cursor.Position.Y;
            IWebElement ele = GetElementByPoint(driver, gazePointX, gazePointY);

            if ( ele != null)
            {
                // IF the proper elt is not found, vary a bit and try to find it.
                int variable = 0;
                Boolean found = false;
                while (!(ele.TagName.Equals("a") || ele.TagName.Equals("img")) && variable < 20)
                {
                    var tmpGazeX = gazePointX;
                    var tmpGazeY = gazePointY;
                    int[] dirArr = { 1, -1 };
                    // move to certain directions X
                    for (int i = 0; i < dirArr.Length; i++)
                    {
                        if (found == false)
                        {
                            ele = GetElementByPoint(driver, (tmpGazeX + dirArr[i] * variable), tmpGazeY);
                        }
                        if (ele != null && ((ele.TagName.Equals("a") && ele.Text.Equals("")) || ele.TagName.Equals("img")))
                        {
                            found = true;
                            break;
                        }

                    }
                    // move to Y
                    for (int i = 0; i < dirArr.Length; i++)
                    {
                        if (found == false)
                        {
                            ele = GetElementByPoint(driver, (tmpGazeX), (tmpGazeY + dirArr[i] * variable));
                        }
                        if (ele != null && ((ele.TagName.Equals("a") && ele.Text.Equals("")) || ele.TagName.Equals("img")))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == true)
                    {
                        break;
                    }

                    variable++;
                }

                if (ele.TagName.Equals("img"))
                {
                    ele = GetParentElement(ele);
                }

                if (ele != null && ele.TagName.Equals("a") && ele.Text.Equals(""))
                {
                    return ele;
                }

            }
            return ele;
        }
        private List<IWebElement> list;

        private List<IWebElement> GetImageHrefsOnScreen()
        {
            IReadOnlyCollection<IWebElement> elts = driver.FindElements(By.TagName("img"));
            List<IWebElement> myList = new List<IWebElement>(elts);
            List<IWebElement> eltsWithClass = myList.Where<IWebElement>(v => v.GetAttribute("class").Equals("rg_ic rg_i")).ToList<IWebElement>();
            List<IWebElement> retList = new List<IWebElement>();

            foreach (IWebElement el in eltsWithClass)
            {
                retList.Add(GetParentElement(el));
            }

            retList = retList.GetRange(0, 10);
            return retList;

        }

        private double GetDistanceToElement(IWebElement ele, Point p)
        {
            var loc = ele.Location.X;
            var width = ele.Size.Width;
            int[] first = { loc - p.X, 0, p.X - (loc + width) };
            int dx = first.Max();
            var loc2 = ele.Location.Y;
            var height = ele.Size.Height;
            int[] second = { loc2 - p.Y, 0, p.Y - (loc2 + height) };
            int dy = second.Max();
            double d = dx * dx + dy * dy;
            return Math.Sqrt(d);
        }

        private IWebElement GetElementLookingAtRectangle()
        {

            var gazePointDataStream = this.host.Streams.CreateGazePointDataStream();
            float scale = GetDPIScale();
            int gazePointX = 0;
            int gazePointY = 0;

            gazePointDataStream.Next += (a, gaze) =>

            {
                gazePointX = Convert.ToInt32(Math.Floor(gaze.Data.X / scale));
                gazePointY = Convert.ToInt32(((Math.Floor(gaze.Data.Y / scale)) / 10) * 9);


                gazePointDataStream.IsEnabled = false;
            };


            while (gazePointDataStream.IsEnabled)
            {

            }

            // var gazePointX = 240;
            // var gazePointY = 230;
            IWebElement closest = list.OrderBy(e => GetDistanceToElement(e, new Point(gazePointX, gazePointY))).First();

            return closest;
        }
        public void CopyImageToClipboard(IWebElement elt)
        {
            IWebElement parent = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
                       "return arguments[0].parentNode;", elt);

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> children = parent.FindElements(By.TagName("div"));

            IWebElement srcElt = children[2];

            String html1 = srcElt.GetAttribute("innerHTML"); //(?=ou\":\")(.*)(?=\\",\\"ow)

            String propReg = @"http.+?(?=\"",)";

            var match = Regex.Match(html1, propReg).Value;

            String fileEnding = match.Substring(match.Length - 4);

            var wClient = new WebClient();
            System.IO.Directory.CreateDirectory("C:/tmp");
            wClient.DownloadFile(match, "C:/tmp/tmp"+ fileEnding);

            Bitmap source = new Bitmap(@"C:/tmp/tmp"+ fileEnding);

            Clipboard.SetImage(source);

        }

        public void DownloadImage(IWebElement ele)
        {
            IWebElement parent = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
                                   "return arguments[0].parentNode;", ele);

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> children = parent.FindElements(By.TagName("div"));

            IWebElement srcElt = children[2];

            String html1 = srcElt.GetAttribute("innerHTML"); //(?=ou\":\")(.*)(?=\\",\\"ow)

            String propReg = @"http.+?(?=\"",)";

            var match = Regex.Match(html1, propReg).Value;

            var dialog = new SaveFileDialog();

            var result = dialog.ShowDialog(); //shows save file dialog
            if (result == DialogResult.OK)
            {
                Console.WriteLine("writing to: " + dialog.FileName); //prints the file to save

                var wClient = new WebClient();
                wClient.DownloadFile(match, dialog.FileName);
            }

        }

        private float GetDPIScale()
        {
            var currentDpi = (int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics", "AppliedDPI", 96);
           return (float)currentDpi / 96;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
