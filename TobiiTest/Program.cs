using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Tobii.Interaction;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Windows.Forms;
using System.Net;
using Microsoft.Win32;

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

            IWebDriver driver = new ChromeDriver();
            driver.Url = "http://www.google.com";

            // Everything starts with initializing Host, which manages connection to the 
            // Tobii Engine and provides all the Tobii Core SDK functionality.
            // NOTE: Make sure that Tobii.EyeX.exe is running
            var host = new Host();

            // 2. Create stream. 
            // var gazePointDataStream = host.Streams.CreateGazePointDataStream();
            var fixationDataStream = host.Streams.CreateFixationDataStream();


            var currentDpi = (int)Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop\\WindowMetrics", "AppliedDPI", 96);

            var scale = (float)currentDpi / 96;

            while (true)
            {
                if (Control.ModifierKeys == System.Windows.Forms.Keys.Control)
                {

                    Console.WriteLine(Cursor.Position.X);
                    Console.WriteLine(Cursor.Position.Y);

                    
                    fixationDataStream.Next += (o, fixation) =>
                    {

                        // On the Next event, data comes as FixationData objects, wrapped in a StreamData<T> object.
                        var fixationPointX = fixation.Data.X / scale;
                        var fixationPointY = fixation.Data.Y / scale;

                        Console.WriteLine(fixationPointX
                           );
                        Console.WriteLine(fixationPointY);

                        IWebElement ele = null;

                        try
                        {
                            //gazePointDataStream.GazePoint((x, y, ts) => Console.WriteLine("Timestamp: {0}\t X: {1} Y:{2}", ts, x, y));
                            // Find the element at the mouse position
                            if (driver is IJavaScriptExecutor)

                                ele = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript(
                                    "return document.elementFromPoint(arguments[0], arguments[1])",
                                    fixationPointX, fixationPointY );

                            // okay, it is 4 lines, but you won't be able to see much without this one :)
                            Console.ReadKey();

                            // we will close the coonection to the Tobii Engine before exit.
                            host.DisableConnection();
                            // Select the element found
                            if (ele != null)
                            {

                                var dialog = new SaveFileDialog();

                                var result = dialog.ShowDialog(); //shows save file dialog
                                if (result == DialogResult.OK)
                                {
                                    Console.WriteLine("writing to: " + dialog.FileName); //prints the file to save

                                    var wClient = new WebClient();
                                    wClient.DownloadFile(ele.GetAttribute("src"), dialog.FileName);
                                }

                            }
                        }
                        catch (Exception f)
                        {
                            Console.WriteLine(f);
                        }
                    };

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

        private static Rectangle GetWindowBounds(IntPtr windowHandle)
        {
            NativeRect nativeNativeRect;
            if (GetWindowRect(windowHandle, out nativeNativeRect))
                return new Rectangle
                {
                    X = nativeNativeRect.Left,
                    Y = nativeNativeRect.Top,
                    Width = nativeNativeRect.Right,
                    Height = nativeNativeRect.Bottom
                };

            return new Rectangle(0d, 0d, 1000d, 1000d);
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