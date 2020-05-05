using System;

namespace NewsApp.Models
{
    public class NewsItem
    {
        public int Id { get; set; }
        public string NewsHeading { get; set; }
        public string NewsContent { get; set;}
        public DateTime PublishDate { get; set; }
        public string NewsSource { get; set; }
        public string Category { get; set;}        
        public bool IsPriority { get; set;}     
    }
}