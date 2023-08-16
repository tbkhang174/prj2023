using MySqlConnector;
using Persistence;
using DAL;

namespace DAL
{
    public static class OrderFilter
    {
        public const int PENDING_ORDERS = 1;
        public const int CONFIRMED_ORDER = 2;
        public const int COMPLETED_ORDERS = 3;
    }
    public class OrderDAL
    {
        private string query = "";
        private MySqlConnection connection = DbConfig.GetConnection();
        public Order GetOrder(MySqlDataReader reader)
        {
            Order order = new Order();
            order.OrderID = reader.GetInt32("order_id");
            order.Manga = new List<Manga>();
            order.Order_PaymentMethod = reader.GetString("payment_method");
            order.StaffID = reader.GetInt32("create_by");
            order.CreateBy = new Staff();
            order.CreateAt = reader.GetDateTime("create_at");
            order.Status = (reader.GetInt32("order_status") == 0) ? "Pending" : (reader.GetInt32("order_status") == 1) ? "Confirmed" : "Completed";
            return order;
        }

        public bool UpdateOrder(int orderID, int orderFilter) {
            try
            {
                switch (orderFilter)
                {
                    case OrderFilter.CONFIRMED_ORDER:
                        query = $@"UPDATE orders SET order_status = 1 where order_id = '{orderID}';";
                        break;
                    case OrderFilter.COMPLETED_ORDERS:
                        query = $@"UPDATE orders SET order_status = 2 where order_id = '{orderID}';";
                        break;
                }
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
           
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("ex: " + ex.Message);
                return false;
            }
            return true;
        }


