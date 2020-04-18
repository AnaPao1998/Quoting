using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotingAPI.Database.Models
{
    public class Quote
    {
        public string QuoteName { get; set; }
        public List<QuoteProducts> QuoteLineItems { get; set; }

    }
}
