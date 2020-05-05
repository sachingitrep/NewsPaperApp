using System;
using System.Collections.Generic;

namespace NewsApp.Models
{
    public enum AgencyType
    {     
        External,
        Internal
    }
    public class NewsAgency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set;}
        public AgencyType Type { get; set; }  
        public bool IsActive { get; set;}      
   }
}