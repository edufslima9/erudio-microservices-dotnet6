using AutoMapper;
using GeekShopping.CartAPI.Models.Context;
using GeekShopping.CartAPI.Repository.Interface;

namespace GeekShopping.CartAPI.Repository
{
    public class CartHeaderRepository : ICartHeaderRepository
    {
        private readonly PostgreSQLContext _context;
        private IMapper _mapper;

        public CartHeaderRepository(PostgreSQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
