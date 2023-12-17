using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagmentSyetemV2
{
    internal class Inventory
    {
        private List<Product> products;

        public Inventory()
        {
            products = new List<Product>();
        }

        public void AddProduct(string name, double price, int quantity)
        {
            Product newProduct = new Product(name, price, quantity);
            products.Add(newProduct);
            Console.WriteLine("Product added to inventory.");
        }

        public void ViewAllProducts()
        {
            if (products.Count == 0)
            {
                Console.WriteLine("Inventory is empty.");
            }
            else
            {
                Console.WriteLine("Inventory:");
                products.ForEach(p => Console.WriteLine(p));

            }
        }
    }
}
