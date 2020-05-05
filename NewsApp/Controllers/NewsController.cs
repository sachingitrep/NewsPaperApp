using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsApp.Models;
using NewsApp.Services;

namespace NewsApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly ILogger<NewsController> _logger;
        private INewsService _newsService;
        public NewsController(ILogger<NewsController> logger, INewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult GetNewsPages(string category)
        {
            try
            {
                var newsPages = _newsService.GetNewsPages(category);
                return this.Ok(newsPages);
            }
            catch (System.Exception exception)
            {
                _logger.LogError(exception.Message);
                throw;
            }
            
        }
    }
}
