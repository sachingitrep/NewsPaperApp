using System;
using System.Collections.Generic;

namespace NewsApp.Models
{
    public class NewsPage
    {
        public NewsPage()
        {
            newsItems = new List<NewsItem>();
        }
        public List<NewsItem> newsItems { get; set; }              
    }
}