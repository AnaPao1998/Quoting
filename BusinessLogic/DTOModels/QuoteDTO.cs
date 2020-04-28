using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotingAPI.DTOModels
{
    public class QuoteDTO
    {
        public string QuoteID { get; set; }
        public string QuoteName { get; set; }
        public string ClientCode { get; set; }
        public List<QuoteProductsDTO> QuoteLineItems { get; set; }
        public bool IsSell { get; set; }
    }
}
