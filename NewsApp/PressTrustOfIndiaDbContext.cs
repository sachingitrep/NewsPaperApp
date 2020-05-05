using System;
using NewsApp.Models;
using Microsoft.EntityFrameworkCore;

public class PressTrustOfIndiaDbContext:DbContext  
{  
     public PressTrustOfIndiaDbContext(DbContextOptions<PressTrustOfIndiaDbContext> context):base(context)  
    {         
    } 
    public DbSet<NewsAgency> NewsAgencies { get; set; }
    public DbSet<NewsItem> NewsItems { get; set; } 
}