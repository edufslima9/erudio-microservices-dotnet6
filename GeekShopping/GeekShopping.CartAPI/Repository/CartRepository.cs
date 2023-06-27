using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Models;
using GeekShopping.CartAPI.Models.Context;
using GeekShopping.CartAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly PostgreSQLContext _context;
        private IMapper _mapper;

        public CartRepository(PostgreSQLContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<bool> ApplyCoupom(string userId, string coupomCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ClearCart(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<CartVO> FindCartByUserId(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveCoupom(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFromCart(long cartDetailsId)
        {
            throw new NotImplementedException();
        }

        public async Task<CartVO> SaveOrUpdateCart(CartVO cartVO)
        {
            throw new NotImplementedException();
        }
    }
}
