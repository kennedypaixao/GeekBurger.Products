using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.Products.Contract
{
    public class Product
    {
        public string StoreName { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<Item> Items { get; set; }
        public decimal Price { get; set; }
    }
    public class ProductChanged
    {
        public ProductState State { get; set; }
        public Product Product { get; set; }
    }

    public enum ProductState
    {
        Deleted = 2,
        Modified = 3,
        Added = 4
    }


}
