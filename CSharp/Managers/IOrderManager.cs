using System.Collections.Generic;
using CSharp.Contracts;

namespace CSharp.Managers
{
    public interface IOrderManager
    {
        List<Order> GetOrders();

        List<Order> GetOrdersByCustomerId(int v);

        Order UpdateOrder(int orderId, Order order);
        
        Order CancelOrder(int orderId);
    }
}