using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsApp.Models;

namespace NewsApp.Services
{
    public interface INewsService
    {
        void RegisterNewsAgencyForGoogle(NewsAgency newsAgency);
        void RegisterNewsAgencyForPressTrustOfIndia(NewsAgency newsAgency);
        void RegisterNewsAgencyForInternal(NewsAgency newsAgency);
        Task<List<NewsPage>> GetNewsPages(string category);
    }
}