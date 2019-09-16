using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using xf.simp.news.Model;

namespace xf.simp.news.Services
{
    public class UniversalMxMService
    {
        private readonly Uri UrlRss;
        private readonly WebClient webClient;
        public event EventHandler<GenericEventArgs<List<Report>>> GetNews_Completed;

        public UniversalMxMService()
        {
            webClient = new WebClient();
            //{
            //    Encoding = Encoding.GetEncoding("windows-1252")
            //}
            //;
            UrlRss = new Uri("https://rss.nytimes.com/services/xml/rss/nyt/World.xml");
        }

        public void GetNews()
        {
            webClient.DownloadStringCompleted += (s, a) =>
            {
                try
                {
                    var resultString = a.Result;
                    var xdocument = XDocument.Parse(resultString);

                    XNamespace atom = xdocument.Root.GetNamespaceOfPrefix("atom");
                    XNamespace media = xdocument.Root.GetNamespaceOfPrefix("media");

                    var listOfNews = (from news in xdocument.Descendants("channel").Elements("item")
                                      where news.Element(media + "content")?.Value != null
                                      select new Report
                                      {
                                          Title = news.Element("title").Value,
                                          Description = news.Element("description").Value,
                                          UrlImage = news.Element(media + "content").Attribute("url").Value,
                                          PubDate = news.Element("pubDate").Value,
                                          Link = news.Element("link").Value
                                      }).ToList();

                    GetNews_Completed?.Invoke(this, new GenericEventArgs<List<Report>>(listOfNews));
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            };

            webClient.DownloadStringAsync(UrlRss);
        }
    }
}