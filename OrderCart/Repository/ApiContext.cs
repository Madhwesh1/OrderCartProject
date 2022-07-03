using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace OrderCart.Repository
{
    public class ApiContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "CartItemsDB");
        }
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
            
        }


        public DbSet<Models.CartProduct> CartProjObject { get; set; }        
        public DbSet<Models.Cart> CartObject { get; set; }
    }
}
