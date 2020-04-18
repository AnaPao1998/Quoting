using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotingAPI.DTOModels
{
    public class QuoteProductsDTO
    {
        public string ProductCode { get; set; }
        public string ClientCode { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public bool IsSell { get; set; }
    }
}
