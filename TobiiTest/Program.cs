using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tobii.Interaction;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows.Forms;
using System.Net;
using Microsoft.Win32;
using System.Drawing;
using OpenQA.Selenium.Interactions;
using System.Threading;

namespace Interaction_Interactors_101
{
    /// <summary>
    /// Interactor is a region on the screen for which some gaze based behavior is defined.
    /// Most common behaviors the Tobii Core SDK is exposing for interactors is gaze aware
    /// (simply a notion of whether somebody is looking at the region or not), activatable
    /// (which means region has some action associated with it, which can be triggered, but
    /// only if someone is looking at it at the same time), pannable (provides panning like
    /// behavior with associated actions, which can be triggered, but again only if someone
    /// is looking at it at the same time). 
    /// 
    /// To help you manage interactors, the Tobii Core SDK provides another concept - InteractorAgents.
    /// When you do not work with WPF or WindowsForms, the Tobii Core SDK has UnboundInteractorAgent,
    /// which you can use to control everything related to interactors.
    /// 
    /// Gaze aware is the most basic behavior we could think of, so let's see how its easy to
    /// define 'you are looking at it' interactor with the Tobii Core SDK.
    /// </summary>
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Opens a browser window.
            IWebDriver driver = new ChromeDriver();
            driver.Url = "http://www.google.com";

            // Everything starts with initializing Host, which manages connection to the 
            // Tobii Engine and provides all the Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            var host = new Host();

            // 2. Create stream. 
            var gazePointDataStream = host.Streams.CreateGazePointDataStream();


            var currentDpi = (int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics", "AppliedDPI", 96);

            var scale = (float)currentDpi / 96;
            Boolean done = false;
            while (true)
            {
                
                if (Control.ModifierKeys == System.Windows.Forms.Keys.Control)
                {
                    gazePointDataStream.Next += (a, gaze) =>
                    {
                        if (!done)
                        {
                            var gazePointX = gaze.Data.X / scale;
                            var gazePointY = gaze.Data.Y / scale;
                            // var mouseX = Cursor.Position.X;
                            // var mouseY = Cursor.Position.Y;

                            IWebElement ele = null;

                            try
                            {
                                if (driver is IJavaScriptExecutor)
                                    // Gets the element at the coordinates using some java script and selenium.^^
                                    ele = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
                                        "return document.elementFromPoint(arguments[0], arguments[1])",
                                        gazePointX, gazePointY);

                                // Select the element found
                                if (ele != null)
                                {
                                    Thread.Sleep(1000);
                                    Actions action = new Actions(driver);
                                    action.ContextClick(ele).Build().Perform();
                                    Thread.Sleep(100);
                                    action.SendKeys("v").Build().Perform();

                                    /*  var dialog = new SaveFileDialog();
                                      //shows save file dteialog
                                      var result = dialog.ShowDialog();
                                      if (result == DialogResult.OK)
                                      {
                                          Console.WriteLine("writing to: " + dialog.FileName); //prints the file to save
                                          var url = ele.GetAttribute("src");
                                          var width = ele.GetAttribute("width");
                                          var height = ele.GetAttribute("height");
                                          var wClient = new WebClient();
                                          wClient.DownloadFile(url, dialog.FileName);
                                      }*/

                                }

                                done = true;
                                host.DisableConnection();
                            }
                            catch (Exception f)
                            {
                                Console.WriteLine(f);
                                done = true;
                                host.DisableConnection();
                            }
                            
                       
                           

                        }

                        };

                 


                    /*   fixationDataStream.Next += (o, fixation) =>
                       {

                           // On the Next event, data comes as FixationData objects, wrapped in a StreamData<T> object.
                           var fixationPointX = fixation.Data.X / scale;
                           var fixationPointY = fixation.Data.Y / scale;
                           var mouseX = Cursor.Position.X;
                           var mouseY = Cursor.Position.Y;

                       }; */

                }
            }
        }

        #region Helpers 


        private static void PrintSampleIntroText()
        {
            Console.Clear();
            Console.WriteLine("============================================================");
            Console.WriteLine("|           Tobii Core SDK: Interactors                    |");
            Console.WriteLine("============================================================");

            Console.WriteLine();
            Console.WriteLine("This sample will demonstrate you the usage of GazeAware interactors.");
            Console.WriteLine("Look at the window to trigger HasGaze event and look away to trigger\n" +
                              "LostGaze event.");

            Console.WriteLine();
        }

        private static Tobii.Interaction.Rectangle GetWindowBounds(IntPtr windowHandle)
        {
            NativeRect nativeNativeRect;
            if (GetWindowRect(windowHandle, out nativeNativeRect))
                return new Tobii.Interaction.Rectangle
                {
                    X = nativeNativeRect.Left,
                    Y = nativeNativeRect.Top,
                    Width = nativeNativeRect.Right,
                    Height = nativeNativeRect.Bottom


                };

            return new Tobii.Interaction.Rectangle(0d, 0d, 1000d, 1000d);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, out NativeRect nativeRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeRect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion
    }
}