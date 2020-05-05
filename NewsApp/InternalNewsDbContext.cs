using System;
using NewsApp.Models;
using Microsoft.EntityFrameworkCore;

public class InternalNewsDbContext:DbContext  
{  
     public InternalNewsDbContext(DbContextOptions<InternalNewsDbContext> context):base(context)  
    {         
    } 
    public DbSet<NewsAgency> NewsAgencies { get; set; }
    public DbSet<NewsItem> NewsItems { get; set; } 
}