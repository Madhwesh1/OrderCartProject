using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCart.Models
{
    public class Cart
    {
        [Key]
        public int cartId { get; set; }
        public List<CartProduct> Product { get; set; }
        public decimal price { get; set; }
        public string orderStatus { get; set; }
    }
}
