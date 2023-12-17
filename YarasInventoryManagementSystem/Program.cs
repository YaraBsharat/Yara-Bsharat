namespace YarasInventoryManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Inventory inventory = new Inventory();
            int choice;

            do
            {
                Console.WriteLine("\nMenu:\n1. Add a product\n2. View all products\n3. Edit a product\n4. Delete a product\n5. Search for a product\n6. Exit\nEnter your choice: \n");
                choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("Enter product name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter price: ");
                        double price = Convert.ToDouble(Console.ReadLine());
                        Console.Write("Enter quantity: ");
                        int quantity = Convert.ToInt32(Console.ReadLine());
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
                    case 4:
                        Console.Write("Enter product name to delete: ");
                        string deleteName = Console.ReadLine();
                        inventory.DeleteProduct(deleteName);
                        break;
                    case 5:
                        Console.Write("Enter product name to search: ");
                        string searchName = Console.ReadLine();
                        inventory.SearchProduct(searchName);
                        break;
                    case 6:
                        Console.WriteLine("Exiting the program...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            } while (choice != 6);
        }
    }
}
