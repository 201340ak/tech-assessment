using System;
using System.Collections.Generic;
using System.Linq;
using CSharp.Contracts;

namespace CSharp.Managers
{
    [Obsolete("This is a temporary Order Manager that serves up dummy data to test Web Api Controllers with.")]
    public class TestOrderManager : IOrderManager
    {
        public List<Order> GetOrders()
        {
            return TestOrderData;
        }

        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            // TODO: For implementation - Get data from accessor
            return TestOrderData.Where(order => order.Customer.Id == customerId).ToList();
        }

        public Order UpdateOrder(int orderId, Order order)
        {
            // TODO: For implementation - Pass through to an engine/accessor to update/set
            var orderToUpdate = TestOrderData.SingleOrDefault(testOrder => testOrder.Id == orderId);
            orderToUpdate = order;

            return orderToUpdate;
        }
        
        public Order CancelOrder(int orderId)
        {
            // TODO: For implementation - Pass through to Engine and Accessor Layers to update status
            var orderToUpdate = TestOrderData.SingleOrDefault(testOrder => testOrder.Id == orderId);
            orderToUpdate.Status = Status.Cancelled;

            return orderToUpdate;
        }

        public Order Create(Order order)
        {
            // TODO: For implementation - Pass through to Engine and Accessor layers to add to data source
            order.Status = Status.Recieved;
            order.Id = TestOrderData.Max(testOrder => testOrder.Id) + 1;
            return order;
        }

        private Customer Customer1 => new Customer
        {
            Id = 1,
            Name = "Customer One"
        };

        private Customer Customer2 => new Customer
        {
            Id = 2,
            Name = "Customer Two"
        };
        
        private List<Order> TestOrderData => new List<Order>
        {
            new Order
            {
                Id = 1,
                Customer = Customer1,
                Status = Status.Recieved
            },
            new Order
            {
                Id = 2,
                Customer = Customer1,
                Status = Status.Shipped
            },
            new Order
            {
                Id = 3,
                Customer = Customer1,
                Status = Status.Delivered
            },
            new Order
            {
                Id = 4,
                Customer = Customer1,
                Status = Status.Delivered
            },
            new Order
            {
                Id = 5,
                Customer = Customer1,
                Status = Status.Delivered
            },
            new Order
            {
                Id = 6,
                Customer = Customer2,
                Status = Status.Recieved
            },
            new Order
            {
                Id = 7,
                Customer = Customer2,
                Status = Status.Shipped
            },
            new Order
            {
                Id = 8,
                Customer = Customer2,
                Status = Status.Cancelled
            },
            new Order
            {
                Id = 9,
                Customer = Customer2,
                Status = Status.Cancelled
            },
            new Order
            {
                Id = 10,
                Customer = Customer2,
                Status = Status.Cancelled
            },
        };
    }
}