using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharp.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CSharp.Managers;

namespace CSharp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class OrderController : ControllerBase
	{
		public OrderController(IOrderManager orderManager)
		{
			OrderManager = orderManager;
		}

		private IOrderManager OrderManager { get; }

		[HttpPost]
		public ActionResult<Order> Create(Order order)
		{
			try
			{
				var createdOrder = OrderManager.Create(order);
				if(createdOrder == null)
				{
					return NotFound();
				}

				return Ok(createdOrder);
			}
			catch
			{
				// Log / Exception Handler
				return BadRequest();
			}
		}

		[HttpGet]
		[Route("GetAll")]
		public ActionResult<List<Order>> Get()
		{
			try
			{
				var orders = OrderManager.GetOrders();
				if(orders == null || !orders.Any())
				{
					return NotFound();
				}

				return Ok(orders);
			}
			catch
			{
				// Log / Exception Handler
				return BadRequest();
			}
		}
		
		[HttpGet]
		public ActionResult<List<Order>> GetByCustomerId(int customerId)
		{
			try
			{
				var orders = OrderManager.GetOrdersByCustomerId(customerId);
				if(orders == null || !orders.Any())
				{
					return NotFound();
				}

				return Ok(orders);
			}
			catch
			{
				// Log / Exception Handle
				return BadRequest();
			}
		}

		[HttpPut]
		public ActionResult<Order> UpdateOrder(int orderId, Order updatedOrder)
		{
			try
			{
				var order = OrderManager.UpdateOrder(orderId, updatedOrder);
				if(order == null)
				{
					return NotFound();
				}
				
				return Ok(order);
			}
			catch
			{
				// Log / Exception Handle
				return BadRequest();
			}
		}

		[HttpPut]
		public ActionResult<List<Order>> CancelOrder(int orderId)
		{
			try
			{
				var order = OrderManager.CancelOrder(orderId);
				if(order == null)
				{
					return NotFound();
				}
				
				return Ok(order);
			}
			catch
			{
				// Log / Exception Handle
				return BadRequest();
			}
		}
	}
}
