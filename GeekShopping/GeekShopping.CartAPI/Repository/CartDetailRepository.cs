using AutoMapper;
using GeekShopping.CartAPI.Models.Context;
using GeekShopping.CartAPI.Repository.Interface;

namespace GeekShopping.CartAPI.Repository
{
    public class CartDetailRepository : ICartDetailRepository
    {
        private readonly PostgreSQLContext _context;
        private IMapper _mapper;

        public CartDetailRepository(PostgreSQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
