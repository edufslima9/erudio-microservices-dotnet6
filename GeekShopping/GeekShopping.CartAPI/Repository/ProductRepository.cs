using AutoMapper;
using GeekShopping.CartAPI.Models.Context;
using GeekShopping.CartAPI.Repository.Interface;

namespace GeekShopping.CartAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly PostgreSQLContext _context;
        private IMapper _mapper;

        public ProductRepository(PostgreSQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
