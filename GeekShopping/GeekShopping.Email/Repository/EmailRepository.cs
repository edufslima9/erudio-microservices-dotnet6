using GeekShopping.Email.Messages;
using GeekShopping.Email.Models;
using GeekShopping.Email.Models.Context;
using GeekShopping.Email.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<PostgreSQLContext> _context;

        public EmailRepository(DbContextOptions<PostgreSQLContext> context)
        {
            _context = context;
        }

        public async Task LogEmail(UpdatePaymentResultMessage resultMessage)
        {
            EmailLog emailLog = new()
            {
                Email = resultMessage.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {resultMessage.OrderId} has beem created successfully!"
            };

            await using var _db = new PostgreSQLContext(_context);
            _db.EmailLogs.Add(emailLog);
            await _db.SaveChangesAsync();
        }
    }
}
