using GeekShopping.Email.Models.Context;
using GeekShopping.Email.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<PostgreSQLContext> _context;

        public OrderRepository(DbContextOptions<PostgreSQLContext> context)
        {
            _context = context;
        }

        public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool paid)
        {
            //await using var _db = new PostgreSQLContext(_context);
            //var header = await _db.OrderHeaders.FirstOrDefaultAsync(x => x.Id == orderHeaderId);
            //if (header != null)
            //{
            //    header.PaymentStatus = paid;
            //    _db.OrderHeaders.Update(header);
            //    await _db.SaveChangesAsync();
            //}
        }
    }
}
