using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Internal;
using System;
using System.Threading;

namespace Easy.Core.Flow.XueXiQG
{
    class Program
    {
        static AndroidDriver<AndroidElement> driver = null;
        // 手机分辨率
        static int width = 1080;
        static int height = 1920;

        static void Main(string[] args)
        {
            // adb shell dumpsys package cn.xuexi.android
            // android.intent.action.MAIN
            // com.alibaba.android.rimet.biz.SplashActivity

            //{
            //  "platformName": "Android",
            //  "deviceName": "hdwifi",
            //  "platformVersion": "10.0.0",
            //  "appPackage": "cn.xuexi.android",
            //  "appActivity": "com.alibaba.android.rimet.biz.SplashActivity",
            //  "noReset": true,
            //  "unicodeKeyboard": true,
            //  "resetKeyboard": true
            //}

            Console.WriteLine("Hello World!");

            AppiumOptions appium = new AppiumOptions();
            appium.AddAdditionalCapability("platformName", "Android");
            appium.AddAdditionalCapability("deviceName", "hdwifi");
            appium.AddAdditionalCapability("platformVersion", "10.0.0");
            appium.AddAdditionalCapability("appPackage", "cn.xuexi.android");
            appium.AddAdditionalCapability("appActivity", "com.alibaba.android.rimet.biz.SplashActivity");
            appium.AddAdditionalCapability("noReset", true);
            appium.AddAdditionalCapability("unicodeKeyboard", true);
            appium.AddAdditionalCapability("resetKeyboard", true);


            Console.WriteLine("打开强国");
            driver = new AndroidDriver<AndroidElement>(new Uri("http://127.0.0.1:4723/wd/hub"), appium);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            Thread.Sleep(5000);




            // 打开一个英文电台 让其后台运行
            RadioStation(2);

            // 预览山东的文章 并分享
            StudyShanDong(2);
            StudyShanDong(3);

            // 预览新闻资讯 并评论然后删除掉
            StudyJournalism(1);
            StudyJournalism(2);

            //等待4分钟 让电台听完
            Thread.Sleep(50000);

        }

        /// <summary>
        /// 打开电台听一个英文节目
        /// </summary>
        /// <returns></returns>
        static void RadioStation(int Tag = 2)
        {

            Console.WriteLine("点击电台按钮");

            // 点击电台按钮
            (new TouchAction(driver)).Tap(968, 2246).Perform();
            Thread.Sleep(2000);

            Console.WriteLine("寻找听英语");
            // 滑动到最后听英语
            TouchAction actions = new TouchAction((IPerformsTouchActions)driver);
            actions.Press(906, 338).Wait(300).MoveTo(80, 338).Release().Perform();
            Thread.Sleep(1000);
            var groupList = driver.FindElementsByXPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[1]/android.support.v4.view.ViewPager/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.view.ViewGroup/android.widget.LinearLayout");

            for (int i = 0; i < groupList.Count; i++)
            {
                var path = $@"/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[1]/android.support.v4.view.ViewPager/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.view.ViewGroup/android.widget.LinearLayout[{i+1}]/android.widget.TextView";
               
                var remark = driver.FindElementByXPath(path).GetAttribute("text");
                if (remark == "听英语") {

                    driver.FindElementByXPath(path).Click();
                    Thread.Sleep(2000);
                    Console.WriteLine("开始听英语新闻");
  
                    driver.FindElementByXPath($@"/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[1]/android.support.v4.view.ViewPager/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ListView/android.widget.FrameLayout[{Tag}]").Click();
                    Thread.Sleep(2000);

                    driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[2]/android.widget.FrameLayout[2]/android.widget.ListView/android.widget.LinearLayout[1]/android.widget.LinearLayout").Click();
                    Console.WriteLine("小话到后台让他去听");

                    driver.FindElementById("cn.xuexi.android:id/btn_back").Click();

                    Thread.Sleep(1000);
                    driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout[1]/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[2]/android.widget.FrameLayout[1]/android.widget.LinearLayout").Click();
                    Console.WriteLine("返回到主页");
                    Thread.Sleep(1000);

                    (new TouchAction(driver)).Tap(535, 2246).Perform();
                    Thread.Sleep(2000);

                    return;
                }
            }

        }


