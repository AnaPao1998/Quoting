using System.Collections.Generic;

namespace QuotingAPI.Database.Models
{
    public class DBContext
    {
        public List<Quote> Quote { get; set; }
        public List<QuoteProducts> QuoteProducts { get; set; }
    }
}
