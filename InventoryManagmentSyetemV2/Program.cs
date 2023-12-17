using InventoryManagmentSyetemV2;

namespace YarasInventoryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Inventory inventory = new Inventory();

            Console.WriteLine("Please Add a product");

            Console.Write("Enter product name: ");
            string name = Console.ReadLine();

            Console.Write("Enter price: ");

            double price = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter quantity: ");

            int quantity = Convert.ToInt32(Console.ReadLine());

            inventory.AddProduct(name, price, quantity);
            
        }
    }
}
