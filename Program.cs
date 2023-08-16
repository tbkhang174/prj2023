using BL;
using Persistence;
using DAL;
using UI;
using MySqlConnector;
class Program
{
    static void Main()
    {
        Ultilities UI = new Ultilities();
        MangaBL cmBL = new MangaBL ();
        OrderBL odBL = new OrderBL();
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        string[] cashierMenu = { "Create Order", "Log Out" };
        string[] sellerMenu = { "Confirm Orders", "Handle Orders", "Log Out" };
        string[] cashierSubMenu = { "Add Product To Order", "Show Order", "Back To Main Menu" };
        Staff? OrderStaff = null;
        Manga mangaInOrder = new Manga();
        List<Manga> comicInOrder = new List<Manga>();
        Order order = new Order();
        int MangaID = 0, quantity = 0, isAddMore = 0;
        string username;
        int orderID = 0, mangaID = 0, quantitya = 0, isAddMorea = 0;
        StaffBL uBL = new StaffBL();
        do
        {
            UI.Title(@" 
   __             _    
  / /  ___  ___ _(_)__ 
 / /__/ _ \/ _ `/ / _ \
/____/\___/\_, /_/_//_/
          /___/        
");
            Console.Write(" UserName: ");
            username = Console.ReadLine() ?? "";
            Console.Write(" Password: ");
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            Console.Clear();
            OrderStaff = uBL.Authorize(username, pass);
            if (OrderStaff != null)
            {
                int roleID = OrderStaff.Role_ID;

                if (roleID == 1)
                {
                    string[] unprocessedAction = { "Change Status To Processing", "Back To Previous Menu" };
                    string[] processingAction = { "Change Status To Completed", "Back To Previous Menu" };
                    List<int> listOrderID = new List<int>();
                    int isConfirm = 0;
                    bool active = true;
                    bool activeSub = true;
                    while (active)
                    {
                        List<Order> pendingOrders = new OrderBL().GetPendingOrder();
                        List<Order> confirmOrders = new OrderBL().GetComfirmedOrder();
                        int SellerChoice = UI.MenuHandle(@"
  _____         __   _        
 / ___/__ ____ / /  (_)__ ____
/ /__/ _ `(_-</ _ \/ / -_) __/
\___/\_,_/___/_//_/_/\__/_/ 
                      
", sellerMenu);
                        switch (SellerChoice)
                        {
                            case 1:
                            listOrderID = new List<int>();
                            if(pendingOrders.Count() == 0 || pendingOrders == null) {
                                Console.WriteLine("Dont Have Any Order To Confirm");
                                UI.PressAnyKeyToContinue();
                            break;
                            }
                            foreach (var item in pendingOrders)
                            {
                                listOrderID.Add(item.OrderID);
                            }
                             UI.PrintListOrder(pendingOrders);
                             do
                             {
                                Console.Write("Enter Order ID: ");
                                int.TryParse(Console.ReadLine(), out orderID);
                                if(listOrderID.IndexOf(orderID) == -1) {
                                    Console.WriteLine("Invalid Order ID Choice");
                                }
                             } while (listOrderID.IndexOf(orderID) == -1);
                             UI.PrintOrder(odBL.GetOrderByID(orderID));
                              isConfirm = 0;
                                do
                             {
                                Console.Write("Enter '1' To Handle Order Or '2' To Cancel Order: ");
                                int.TryParse(Console.ReadLine(), out isConfirm);
                                if(isConfirm <= 0 || isConfirm > 2) {
                                    Console.WriteLine("Invalid Choice");
                                }
                             } while (isConfirm <= 0 || isConfirm > 2);
                             if(isConfirm == 1) {
                                Console.WriteLine((odBL.ConfirmOrder(orderID)) ? "Handle order completed!" : "Handle order failed!");
                             } else {
                                Console.WriteLine("Cancel order completed!s");
                             }
                             Console.ReadLine();
                                break;
                            case 2:
                            if(confirmOrders.Count() == 0 || confirmOrders == null) {
                                Console.WriteLine("Dont Have Any Order To Handle");
                                UI.PressAnyKeyToContinue();
                            break;
                            }
                            listOrderID = new List<int>();
                            foreach (var item in confirmOrders)
                            {
                                listOrderID.Add(item.OrderID);
                            }
                             UI.PrintListOrder(confirmOrders);
                             do
                             {
                                Console.Write("Enter Order ID: ");
                                int.TryParse(Console.ReadLine(), out orderID);
                                if(listOrderID.IndexOf(orderID) == -1) {
                                    Console.WriteLine("Invalid Order ID Choice");
                                }
                             } while (listOrderID.IndexOf(orderID) == -1);
                             UI.PrintOrder(odBL.GetOrderByID(orderID));
                              isConfirm = 0;
                                do
                             {
                                Console.Write("Enter '1' To Handle Order Or '2' To Cancel Order: ");
                                int.TryParse(Console.ReadLine(), out isConfirm);
                                if(isConfirm <= 0 || isConfirm > 2) {
                                    Console.WriteLine("Invalid Choice");
                                }
                             } while (isConfirm <= 0 || isConfirm > 2);
                             if(isConfirm == 1) {
                                Console.WriteLine((odBL.HandleOrder(orderID)) ? "Handle order completed!" : "Handle order failed!");
                             } else {
                                Console.WriteLine("Cancel order completed!s");
                             }
                             Console.ReadLine();
                                break;
                            case 3:
                                active = false;
                                break;
                            default:
                                break;
                        }
                    }


                }
                else if (roleID == 2)
                {
                    bool active = true, activeSub = true;
                    string answer;
                    while (active)
                    {
                        int cashierChoice = UI.MenuHandle(@"        
    ___    ____       
  / __/__ / / /__ ____
 _\ \/ -_) / / -_) __/
/___/\__/_/_/\__/_/   
                                           
", cashierMenu);
                        switch (cashierChoice)
                        {
                            case 1:
                                do
                                {
                                    UI.ShowListManga(cmBL.GetAllManga());
                                    do
                                    {
                                        Console.Write("Choose Manga By ID: ");
                                        int.TryParse(Console.ReadLine(), out MangaID);
                                        if ((MangaID <= 0 || MangaID > cmBL.GetAllManga().Count()))
                                        {
                                            Console.WriteLine("Invalid ID");
                                            UI.PressAnyKeyToContinue();
                                            UI.ShowListManga(cmBL.GetAllManga());
                                        }
                                        else
                                        {
                                            mangaInOrder = cmBL.GetMangaByID(MangaID);
                                        }
                                    } while (MangaID <= 0 || MangaID > cmBL.GetAllManga().Count());
                                    do
                                    {
                                        Console.Write("Enter Quantity: ");
                                        int.TryParse(Console.ReadLine(), out quantity);
                                        if (quantity <= 0 || quantity > cmBL.GetMangaByID(MangaID).Quantity)
                                        {
                                            Console.WriteLine("Invalid Quantity!");
                                            UI.PressAnyKeyToContinue();
                                            UI.ShowListManga(cmBL.GetAllManga());
                                            Console.WriteLine("Manga ID: " + MangaID);
                                        }
                                        else
                                        {
                                            mangaInOrder.Quantity = quantity;
                                        }
                                    } while (quantity <= 0 || quantity > cmBL.GetMangaByID(MangaID).Quantity);
                                    do
                                    {
                                        Console.Write("Enter '1' To Add More Or '2' To Create Order");
                                        int.TryParse(Console.ReadLine(), out isAddMore);
                                        if (isAddMore <= 0 || isAddMore > 2)
                                        {
                                            Console.WriteLine("Invalid Choice!");
                                            UI.PressAnyKeyToContinue();
                                            UI.ShowListManga(cmBL.GetAllManga());
                                            Console.WriteLine("Manga ID: " + MangaID);
                                            Console.WriteLine("Quantity: " + quantity);
                                        }
                                        comicInOrder.Add(mangaInOrder);
                                    } while (isAddMore <= 0 || isAddMore > 2);
                                } while (isAddMore != 2);
                                order.Manga = comicInOrder;
                                order.CreateBy = OrderStaff;
                                Console.WriteLine("Order Staff: " + order.CreateBy.StaffName);
                                foreach (var item in order.Manga)
                                {
                                    Console.WriteLine("Manga Name: " + item.MangaName + " - Quantity: " + item.Quantity);
                                    Console.WriteLine(item.Quantity);
                                }
                                Console.WriteLine(odBL.CreateOrder(order) ? "Create Order Completed!" : "Create Order Failed!");
                                break;
                            case 2:
                                active = false;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid User Name Or Password!");
                UI.PressAnyKeyToContinue();
                Main();
            }
            break;
        } while (true);
    }
}