        public bool CreateOrder(Order order)
        {
            bool result = false;
            try
            {
                using (MySqlTransaction trans = connection.BeginTransaction())
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.Connection = connection;
                    cmd.Transaction = trans;
                    //Lock update all tables
                    cmd.CommandText = "lock tables Manga write, Orders write, OrderDetails write;";
                    cmd.ExecuteNonQuery();
                    MySqlDataReader? reader = null;
                    try
                    {
                        //Insert Order
                        cmd.CommandText = "insert into Orders(create_by) values (@createby);";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@createby", order.CreateBy.StaffID);
                        cmd.ExecuteNonQuery();
                        //get new Order_ID
                        cmd.CommandText = "select LAST_INSERT_ID() as order_id";
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            order.OrderID = reader.GetInt32("order_id");
                        }
                        reader.Close();
                        //insert Order Details table
                        foreach (Manga item in order.Manga)
                        {
                            if (item.MangaID == 0 || item.Quantity <= 0)
                            {
                                throw new Exception("Not Exists Item");
                            }
                            //insert to Order Details
                            cmd.CommandText = @"insert into orderdetails(order_id, manga_id, manga_quantity) values 
                            (" + order.OrderID + ", " + item.MangaID + ", " + item.Quantity + ");";
                            cmd.ExecuteNonQuery();
                        }
                        //commit transaction
                        trans.Commit();
                        result = true;
                    }
                    catch
                    {
                        try
                        {
                            trans.Rollback();
                        }
                        catch { }
                    }
                    finally
                    {
                        //unlock all tables;
                        cmd.CommandText = "unlock tables;";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            return result;
        }

        // internal Order GetOrder(MySqlDataReader reader)
        // {
        //     Order order = new Order();
        //     order.OrderID = reader.GetInt32("order_id");
        //     order.Order_Create_by = reader.GetString("create_by");
        //     order.Order_PaymentMethod = reader.GetString("payment_method");
        //     return order;
        // }

        public Order GetOrderByID(int orderID) {
            Order order = new Order();
            try
            {
                query = $@"select * from orders o
                inner join staffs s on s.staff_id = o.create_by
                 where o.order_id = '{orderID}';";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    order = GetOrder(reader);
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("ex: " + ex.Message);
            }
            order.Manga = GetMangaByOrderID(orderID);
            order.CreateBy = new StaffDAL().GetStaffByID(order.StaffID);
            foreach (var item in order.Manga)
            {
                item.Type = new MangaDAL().GetTypeByID(item.TypeID);
            }
            return order;
        }

        public List<Manga> GetMangaByOrderID(int orderID) {
            MangaDAL cDAL = new MangaDAL();
            List<Manga> mangas = new List<Manga>();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                command.CommandText = $@"select * from orders O
                                        INNER JOIN orderdetails OD ON O.order_id = OD.order_id
                                        INNER JOIN manga C ON OD.manga_id = C.manga_id
                                        WHERE O.order_id = @orderid;";
                command.Parameters.AddWithValue("@orderid", orderID);
                MySqlDataReader reader = command.ExecuteReader();
                while(reader.Read()) {
                    mangas.Add(cDAL.GetManga(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return mangas;
        }

        public List<Order> GetOrders(int orderFilter)
        {
            List<Order> orders = new List<Order>();
            try
            {
                switch (orderFilter)
                {
                    case OrderFilter.PENDING_ORDERS:
                        query = @"select * from orders where order_status = 0;";
                        break;
                    case OrderFilter.CONFIRMED_ORDER:
                        query = @"select * from orders where order_status = 1;";
                        break;
                    case OrderFilter.COMPLETED_ORDERS:
                        query = @"select * from orders where order_status = 2;";
                        break;
                }
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    orders.Add(GetOrder(reader));
                }
                reader.Close();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("ex: " + ex.Message);
            }
            List<Order> output = new List<Order>();
            foreach(var order in orders){
                output.Add(GetOrderByID(order.OrderID));
            }
            return output;
        }

        public List<Manga> GetMangasInOrder(int orderID)
        {

            return new List<Manga>();
        }

        // internal Persistence.Type GetOrderType(MySqlDataReader reader)
        // {
        //     Persistence.Type ordertype = new Persistence.Type();
        //     ordertype.TypeID = reader.GetInt32("type_id");
        //     ordertype.MangaType = reader.GetString("type_name");
        //     return ordertype;
        // }

        // public Persistence.Type GetOrderTypeByID(int OrdertypeID)
        // {
        //     Persistence.Type type = new Persistence.Type();
        //     try
        //     {
        //         MySqlCommand command = new MySqlCommand("", connection);
        //         query = "SELECT * FROM OrderType WHERE ordertype_id = @ordertypeid;";
        //         command.CommandText = query;
        //         command.Parameters.AddWithValue("@ordertypeid", OrdertypeID);
        //         MySqlDataReader reader = command.ExecuteReader();
        //         if (reader.Read())
        //         {
        //             type = GetOrderType(reader);
        //         }
        //         reader.Close();
        //     }
        //     catch { }
        //     return type;
        // }

        // public List<Order> GetAllOrder()
        // {
        //     List<Order> Orders = new List<Order>();
        //     try
        //     {
        //         MySqlCommand command = new MySqlCommand("", connection);
        //         query = "SELECT * FROM Orders;";
        //         command.CommandText = query;
        //         MySqlDataReader reader = command.ExecuteReader();
        //         while (reader.Read())
        //         {
        //             Orders.Add(GetOrder(reader));
        //         }
        //         reader.Close();
        //     }
        //     catch { }
        //     foreach (var item in Orders)
        //     {
        //         item.Typea = GetOrderTypeByID(item.OrderTypeID);
        //     }
        //     return Orders;
        // }

        // public Order GetOrderByID(int OrderID)
        // {
        //     Order order = new Order();
        //     try
        //     {
        //         MySqlCommand command = new MySqlCommand("", connection);
        //         query = "SELECT * FROM Orders WHERE order_id = @orderID;";
        //         command.CommandText = query;
        //         command.Parameters.AddWithValue("@orderID", OrderID);
        //         MySqlDataReader reader = command.ExecuteReader();
        //         if (reader.Read())
        //         {
        //             order = GetOrder(reader);
        //         }
        //         reader.Close();
        //     }
        //     catch { }            
            
        //     return order;
        // }

        
    }
}
