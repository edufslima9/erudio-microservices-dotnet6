using GeekShopping.CartAPI.Data.ValueObjects;

namespace GeekShopping.CartAPI.Repository.Interface
{
    public interface ICartRepository
    {
        Task<CartVO> FindCartByUserId(string userId);
        Task<CartVO> SaveOrUpdateCart(CartVO cart);
        Task<bool> RemoveFromCart(long cartDetailsId);
        Task<bool> ApplyCoupom(string userId, string coupomCode);
        Task<bool> RemoveCoupom(string userId);
        Task<bool> ClearCart(string userId);
    }
}
