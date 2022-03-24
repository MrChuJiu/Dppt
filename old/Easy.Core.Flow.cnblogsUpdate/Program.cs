using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Easy.Core.Flow.cnblogsUpdate
{
    internal class Program
    {
        static string Cookie = "";
        static string XsrfCookie = "";

        static string SoureStr = "https://git.imweb.io/hdong/ImageBed/raw/master/";
        static string TargetStr = "https://gitee.com/github-HD/image-bed/raw/master/";

        static int Page = 7;
        static List<string> UrlList = new List<string>();

        static async Task Main(string[] args)
        {

            var handler = new HttpClientHandler() { UseCookies = false };
            var client = new HttpClient(handler);

            for (int i = 1; i <= Page; i++)
            {
                var message = new HttpRequestMessage(HttpMethod.Get, $"https://i.cnblogs.com/api/posts/list?p={i}&cid=&tid=&t=1&cfg=0&search=&orderBy=&scid=");
                message.Headers.Add("Cookie", Cookie);
                var result = await client.SendAsync(message);
                result.EnsureSuccessStatusCode();
                var value = await result.Content.ReadAsStringAsync();
                var rootData = JsonSerializer.Deserialize<Root>(value);

                foreach (var item in rootData.postList)
                {
                    UrlList.Add("https://i.cnblogs.com/api/posts/" + item.id);
                }
            }

            foreach (var item in UrlList) {

                var message = new HttpRequestMessage(HttpMethod.Get, item);
                message.Headers.Add("Cookie", Cookie);
                var result = await client.SendAsync(message);
                result.EnsureSuccessStatusCode();
                var value = await result.Content.ReadAsStringAsync();
                var rootData = JsonSerializer.Deserialize<RootPost>(value);
                rootData.blogPost.postBody = rootData.blogPost.postBody.Replace(SoureStr, TargetStr);

                var str = JsonSerializer.Serialize(rootData.blogPost);
                HttpContent content = new StringContent(str);
                content.Headers.Add("cookie", Cookie);
                content.Headers.Add("x-xsrf-token", XsrfCookie);
                content.Headers.Add("origin", "https://i.cnblogs.com");
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                HttpResponseMessage response = await client.PostAsync("https://i.cnblogs.com/api/posts", content);//改成自己的
                response.EnsureSuccessStatusCode();//用来抛异常的

            }


            Console.Read();
        }
    }



    //如果好用，请收藏地址，帮忙分享。
    public class PostListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 浅讲.Net 6 之 WebApplicationBuilder
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isPublished { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isDraft { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int feedBackCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int webCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int aggCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int viewCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string datePublished { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string entryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int postType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int postConfig { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dateUpdated { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int accessPermission { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isInSiteHome { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isInSiteCandidate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isMarkdown { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public List<PostListItem> postList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int postsCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string categoryName { get; set; }
    }








    //如果好用，请收藏地址，帮忙分享。
    public class BlogPost
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int postType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int accessPermission { get; set; }
        /// <summary>
        /// Abp vNext 番外篇-疑难杂症丨浅谈扩展属性与多用户设计
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string postBody { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<int> categoryIds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool inSiteCandidate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool inSiteHome { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string siteCategoryId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> blogTeamIds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isPublished { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool displayOnHomePage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isAllowComments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool includeInMainSyndication { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isPinned { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isOnlyForRegisterUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isUpdateDateAdded { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string entryName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string featuredImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> tags { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string datePublished { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isMarkdown { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isDraft { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string autoDesc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool changePostType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int blogId { get; set; }
        /// <summary>
        /// 初久的私房菜
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool removeScript { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string clientInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool changeCreatedTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool canChangeCreatedTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isContributeToImpressiveBugActivity { get; set; }
    }

    public class RootPost
    {
        /// <summary>
        /// 
        /// </summary>
        public BlogPost blogPost { get; set; }
    }





}
