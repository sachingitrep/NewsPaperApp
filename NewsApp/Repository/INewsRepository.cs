using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsApp.Models;

namespace NewsApp.Repository
{
    public interface INewsRepository
    {
        Task<List<NewsItem>> GetNewsContents(string category);
        void RegisterNewsAgency(NewsAgency newsAgency);
    }
}