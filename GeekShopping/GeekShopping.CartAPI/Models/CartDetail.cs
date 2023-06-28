using GeekShopping.CartAPI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.CartAPI.Models
{
    [Table("cart_detail")]
    public class CartDetail : BaseEntity
    {
        //[Column("cart_header_id")]
        public long CartHeaderId { get; set; }

        [ForeignKey("CartHeaderId")]
        public virtual CartHeader? CartHeader { get; set; }

        //[Column("product_id")]
        public long ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Column("count")]
        public int Count { get; set; }
    }
}
