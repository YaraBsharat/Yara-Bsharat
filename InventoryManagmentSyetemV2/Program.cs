namespace InventoryManagmentSyetemV2
{
    class Program
    {
        static void Main(string[] args)
        {
            Inventory inventory = new Inventory();
            int option;

            do
            {
                Console.WriteLine("\nPlease Enter what you need to do:\n 1. Add a product. \n 2. View all products \n 3. Edit a product");
                Console.WriteLine("\nPlease Enter 0 To close");
                option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.Write("Enter product name: ");
                        var name = Console.ReadLine();

                        Console.Write("Enter price: ");

                        var price = Convert.ToDouble(Console.ReadLine());
                        
                        Console.Write("Enter quantity: ");

                        var quantity = Convert.ToInt32(Console.ReadLine());

                        inventory.AddProduct(name, price, quantity);
                        break;
                    case 2:
                        inventory.ViewAllProducts();
                        break;
                    case 3:
                        Console.Write("Enter product name to edit: ");
                        string editName = Console.ReadLine();
                        inventory.EditProduct(editName);
                        break;
                }

            } while (option != 0);
            
        }
    }
}
