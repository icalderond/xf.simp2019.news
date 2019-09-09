using System;
using System.Net;

namespace xf.simp.news.Helpers
{
    public class WebClientHandler : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest w = base.GetWebRequest(address);
            w.Timeout = 20 * 60 * 1000;
            return w;
        }
    }
}
