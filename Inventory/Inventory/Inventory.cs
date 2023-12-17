namespace Inventory
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

        public void EditProduct(string name)
        {
            Product productToEdit = products.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (productToEdit != null)
            {
                Console.WriteLine("Enter new name, price, and quantity for the product:");
                var newName = Console.ReadLine();
                var newPrice = Convert.ToDouble(Console.ReadLine());
                var newQuantity = Convert.ToInt32(Console.ReadLine());

                productToEdit.Name = newName;
                productToEdit.Price = newPrice;
                productToEdit.Quantity = newQuantity;

                Console.WriteLine("Product updated.");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }

        public void DeleteProduct(string name)
        {
            Product productToDelete = products.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (productToDelete != null)
            {
                products.Remove(productToDelete);
                Console.WriteLine("Product deleted from inventory.");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }
        public void SearchProduct(string name)
        {
            Product product = products.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (product != null)
            {
                Console.WriteLine(product);
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }
    }

}