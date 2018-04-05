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

namespace SeleniumApproach
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            IWebDriver driver = new ChromeDriver();
            driver.Url = "https://www.google.ch/search?q=tree&dcr=0&source=lnms&tbm=isch&sa=X&ved=0ahUKEwiew6yFm6PaAhWFkywKHdgCBvYQ_AUICigB&biw=1536&bih=720";


           


            By locator = By.XPath("//*[@id=\"rg_s\"]/div[1]/a/div[2]");
            IWebElement hoverElement = driver.FindElement(By.XPath("//*[@id=\"rg_s\"]/div[1]/a"));
         
            Actions builder = new Actions(driver);
            builder.MoveToElement(hoverElement).Perform();


            Thread.Sleep(10000);

            Console.WriteLine(hoverElement);
        }
    }
}