        /// <summary>
        /// 学习山东
        /// </summary>
        static void StudyShanDong(int Tag = 2) {

            Console.WriteLine("开始找山东频道");
            var groupList = driver.FindElementsByXPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[1]/android.support.v4.view.ViewPager/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.view.ViewGroup/android.widget.LinearLayout");
            for (int i = 0; i < groupList.Count; i++)
            {
                var path = $@"/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[1]/android.support.v4.view.ViewPager/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.LinearLayout/android.view.ViewGroup/android.widget.LinearLayout[{i + 1}]/android.widget.TextView";
                var remark = driver.FindElementByXPath(path).GetAttribute("text");
                if (remark == "山东") {

                    driver.FindElementByXPath(path).Click();
                    Thread.Sleep(2000);
                    Console.WriteLine("进入山东");

                    var sdXuexi = driver.FindElementsByXPath($@"/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[1]/android.support.v4.view.ViewPager/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ListView/android.widget.FrameLayout[1]/android.widget.LinearLayout/android.widget.FrameLayout/android.support.v7.widget.RecyclerView/android.widget.LinearLayout[1]");

                    for (int c = 0; c < sdXuexi.Count; c++)
                    {
                        var sdXueXiItem = @$"/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[1]/android.support.v4.view.ViewPager/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ListView/android.widget.FrameLayout[1]/android.widget.LinearLayout/android.widget.FrameLayout/android.support.v7.widget.RecyclerView/android.widget.LinearLayout[{c + 1}]/android.widget.TextView";
                        var sdXueXiText  = driver.FindElementByXPath(sdXueXiItem).GetAttribute("text");
                        if (sdXueXiText == "山东学习平台") {
                            Console.WriteLine("找到山东学习平台");
                            driver.FindElementByXPath(sdXueXiItem).Click();
                            Thread.Sleep(2000);

                            Console.WriteLine("获取山东新闻列表");

                            var sdXinWenItemPaht = $@"/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[4]/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ListView/android.widget.FrameLayout[{Tag}]";
                            driver.FindElementByXPath(sdXinWenItemPaht).Click();
                            Console.WriteLine("开始预览新闻 随便滑动模拟人工");
                            Thread.Sleep(2000);

                            TouchAction actions = new TouchAction((IPerformsTouchActions)driver);
                            actions.Press(0, 1200).Wait(300).MoveTo(0, 1000).Release().Perform();
                            Thread.Sleep(1000);

                            Thread.Sleep(20000);

                            Console.WriteLine("等待预览 一会时间");


                            Console.WriteLine("分享新闻");
                            driver.FindElementByXPath(@$"/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.ImageView[2]").Click();
                            Thread.Sleep(2000);
                            driver.FindElementByXPath(@$"/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.RelativeLayout/android.support.v4.view.ViewPager/android.widget.GridView/android.widget.RelativeLayout[2]/android.widget.ImageView").Click();
                            Thread.Sleep(2000);
                            driver.FindElementById("com.tencent.mm:id/dn").Click();


                            Thread.Sleep(20000);

                            Thread.Sleep(2000);
                            Console.WriteLine("返回到主页面");
                            driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout[2]/android.widget.ImageView[1]").Click();
                            Thread.Sleep(2000);

                            driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[3]/android.widget.ImageView").Click();

                            Thread.Sleep(2000);
                            (new TouchAction(driver)).Tap(535, 2246).Perform();
                            Thread.Sleep(2000);


                            return;
                        }

                    }

                    return;

                }


            }


        }

        /// <summary>
        /// 打开新闻 并预览 评论 删除
        /// </summary>
        /// <param name="Tag"></param>
        static void StudyJournalism(int Tag = 1) {

            Console.WriteLine("开始打开新闻");
            driver.FindElementByXPath($@"/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.LinearLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[1]/android.support.v4.view.ViewPager/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.ListView/android.widget.FrameLayout[{Tag}]").Click();
            Thread.Sleep(30000);

            Console.WriteLine("添加评论");
            driver.FindElementByXPath($@"/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.LinearLayout/android.widget.TextView").Click();
            Thread.Sleep(2000);
            driver.FindElementByXPath($@"/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.EditText").SendKeys("好好学习，天天向上，加油加油！");
            Thread.Sleep(2000);
            driver.FindElementByXPath($@"/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.RelativeLayout/android.widget.TextView[2]").Click();
            Thread.Sleep(3000);

            Console.WriteLine("删除掉自己的评论");
            driver.FindElementByXPath($@"/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout[1]/android.view.ViewGroup/android.support.v7.widget.RecyclerView/android.widget.LinearLayout[3]/android.widget.LinearLayout[3]/android.widget.TextView[3]").Click();
            Thread.Sleep(2000);
            driver.FindElementByXPath($@"/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.ScrollView/android.widget.LinearLayout/android.widget.Button[2]").Click();

            Thread.Sleep(30000);
            Console.WriteLine("看完新闻返回主页面");
            driver.FindElementByXPath($@"/hierarchy/android.widget.FrameLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.FrameLayout[2]/android.widget.FrameLayout[2]/android.widget.LinearLayout/android.widget.FrameLayout/android.widget.LinearLayout[2]/android.widget.ImageView[1]").Click();
            

        }


    }
}
