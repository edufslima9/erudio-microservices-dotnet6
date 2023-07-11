using GeekShopping.Email.Messages;
using GeekShopping.Email.Models;

namespace GeekShopping.Email.Repository.Interface
{
    public interface IEmailRepository
    {
        Task LogEmail(UpdatePaymentResultMessage resultMessage);
    }
}
