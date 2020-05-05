using System;
using System.Linq;
using System.Collections.Generic;
using NewsApp.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Repository
{
    public class InternalNewsRepository : INewsRepository
    {
        private readonly InternalNewsDbContext _context;

        public InternalNewsRepository(InternalNewsDbContext context)
        {
            _context = context;
        }

        public async Task<List<NewsItem>> GetNewsContents(string category)
        {
            var newsItems = await _context.NewsItems.Where(n => n.Category == category || n.Category == "Advertisement")
                            .ToListAsync();
            return newsItems;
        }

        public void RegisterNewsAgency(NewsAgency newsAgency)
        {
            try
            {
                _context.NewsAgencies.Add(newsAgency);
                _context.SaveChanges();                   
            }
            catch (System.Exception)
            {                
                throw;
            }
        }
    }
}