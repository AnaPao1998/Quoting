using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotingAPI.Database.Models
{
    public class Quote
    {
        public string QuoteCode { get; set; }
        public string ClientCode { get; set; }
        public float Price { get; set; }
        public bool Sold { get; set; }
        public List <QuoteProducts> QuoteProducts { get; set; }

    }
}
