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
            webClient = new WebClient
            {
                Encoding = Encoding.GetEncoding("windows-1252")
            };
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

        private string GetDescription(string value)
        {
            var indexof = value.IndexOf("/> ", StringComparison.Ordinal) + 3;
            var length = value.Length;
            string result = value.Substring(indexof, length - indexof);
            return result;
        }

        private string GetImageUrl(string value)
        {
            var indexof = value.IndexOf(@"src=", StringComparison.Ordinal) + 5;
            var cutedString = value.Substring(indexof, value.Length - indexof);
            var findQuotes = cutedString.IndexOf("\"", StringComparison.Ordinal);
            string site = cutedString.Substring(0, findQuotes);

            var textToFind = "http://www.eluniversal";
            var textToReplace = "https://archivo.eluniversal";

            string resultFormatted = site.Contains(textToFind) ? site.Replace(textToFind, textToReplace) : site;
            return resultFormatted;
        }
    }
}