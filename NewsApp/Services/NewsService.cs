using System;
using System.Linq;
using System.Collections.Generic;
using NewsApp.Models;
using NewsApp.Repository;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace NewsApp.Services
{
    public class NewsService : INewsService
    {        
        private GoogleNewsRepository _googleNewsRepository;
        private InternalNewsRepository _internalNewsRepository;
        private PressTrustOfIndiaNewsRepository _pressTrustOfIndiaNewsRepository;
        private IConfiguration _configuration;

        public NewsService(GoogleNewsRepository googleNewsRepository, InternalNewsRepository internalNewsRepository, PressTrustOfIndiaNewsRepository pressTrustOfIndiaNewsRepository, IConfiguration configuration)
        {
            _googleNewsRepository = googleNewsRepository;
            _internalNewsRepository = internalNewsRepository;
            _pressTrustOfIndiaNewsRepository = pressTrustOfIndiaNewsRepository;
            _configuration = configuration;            
        }
        
        public async Task<List<NewsPage>> GetNewsPages(string category)
        {
            var newsPages = new List<NewsPage>();
            var newsItems = new List<NewsItem>();
            var maxNewsPages = _configuration.GetValue<int>("NewsConfiguration:maxNewsPages");
            var maxNewsItemPerPage = _configuration.GetValue<int>("NewsConfiguration:maxNewsItemPerPage");
            var maxAdvertisementPerPage = _configuration.GetValue<int>("NewsConfiguration:maxAdvertisementPerPage");


            var googleNewsItems = _googleNewsRepository.GetNewsContents(category);
            var internalNewsItems = _internalNewsRepository.GetNewsContents(category);
            var pressTrustOfIndiaNewsItems = _pressTrustOfIndiaNewsRepository.GetNewsContents(category);

            await Task.WhenAll(googleNewsItems, internalNewsItems, pressTrustOfIndiaNewsItems);


            newsItems.AddRange(googleNewsItems.Result);
            newsItems.AddRange(internalNewsItems.Result);
            newsItems.AddRange(pressTrustOfIndiaNewsItems.Result);

            List<NewsItem> itemsForPriorityNewsItems, itemsForNonPriorityNewsItems;
            GetPriorityandNonPriorityNewsItems(newsItems, maxNewsItemPerPage, out itemsForPriorityNewsItems, out itemsForNonPriorityNewsItems);

            var priorityNewsPages = GetNewsItemsPages(itemsForPriorityNewsItems, maxNewsItemPerPage);
            var nonPriorityNewsPages = GetNonPriorityNewsItemsPages(itemsForNonPriorityNewsItems, maxNewsItemPerPage, maxAdvertisementPerPage);
            newsPages.AddRange(priorityNewsPages);
            newsPages.AddRange(nonPriorityNewsPages);

            return newsPages.Take(maxNewsPages).ToList();
        }

        private static void GetPriorityandNonPriorityNewsItems(List<NewsItem> newsItems, int maxNewsItemPerPage, out List<NewsItem> itemsForPriorityNewsItems, out List<NewsItem> itemsForNonPriorityNewsItems)
        {
            var priorityNewsItems = newsItems.Where(n => n.Category != "Advertisement" && n.IsPriority == true).ToList();
            var countToSkipFromPriorityNewsItems = priorityNewsItems.Count() % maxNewsItemPerPage;
            var countToTakePriorityNewsItems = priorityNewsItems.Count() - countToSkipFromPriorityNewsItems;
            itemsForPriorityNewsItems = priorityNewsItems.Take(countToTakePriorityNewsItems).OrderByDescending(n => n.PublishDate).ToList();
            itemsForNonPriorityNewsItems = newsItems.Except(itemsForPriorityNewsItems).ToList();
        }       

        public List<NewsPage> GetNonPriorityNewsItemsPages(List<NewsItem> newsItems, int maxItemPerPage, int maxAdvertisementPerPage)
        {
            var newsPages = new List<NewsPage>();

            if(maxItemPerPage < maxAdvertisementPerPage)
            {
                throw new System.Exception("Advertisements per page should not be greater than max items per page");
            }

            var advertisements = newsItems.Where(n => n.Category == "Advertisement").OrderByDescending(n => n.PublishDate).ToList();
            var newsItemsWithoutAdvertisements = newsItems.Where(n => n.Category != "Advertisement")
            .OrderBy(n => n.IsPriority).ThenByDescending(n => n.PublishDate).ToList();
            
            var arrNewsItemsWithoutAdvertisements = GroupNewsItems(newsItemsWithoutAdvertisements, maxItemPerPage - maxAdvertisementPerPage).ToArray();
            var arrAdvertisements = GroupNewsItems(advertisements, maxAdvertisementPerPage).ToArray();
                        
            if(arrNewsItemsWithoutAdvertisements.Count() == arrAdvertisements.Count())
            {
                newsPages = AddNewsItemsPages(0, 0, null, arrAdvertisements, arrNewsItemsWithoutAdvertisements, true);
            }
            else if(arrNewsItemsWithoutAdvertisements.Count() > arrAdvertisements.Count())
            {

                newsPages = AddNewsItemsPages(maxItemPerPage, (maxItemPerPage - maxAdvertisementPerPage), newsItemsWithoutAdvertisements, arrAdvertisements, arrNewsItemsWithoutAdvertisements, false);
            }
            else
            {
               newsPages = AddNewsItemsPages(maxItemPerPage, maxAdvertisementPerPage, advertisements, arrNewsItemsWithoutAdvertisements, arrAdvertisements, false);
            }

            return newsPages;            
        }

        private List<NewsPage> AddNewsItemsPages(int maxItemPerPage, int itemPerPage, List<NewsItem> newsItems, List<NewsItem>[] arrNewsItemsMinimum, List<NewsItem>[] arrNewsItemsMaximum, bool isEqualItems)
        {
            var newsPages = new List<NewsPage>();

            for (int i = 0; i < arrNewsItemsMinimum.Count(); i++)
            {
                var newsItemMinimum = arrNewsItemsMinimum[i];
                var newsItemMaximum = arrNewsItemsMaximum[i];
                newsItemMinimum.AddRange(newsItemMaximum);
                newsPages.Add(new NewsPage
                {
                    newsItems = newsItemMinimum
                });
            }
            if(!isEqualItems)
            {
                var itemsToSkip = arrNewsItemsMinimum.Count() * itemPerPage;
                var remainingNewsItems = newsItems.Skip(itemsToSkip).ToList();
                var remainingNewsPages = GetNewsItemsPages(remainingNewsItems, maxItemPerPage);
                newsPages.AddRange(remainingNewsPages);
            }
            
            return newsPages;
        }

        public List<NewsPage> GetNewsItemsPages(List<NewsItem> newsItems, int maxItemPerPage)
        {
            var newsPages = new List<NewsPage>();
            var lstNewsItems = GroupNewsItems(newsItems, maxItemPerPage);

            foreach (var item in lstNewsItems)
            {
                newsPages.Add(new NewsPage
                {
                    newsItems = item
                });
            }

            return newsPages;
        }

        public List<List<NewsItem>> GroupNewsItems(List<NewsItem> newsItems, int nSize)  
        {        
            var list = new List<List<NewsItem>>(); 
            for (int i = 0; i < newsItems.Count; i += nSize) 
            { 
                list.Add(newsItems.GetRange(i, Math.Min(nSize, newsItems.Count - i))); 
            } 

            return list; 
        }

        public void RegisterNewsAgencyForGoogle(NewsAgency newsAgency)
        {
            _googleNewsRepository.RegisterNewsAgency(newsAgency);
        }

        public void RegisterNewsAgencyForPressTrustOfIndia(NewsAgency newsAgency)
        {
            _pressTrustOfIndiaNewsRepository.RegisterNewsAgency(newsAgency);
        }

        public void RegisterNewsAgencyForInternal(NewsAgency newsAgency)
        {
            _internalNewsRepository.RegisterNewsAgency(newsAgency);
        } 
   }
}