namespace Inventory
{
    class Program
    {
        static void Main(string[] args)
        {
            var inventory = new Inventory();
            int option;

            do
            {
                Console.WriteLine("\nPlease Enter what you need to do:\n 1. Add a product. \n 2. View all products. \n 3. Edit a product. \n 4. Delete a product. \n 5. Search for a product. \n 6. Exit.");
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
                        string productNameToEdit = Console.ReadLine();
                        inventory.EditProduct(productNameToEdit);
                        break;
                    case 4:
                        Console.Write("Enter product name to delete: ");
                        string productNameToDelete = Console.ReadLine();
                        inventory.DeleteProduct(productNameToDelete);
                        break;
                    case 5:
                        Console.Write("Enter product name to search: ");
                        string productNameToSearch = Console.ReadLine();
                        inventory.SearchProduct(productNameToSearch);
                        break;
                    case 6:
                        Console.WriteLine("Exiting the program...");
                        break;
                    default:
                        Console.WriteLine("Invalid otion. Please enter a valid option.");
                        break;
                }

            } while (option != 6);
        }
 
    }
}
