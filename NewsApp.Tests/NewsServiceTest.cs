using NUnit.Framework;
using NewsApp.Services;
using NewsApp.Repository;
using Microsoft.Extensions.Configuration;
using NewsApp.Models;
using System.Collections.Generic;
using System;

namespace NewsApp.Tests.Services
{
    [TestFixture]
    public class NewsItemsTest
    {
        private NewsService _newsService;
        private GoogleNewsRepository _googleNewsRepository;
        private InternalNewsRepository _internalNewsRepository;

        private PressTrustOfIndiaNewsRepository _pressTrustOfIndiaNewsRepository;
        private IConfiguration configuration;

        private List<NewsItem> _newsItems;
        [SetUp]
        public void Setup()
        {
            _newsService = new NewsService(_googleNewsRepository, _internalNewsRepository, _pressTrustOfIndiaNewsRepository, configuration);
            _newsItems = this.getTestData();
        }

        public List<NewsItem> getTestData()
        {
            var newsItems = new List<NewsItem>();

            newsItems.Add(new NewsItem {
                Id = 1,
                Category = "Political",
                IsPriority = true,
                NewsContent = "Test for political news",
                NewsHeading = "Test Political",
                NewsSource = "Google",
                PublishDate = DateTime.Now
            });

            newsItems.Add(new NewsItem {
                Id = 2,
                Category = "Political",
                IsPriority = true,
                NewsContent = "Test for political news",
                NewsHeading = "Test Political",
                NewsSource = "Google",
                PublishDate = DateTime.Now
            });

            newsItems.Add(new NewsItem {
                Id = 3,
                Category = "Political",
                IsPriority = true,
                NewsContent = "Test for political news",
                NewsHeading = "Test Political",
                NewsSource = "Google",
                PublishDate = DateTime.Now
            });

            newsItems.Add(new NewsItem {
                Id = 4,
                Category = "Political",
                IsPriority = true,
                NewsContent = "Test for political news",
                NewsHeading = "Test Political",
                NewsSource = "Google",
                PublishDate = DateTime.Now
            });

            newsItems.Add(new NewsItem {
                Id = 5,
                Category = "Political",
                IsPriority = true,
                NewsContent = "Test for political news",
                NewsHeading = "Test Political",
                NewsSource = "Google",
                PublishDate = DateTime.Now
            });

            newsItems.Add(new NewsItem {
                Id = 6,
                Category = "Political",
                IsPriority = true,
                NewsContent = "Test for political news",
                NewsHeading = "Test Political",
                NewsSource = "Google",
                PublishDate = DateTime.Now
            });

            newsItems.Add(new NewsItem {
                Id = 7,
                Category = "Political",
                IsPriority = true,
                NewsContent = "Test for political news",
                NewsHeading = "Test Political",
                NewsSource = "Google",
                PublishDate = DateTime.Now
            });

            newsItems.Add(new NewsItem {
                Id = 8,
                Category = "Political",
                IsPriority = true,
                NewsContent = "Test for political news",
                NewsHeading = "Test Political",
                NewsSource = "Google",
                PublishDate = DateTime.Now
            });

            return newsItems;
        }
        
        [Test]
        public void GroupNewsItemsNotEmpty()
        {  
            var lstNewsItems = _newsService.GroupNewsItems(_newsItems, 4);
            Assert.IsNotEmpty(lstNewsItems);            
        }

        [Test]
        public void GroupNewsItemsForMactch()
        {  
            var lstNewsItems = _newsService.GroupNewsItems(_newsItems, 8);
            Assert.AreEqual(1, lstNewsItems.Count);            
        }

        [Test]
        public void GroupNewsItemsCheckForNull()
        {  
            var lstNewsItems = _newsService.GroupNewsItems(_newsItems, 8);
            Assert.IsNotNull(lstNewsItems);            
        }

        [Test]
        public void GroupNewsItemsForExactMatch()
        {  
            var lstNewsItems = _newsService.GroupNewsItems(_newsItems, 1);
            Assert.AreEqual(8, lstNewsItems.Count);            
        }

        [Test]
        public void GetNewsItemsPagesNotEmpty()
        {  
            var lstNewsPages = _newsService.GetNewsItemsPages(_newsItems, 8);
            Assert.IsNotEmpty(lstNewsPages);            
        }

        [Test]
        public void GetNewsItemsPagesNotNull()
        {  
            var lstNewsPages = _newsService.GetNewsItemsPages(_newsItems, 8);
            Assert.IsNotNull(lstNewsPages);            
        }

        [Test]
        public void GetNewsItemsPagesSuccess()
        {  
            var lstNewsPages = _newsService.GetNewsItemsPages(_newsItems, 8);
            Assert.AreEqual(1, lstNewsPages.Count);            
        }

        [Test]
        public void GetNonPriorityNewsItemsPagesNotNull()
        {  
            var lstNewsPages = _newsService.GetNonPriorityNewsItemsPages(_newsItems, 8, 2);
            Assert.IsNotNull(lstNewsPages);            
        }

        [Test]
        public void GetNonPriorityNewsItemsPagesSuccess()
        {  
            _newsItems.ForEach(item => item.IsPriority = false);
            var lstNewsPages = _newsService.GetNonPriorityNewsItemsPages(_newsItems, 8, 2);
            Assert.AreEqual(1, lstNewsPages.Count);            
        }

        [Test]        
        public void GetNonPriorityNewsItemsPagesFail()
        {  
            _newsItems.ForEach(item => item.IsPriority = false);            
            Assert.Throws<Exception>(() => _newsService.GetNonPriorityNewsItemsPages(_newsItems, 8, 9));            
        }        
    }
}