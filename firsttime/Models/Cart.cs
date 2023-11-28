using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace firsttime.Models
{
    public class Cart
    {
        private List<Product> cartItems;

        public Cart() {
            cartItems = new List<Product>();
        }

        public void AddToCart(Product product)
        {
            cartItems.Add(product);
        }
        public void RemoveFromCart(Product product)
        {
            cartItems.Remove(product);
        }

        public List<Product> GetCartItems()
        {
            return cartItems;
        }

        public void ClearCart()
        {
            cartItems.Clear();
        }

        public decimal CalculateTotalPrice()
        {
            decimal totalPrice = 0;
            foreach (var product in cartItems)
            {
                totalPrice += product.Price;
            }
            return totalPrice;
        }
    }
}
