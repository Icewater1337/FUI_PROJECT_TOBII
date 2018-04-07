﻿using Microsoft.Win32;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tobii.Interaction;

namespace SeleniumApproach
{
    public partial class Form1 : Form
    {
        Boolean handled = false;
        IWebDriver driver;
        Actions builder;
        Host host;

        public Form1()
        {
            InitializeComponent();
           
            this.driver = new ChromeDriver();
            this.builder = new Actions(driver);
            this.host = new Host();

            driver.Url = "https://www.google.ch/search?q=tree&dcr=0&source=lnms&tbm=isch&sa=X&ved=0ahUKEwiew6yFm6PaAhWFkywKHdgCBvYQ_AUICigB&biw=1536&bih=720";

            
           
          

          //  By locator = By.XPath("//*[@id=\"rg_s\"]/div[1]/a/div[2]");
          //IWebElement hoverElement = driver.FindElement(By.XPath("//*[@id=\"rg_s\"]/div[1]/a"));
         


            //Thread.Sleep(10000);

            //Console.WriteLine(ele);
        }

        private void Wb_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (!handled && e.KeyCode == System.Windows.Forms.Keys.F1)
            {
             

                //  locationX = Cursor.Position.X;
                // locationY = Cursor.Position.Y;

               


                StartGazePointDataStream();


                handled = true;

            }
        }

        private void HoverOverElement(IWebElement ele)
        {
            if (ele != null)
            {
                Thread.Sleep(5000);
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

        private void StartGazePointDataStream()
        {

            var gazePointDataStream = this.host.Streams.CreateGazePointDataStream();
            float scale = GetDPIScale();
            int gazePointX = 0;
            int gazePointY = 0;

            Thread.Sleep(5000);
         
           gazePointDataStream.Next += (a, gaze) =>
            
            {
                gazePointX = Convert.ToInt32(Math.Floor(gaze.Data.X / scale));
                gazePointY = Convert.ToInt32(Math.Floor(gaze.Data.Y / scale));

              
                gazePointDataStream.IsEnabled = false;
            };


            while ( gazePointDataStream.IsEnabled)
            {

            }

            // var  locationX = Cursor.Position.X;
            // var locationY = Cursor.Position.Y;
            IWebElement ele = GetElementByPoint(driver, gazePointX, gazePointY);

            // IF the proper elt is not found, vary a bit and try to find it.
            int variable = 0;
            Boolean found = false;
            while ( !(ele.TagName.Equals("a") || ele.TagName.Equals("img")) && variable < 20) {
                var tmpGazeX = gazePointX;
                var tmpGazeY = gazePointY;
                int[] dirArr = { 1, -1 };
                // move to certain directions X
                for (int i = 0; i < dirArr.Length; i++)
                {
                    if ( found == false)
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
                    if ( found == false )
                    {
                        ele = GetElementByPoint(driver, (tmpGazeX), (tmpGazeY + dirArr[i] * variable));
                    }
                    if (ele != null &&(( ele.TagName.Equals("a") && ele.Text.Equals("")) || ele.TagName.Equals("img")))
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
                HoverOverElement(ele);

            }
        }

        private float GetDPIScale()
        {
            var currentDpi = (int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics", "AppliedDPI", 96);
           return (float)currentDpi / 96;
            
        }
    }
}
