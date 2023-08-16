using Persistence;
namespace UI
{
    public class Ultilities
    {
        public int MenuHandle(string? title, string[] menuItem)
        {
            int i = 0, choice;
            if (title != null)
                Title(title);
            for (; i < menuItem.Count(); i++)
            {
                System.Console.WriteLine("" + (i + 1) + ". " + menuItem[i]);
            }
            Line();
            do
            {
                System.Console.Write("Your choice: ");
                int.TryParse(System.Console.ReadLine(), out choice);
            } while (choice <= 0 || choice > menuItem.Count());
            return choice;
        }
        public void ShowListManga(List<Manga> Mangas)
        {
            Console.WriteLine("───────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
            Console.WriteLine("     Create Order                                                                                                  ");
            Console.WriteLine("───────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
            Console.WriteLine("| {0, 5} | {1, 35} | {2, 20} | {3, 20} | {4, 20} |", "ID", "Manga Name", "Manga Type", "Quantity", "Price");
            Console.WriteLine("───────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
            foreach (Manga item in Mangas)
            {
                Console.WriteLine("| {0, 5} | {1, 35} | {2, 20} | {3, 20} | {4, 20} |", item.MangaID, item.MangaName, item.Type.MangaType, item.Quantity, item.Price);
            }
            Console.WriteLine("───────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
        }
        public void Title(string title)
        {
            Line();
            Console.WriteLine();
            System.Console.WriteLine(" " + title);
            Console.WriteLine();
            Line();
        }
        public void Line()
        {
            System.Console.WriteLine("────────────────────────────────────────────────");
        }
        public void PressAnyKeyToContinue()
        {
            Console.Write("Press any key to continue.");
            Console.ReadKey();
        }

        // public void ShowListOrder(List<Order> Orders)
        // {
        //     Console.WriteLine("───────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
        //     Console.WriteLine("     List Order                                                                                                  ");
        //     Console.WriteLine("───────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
        //     Console.WriteLine("| {0, 5} | {1, 35} | {2, 20} | {3, 20} | {4, 20} |", "ID", "Create Time", "Create By", "Quantity", "Payment Method");
        //     Console.WriteLine("───────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
        //     foreach (Order item in Orders)
        //     {
        //         Console.WriteLine("| {0, 5} | {1, 35} | {2, 20} | {3, 20} | {4, 20} |", item.OrderID, item.CreateAt, item.CreateBy, item.Order_Quantity, item.Order_PaymentMethod);
        //     }
        //     Console.WriteLine("───────────────────────────────────────────────────────────────────────────────────────────────────────────────────");
        // }

        public void PrintListOrder(List<Order> orders) {
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────");
            Console.WriteLine("| {0, 15} | {1, 30} | {2, 17} | {3, 15} |", "Order ID", "Create At", "Create By", "Status");
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────");
            foreach (var item in orders)
            {
                 Console.WriteLine("| {0, 15} | {1, 30} | {2, 17} | {3, 15} |", item.OrderID, item.CreateAt, item.CreateBy.StaffName, item.Status); 
                 Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────");
            }
        }

        public void PrintOrder(Order order) {
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────");
            Console.WriteLine("           Manga Store                                                                                    ");
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────");
            Console.WriteLine(" Order Staff: " + order.CreateBy.StaffName + " - ID: " + order.CreateBy.StaffID);
            Console.WriteLine(" Order Create At: " + order.CreateAt);
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────");
            Console.WriteLine("| {0, 15} | {1, 30} | {2, 15} | {3, 15} | {4, 15} |", "Manga ID", "Name", "Quantity", "Manga Type", "Price");
            Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────");
            foreach (var item in order.Manga)
            {
                 Console.WriteLine("| {0, 15} | {1, 30} | {2, 15} | {3, 15} | {4, 15} |", item.MangaID, item.MangaName, item.Quantity, item.Type.MangaType, item.Price); 
                 Console.WriteLine("──────────────────────────────────────────────────────────────────────────────────────────────────────────");
            }
            Console.WriteLine();
            Console.WriteLine("                                              Thank you!");
            Console.ResetColor();
        }

    }
}

