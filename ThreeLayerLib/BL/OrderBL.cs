using Persistence;
using DAL;

namespace BL
{
    public class OrderBL
    {
        private OrderDAL oDAL = new OrderDAL();
        public bool CreateOrder(Order order) {
            return oDAL.CreateOrder(order);
        }

        public List<Order> GetPendingOrder() {
            return oDAL.GetOrders(OrderFilter.PENDING_ORDERS);
        }
        public List<Order> GetComfirmedOrder() {
            return oDAL.GetOrders(OrderFilter.CONFIRMED_ORDER);
        }
        public List<Order> GetCompletedOrder() {
            return oDAL.GetOrders(OrderFilter.COMPLETED_ORDERS);
        }
        public bool ConfirmOrder(int orderID) {
            return oDAL.UpdateOrder(orderID, OrderFilter.CONFIRMED_ORDER);
        }
        public bool HandleOrder(int orderID) {
            return oDAL.UpdateOrder(orderID, OrderFilter.COMPLETED_ORDERS);
        }
        public Order GetOrderByID(int orderID) {
            return oDAL.GetOrderByID(orderID);
        }

        // internal List<Order> CreateOrder()
        // {
        //     throw new NotImplementedException();
        // }

        // private OrderDAL cDAL = new OrderDAL();
        // public List<Order> GetAllOrder() {
        //     return oDAL.GetAllOrder();
        // }
        // public Order GetOrderByID(int OrderID) {
        //     return oDAL.GetOrderByID(OrderID);
        // }
    }
}