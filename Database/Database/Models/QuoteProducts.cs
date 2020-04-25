using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotingAPI.Database.Models
{
    public class QuoteProducts
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }

    }
}
