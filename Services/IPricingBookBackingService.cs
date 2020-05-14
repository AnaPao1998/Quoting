using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IPricingBookBackingService
    {
        public Task<List<ProductBsDTO>> GetAllProduct();
    }
}
