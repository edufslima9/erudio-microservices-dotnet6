using GeekShopping.Email.Models;

namespace GeekShopping.Email.Repository.Interface
{
    public interface IOrderRepository
    {
        Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid);
    }
}
