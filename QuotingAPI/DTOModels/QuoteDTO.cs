using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotingAPI.DTOModels
{
    public class QuoteDTO
    {
        public string QuoteName { get; set; }
        public List <QuoteProductsDTO> QuoteLineItems { get; set; }
    }
}
