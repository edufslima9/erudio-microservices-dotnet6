using GeekShopping.OrderAPI.Models;

namespace GeekShopping.OrderAPI.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader header);
        Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid);
    }
}
