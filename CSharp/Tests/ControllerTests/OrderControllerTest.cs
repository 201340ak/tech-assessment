using System;
using Xunit;
using CSharp.Controllers;
using System.Linq;
using CSharp.Contracts;
using CSharp.Managers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Moq;

namespace ControllerTests
{
    public class OrderControllerTest
    {
        [Fact]
        public void OrderController_GetOrders_ShouldReturnAllTenOrders()
        {
            // Arrange
            var orderManager = new Mock<IOrderManager>(MockBehavior.Strict);
            orderManager.Setup(manager => manager.GetOrders())
                .Returns(TestOrderData);

            var controller = new OrderController(orderManager.Object);
            // Act
            var orderResult = controller.Get();

            // Assert
            Assert.NotNull(orderResult);
            Assert.True(orderResult.Result is OkObjectResult);
            var orders  = (orderResult.Result as OkObjectResult).Value as List<Order>;
            Assert.NotNull(orders);
            Assert.Equal(10, orders.Count);
        }

        [Fact]
        public void OrderController_GetOrdersByCustomer_ShouldReturn5Orders()
        {
            // Arrange
            var orderManager = new Mock<IOrderManager>(MockBehavior.Strict);
            orderManager.Setup(manager => manager.GetOrdersByCustomerId(It.IsAny<int>()))
                .Returns(TestOrderData.Where(order => order.Customer.Id == 1).ToList());

            var controller = new OrderController(orderManager.Object);
            // Act
            var orderResult = controller.GetByCustomerId(1);

            // Assert
            Assert.NotNull(orderResult);
            Assert.True(orderResult.Result is OkObjectResult);
            var orders  = (orderResult.Result as OkObjectResult).Value as List<Order>;
            Assert.NotNull(orders);
            Assert.Equal(5, orders.Count);
            Assert.True(orders.All(order => order.Customer.Id == 1));
        }

        [Fact]
        public void OrderController_GetOrdersByCustomer_CustomerNotFound_ShouldReturnNotFound()
        {
            // Arrange
            var orderManager = new Mock<IOrderManager>(MockBehavior.Strict);
            orderManager.Setup(manager => manager.GetOrdersByCustomerId(It.IsAny<int>()))
                .Returns(null as List<Order>);

            var controller = new OrderController(orderManager.Object);
            // Act
            var orderResult = controller.GetByCustomerId(1);

            // Assert
            Assert.NotNull(orderResult);
            Assert.True(orderResult.Result is NotFoundResult);
        }

        [Fact]
        public void OrderController_UpdateOrder_ShouldReturnOkayAndUpdatedOrder()
        {
            // Arrange
            var orderManager = new Mock<IOrderManager>(MockBehavior.Strict);
            orderManager.Setup(manager => manager.UpdateOrder(It.IsAny<int>(), It.IsAny<Order>()))
                .Returns((int id, Order orderToUpdate) => orderToUpdate);
                
            var controller = new OrderController(orderManager.Object);
            // Act
            var updatedOrderResult = controller.UpdateOrder(1, new Order
            {
                Status = Status.Shipped
            });

            // Assert
            Assert.NotNull(updatedOrderResult);
            Assert.True(updatedOrderResult.Result is OkObjectResult);
            var updatedOrder  = (updatedOrderResult.Result as OkObjectResult).Value as Order;
            Assert.NotNull(updatedOrder);
            Assert.Equal(Status.Shipped, updatedOrder.Status);
        }

        [Fact]
        public void OrderController_UpdateOrder_OrderNotFound()
        {
            // Arrange
            var orderManager = new Mock<IOrderManager>(MockBehavior.Strict);
            orderManager.Setup(manager => manager.UpdateOrder(It.IsAny<int>(), It.IsAny<Order>()))
                .Returns(null as Order);

            var controller = new OrderController(orderManager.Object);
            // Act
            var updatedOrderResult = controller.UpdateOrder(1, new Order
            {
                Status = Status.Shipped
            });

            // Assert
            Assert.NotNull(updatedOrderResult);
            Assert.True(updatedOrderResult.Result is NotFoundResult);
        }

        [Fact]
        public void OrderController_UpdateOrder_CouldNotSuccessfullyUpdateOrder()
        {
            // Arrange
            var orderManager = new Mock<IOrderManager>(MockBehavior.Strict);
            orderManager.Setup(manager => manager.UpdateOrder(It.IsAny<int>(), It.IsAny<Order>()))
                .Throws(new Exception());

            var controller = new OrderController(orderManager.Object);
            // Act
            var updatedOrderResult = controller.UpdateOrder(1, new Order
            {
                Status = Status.Shipped
            });

            // Assert
            Assert.NotNull(updatedOrderResult);
            Assert.True(updatedOrderResult.Result is BadRequestResult);
        }

        [Fact]
        public void OrderController_CancelOrder_ShouldReturnOkResult()
        {
            // Arrange
            var orderManager = new Mock<IOrderManager>(MockBehavior.Strict);
            orderManager.Setup(manager => manager.CancelOrder(It.IsAny<int>()))
                .Returns((int id) => new Order{Status = Status.Cancelled, Id = id });

            var controller = new OrderController(orderManager.Object);
            // Act
            var updatedOrderResult = controller.CancelOrder(1);

            // Assert
            Assert.NotNull(updatedOrderResult);
            Assert.True(updatedOrderResult.Result is OkObjectResult);
            var updatedOrder  = (updatedOrderResult.Result as OkObjectResult).Value as Order;
            Assert.NotNull(updatedOrder);
            Assert.Equal(Status.Cancelled, updatedOrder.Status);
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
