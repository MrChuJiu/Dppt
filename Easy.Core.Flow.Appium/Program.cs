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
        static AndroidDriver<AndroidElement> driver = null;
        static int width = 1080;
        static int height = 1920;


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
            driver = new AndroidDriver<AndroidElement>(new Uri("http://127.0.0.1:4723/wd/hub"), appium);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);


            // EnvelopesWeiChatFriends("Bella","0.01");


        }
        /// <summary>
        /// 根据好友微信号判断自己是否被删
        /// </summary>
        /// <param name="friend"></param>
        /// <returns></returns>
        static bool IsDeleteWeiChatFriends(string friend) {
            Console.WriteLine("点击微信搜索框");
            driver.FindElementsById("com.tencent.mm:id/f8y")[0].Click();
            Console.WriteLine("在搜索框输入搜索信息");
            driver.FindElementsById("com.tencent.mm:id/bhn")[0].SendKeys(friend);
            Console.WriteLine("点击搜索到的好友");
            driver.FindElementsById("com.tencent.mm:id/tm")[0].Click();
            Console.WriteLine("# 转账");
            driver.FindElementsById("com.tencent.mm:id/aks")[0].Click();
            driver.FindElementsById("com.tencent.mm:id/pa")[5].Click();
            driver.FindElementsById("com.tencent.mm:id/cx_")[0].Click();
            driver.FindElementsById("com.tencent.mm:id/cxi")[0].Click();
            Thread.Sleep(1000);
            Console.WriteLine("# 判断是否被删");
            var isExist = IsElementExist("com.tencent.mm:id/jh");
            return isExist;
        }
        /// <summary>
        /// 给指定的好友发送红包
        /// </summary>
        static void EnvelopesWeiChatFriends(string friend, string money)
        {
            Console.WriteLine("点击微信搜索框");
            driver.FindElementsById("com.tencent.mm:id/f8y")[0].Click();
            Console.WriteLine("在搜索框输入搜索信息");
            driver.FindElementsById("com.tencent.mm:id/bhn")[0].SendKeys(friend);
            Console.WriteLine("点击搜索到的好友");
            driver.FindElementsById("com.tencent.mm:id/tm")[0].Click();
            Console.WriteLine("点击更多按钮");
            driver.FindElementsById("com.tencent.mm:id/aks")[0].Click();
            Console.WriteLine("点击红包");
            driver.FindElementsById("com.tencent.mm:id/p_")[4].Click();
            Console.WriteLine("输入金额");
            driver.FindElementsById("com.tencent.mm:id/dbc")[0].SendKeys(money);
            Console.WriteLine("点击发送");
            driver.FindElementsById("com.tencent.mm:id/ddo")[0].Click();

            Thread.Sleep(3000);
            (new TouchAction(driver)).Tap(937, 1952).Perform();
            (new TouchAction(driver)).Tap(937, 1848).Perform();
            (new TouchAction(driver)).Tap(937, 1848).Perform();
            (new TouchAction(driver)).Tap(563, 1952).Perform();
            (new TouchAction(driver)).Tap(540, 1848).Perform();
            (new TouchAction(driver)).Tap(197, 1848).Perform();

        }
        /// <summary>
        /// 给指定好友发送指定消息
        /// </summary>
        /// <param name="friends"></param>
        static void NoticeWeiChatFriends(string friend, string message)
        {
            Console.WriteLine("点击微信搜索框");
            driver.FindElementsById("com.tencent.mm:id/f8y")[0].Click();
            Console.WriteLine("在搜索框输入搜索信息");
            driver.FindElementsById("com.tencent.mm:id/bhn")[0].SendKeys(friend);
            Console.WriteLine("点击搜索到的好友");
            driver.FindElementsById("com.tencent.mm:id/tm")[0].Click();
            Console.WriteLine("输入文字");
            driver.FindElementsById("com.tencent.mm:id/al_")[0].SendKeys(message);
            Console.WriteLine("输入表情");
            driver.FindElementsById("com.tencent.mm:id/anz")[0].Click();
            driver.FindElementsById("com.tencent.mm:id/rv")[0].Click();
            Console.WriteLine("点击发送按钮发送信息");
            driver.FindElementsById("com.tencent.mm:id/anv")[0].Click();
        }
        /// <summary>
        /// 根据给出的微信号添加好友
        /// </summary>
        /// <param name="friends"></param>
        static void AddWeiChatFriends(string friend) {
            Console.WriteLine("点击加号");
            driver.FindElementsById("com.tencent.mm:id/ef9")[0].Click();
            Console.WriteLine("点击添加好友");
            driver.FindElementsById("com.tencent.mm:id/gam")[1].Click();
            Console.WriteLine("输入框");
            driver.FindElementsById("com.tencent.mm:id/fcn")[0].Click();
            Console.WriteLine("输入内容");
            driver.FindElementsById("com.tencent.mm:id/bhn")[0].SendKeys(friend);
            Console.WriteLine("点击搜索");
            driver.FindElementsById("com.tencent.mm:id/ga1")[0].Click();
        }
        /// <summary>
        /// 获取微信号所有好友
        /// </summary>
        /// <returns></returns>
        static List<string> GetWeiChatAddressAllList() {

            var remarks = new List<string>();
            var res = GetWeiChatAddressList(true);
            remarks.AddRange(res);

            while (true)
            {
                var isEnd = IsElementExist("com.tencent.mm:id/azb");
                Thread.Sleep(100);
                res = GetWeiChatAddressList(false);
                remarks.AddRange(res);
                if (isEnd)
                {
                    break;
                }
            }

            Console.WriteLine("获取好友列表名称完成");
            Thread.Sleep(100);
            return remarks;
        }
        /// <summary>
        /// 获取微信当前页面好友列表
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        static List<string> GetWeiChatAddressList(bool flag) {
            if (flag) {
                driver.FindElementsById("com.tencent.mm:id/cn_")[1].Click();
                Thread.Sleep(100);
                Console.WriteLine("上滑");
                SwipeUp(1 / 2, 2000);
            } else {
                SwipeUp(5 / 6, 2000);
            }

            Thread.Sleep(100);
            Console.WriteLine("获取昵称（备注）");
            var addressList = driver.FindElementsById("com.tencent.mm:id/dy5");
            var remarks = new List<string>();
            foreach (var address in addressList)
            {
                var remark = address.GetAttribute("content-desc");
                Console.WriteLine(" # 排除自己和微信官方号");
                if (remark != "自己的微信名" && remark.Contains("微信"))
                {
                    remarks.Add(remark);
                }
            }

            return remarks;
        }
        /// <summary>
        /// 判断是否存在某个Id
        /// </summary>
        /// <param name="element"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        static bool IsElementExist(string element, Int32 timeout = 3) {
            var count = 0;
            while (count < timeout)
            {
                var souce = driver.PageSource;
                if (souce.Contains(element)) {
                    return true;
                }
                else {
                    count += 1;
                    Thread.Sleep(100);
                    return false;
                }
            }
            return false;
        }
        /// <summary>
        /// 向上滑动
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="time"></param>
        static void SwipeUp(float distance, int time) {

            //driver.StartActivityWithIntent(1 / 2 * width, 9 / 10 * height, 1 / 2 * width, (9 / 10 - distance) * height, time)
        }
        /// <summary>
        /// 返回搜索框 / 暂时只适用于删除好友需要定制
        /// </summary>
        static void SerchBack() {
            Thread.Sleep(100);
            driver.FindElementById("com.tencent.mm:id/dn").Click();
            Thread.Sleep(100);
            driver.FindElementById("com.tencent.mm:id/rs").Click();
            Thread.Sleep(100);
            // 清除搜索框，输入下一个
            driver.FindElementById("com.tencent.mm:id/fsv").Click();
        }
        /// <summary>
        /// 根据好友名称删除好友
        /// </summary>
        /// <param name="friend"></param>
        static void DelFriend(string friend) {

            Console.WriteLine("点击微信搜索框");
            driver.FindElementById("com.tencent.mm:id/cn1").Click();
            Console.WriteLine("在搜索框输入搜索信息");
            Thread.Sleep(100);
            driver.FindElementById("com.tencent.mm:id/bhn").SendKeys(friend);
            Thread.Sleep(100);
            Console.WriteLine("点击搜索到的人");
            driver.FindElementById("com.tencent.mm:id/tm").Click();
            Thread.Sleep(100);
            Console.WriteLine("点击聊天对话框右上角...");
            driver.FindElementById("com.tencent.mm:id/cj").Click();
            Thread.Sleep(100);
            Console.WriteLine("点击头像");
            driver.FindElementById("com.tencent.mm:id/f3y").Click();
            Thread.Sleep(100);
            Console.WriteLine("点击联系人右上角...");
            driver.FindElementById("com.tencent.mm:id/cj").Click();
            Thread.Sleep(100);
            Console.WriteLine("点击删除按钮");
            driver.FindElementById("com.tencent.mm:id/g6f").Click();
            Thread.Sleep(100);
            Console.WriteLine("点击弹出框中的删除");
            driver.FindElementById("com.tencent.mm:id/doz").Click();
        }
    }
}
