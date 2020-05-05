using System;
using NewsApp.Models;
using Microsoft.EntityFrameworkCore;

public class GoogleNewsDbContext:DbContext  
{  
     public GoogleNewsDbContext(DbContextOptions<GoogleNewsDbContext> context):base(context)  
    {         
    } 
    public DbSet<NewsAgency> NewsAgencies { get; set; }
    public DbSet<NewsItem> NewsItems { get; set; } 
}