using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCart.Models
{
    public class CartProduct
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
