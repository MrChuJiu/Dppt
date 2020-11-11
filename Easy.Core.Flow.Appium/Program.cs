using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;


namespace Easy.Core.Flow.Appium
{
    class Program
    {
        static void Main(string[] args)
        {
            AppiumOptions appium = new AppiumOptions();
            appium.AddAdditionalCapability("platformName", "Android");
            appium.AddAdditionalCapability("deviceName", "hdwifi");
            appium.AddAdditionalCapability("platformVersion", "10.0.0");
            appium.AddAdditionalCapability("appPackage", "com.tencent.mm");
            appium.AddAdditionalCapability("appActivity", "com.tencent.mm.ui.LauncherUI");
            appium.AddAdditionalCapability("noReset", true);
            appium.AddAdditionalCapability("unicodeKeyboard", true);
            appium.AddAdditionalCapability("resetKeyboard", true);

            Console.WriteLine("打开微信");
            var driver = new AndroidDriver<AndroidElement>(new Uri("http://127.0.0.1:4723/wd/hub"), appium);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

            #region 根据微信号自动添加好友
            //Console.WriteLine("点击加号");
            //driver.FindElementsById("com.tencent.mm:id/ef9")[0].Click();
            //Console.WriteLine("点击添加好友");
            //driver.FindElementsById("com.tencent.mm:id/gam")[1].Click();
            //Console.WriteLine("输入框");
            //driver.FindElementsById("com.tencent.mm:id/fcn")[0].Click();
            //Console.WriteLine("输入内容");
            //driver.FindElementsById("com.tencent.mm:id/bhn")[0].SendKeys("hd377749229");
            //Console.WriteLine("点击搜索");
            //driver.FindElementsById("com.tencent.mm:id/ga1")[0].Click();
            #endregion

            #region 根据微信号自动发送 对应的消息
            //Console.WriteLine("点击微信搜索框");
            //driver.FindElementsById("com.tencent.mm:id/f8y")[0].Click();
            //Console.WriteLine("在搜索框输入搜索信息");
            //driver.FindElementsById("com.tencent.mm:id/bhn")[0].SendKeys("Bella");
            //Console.WriteLine("点击搜索到的好友");
            //driver.FindElementsById("com.tencent.mm:id/tm")[0].Click();
            //Console.WriteLine("输入文字");
            //driver.FindElementsById("com.tencent.mm:id/al_")[0].SendKeys("hello 我是自动发送程序");
            //Console.WriteLine("输入表情");
            //driver.FindElementsById("com.tencent.mm:id/anz")[0].Click();
            //driver.FindElementsById("com.tencent.mm:id/rv")[0].Click();
            //Console.WriteLine("点击发送按钮发送信息");
            //driver.FindElementsById("com.tencent.mm:id/anv")[0].Click();
            //driver.Quit();
            #endregion

            #region 自动给指定好友发送红包
            //Console.WriteLine("点击微信搜索框");
            //driver.FindElementsById("com.tencent.mm:id/f8y")[0].Click();
            //Console.WriteLine("在搜索框输入搜索信息");
            //driver.FindElementsById("com.tencent.mm:id/bhn")[0].SendKeys("Bella");
            //Console.WriteLine("点击搜索到的好友");
            //driver.FindElementsById("com.tencent.mm:id/tm")[0].Click();
            //Console.WriteLine("点击更多按钮");
            //driver.FindElementsById("com.tencent.mm:id/aks")[0].Click();
            //Console.WriteLine("点击红包");
            //driver.FindElementsById("com.tencent.mm:id/p_")[4].Click();
            //Console.WriteLine("输入金额");
            //driver.FindElementsById("com.tencent.mm:id/dbc")[0].SendKeys("0.01");
            //Console.WriteLine("点击发送");
            //driver.FindElementsById("com.tencent.mm:id/ddo")[0].Click();


            //Thread.Sleep(3000);
            //(new TouchAction(driver)).Tap(937, 1952).Perform();
            //(new TouchAction(driver)).Tap(937, 1848).Perform();
            //(new TouchAction(driver)).Tap(937, 1848).Perform();
            //(new TouchAction(driver)).Tap(563, 1952).Perform();
            //(new TouchAction(driver)).Tap(540, 1848).Perform();
            //(new TouchAction(driver)).Tap(197, 1848).Perform();

            #endregion

            #region 扫描微信好友
            //driver.FindElementsById("com.tencent.mm:id/cn_")[1].Click();
            //Console.WriteLine("获取昵称（备注）");
            //// 微信做了滚动加载 只能获取到当前页面的好友信息 这个比较骚
            //var address_list = driver.FindElementsById("com.tencent.mm:id/dy5");
            //var remarks = new List<string>();
            //foreach (var address in address_list)
            //{
            //    var remark = address.GetAttribute("content-desc");
            //    Console.WriteLine(" # 排除自己和微信官方号");
            //    if (remark != "自己的微信名" && remark.Contains("微信"))
            //    {
            //        remarks.Add(remark);
            //    }
            //}
            //Console.WriteLine("扫描完成");

            #endregion

            #region 检测自己是否被删除
            //Console.WriteLine("点击微信搜索框");
            //driver.FindElementsById("com.tencent.mm:id/f8y")[0].Click();
            //Console.WriteLine("在搜索框输入搜索信息");
            //driver.FindElementsById("com.tencent.mm:id/bhn")[0].SendKeys("Bella");
            //Console.WriteLine("点击搜索到的好友");
            //driver.FindElementsById("com.tencent.mm:id/tm")[0].Click();
            //Console.WriteLine("# 转账");
            //driver.FindElementsById("com.tencent.mm:id/aks")[0].Click();
            //driver.FindElementsById("com.tencent.mm:id/pa")[5].Click();
            //driver.FindElementsById("com.tencent.mm:id/cx_")[0].Click();
            //driver.FindElementsById("com.tencent.mm:id/cxi")[0].Click();
            //Console.WriteLine("# 判断是否被删");
            #endregion
        }
    }
}
