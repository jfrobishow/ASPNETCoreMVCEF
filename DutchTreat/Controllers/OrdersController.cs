using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
	[Route("api/[Controller]")]
  public class OrdersController : Controller
	{

		private readonly IDutchRepository _repository;
		private readonly ILogger<ProductsController> _logger;
		private readonly IMapper _mapper;

		public OrdersController(IDutchRepository repository, ILogger<ProductsController> logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult Get(bool includeItems = true)
		{
			try
			{
				var results = _repository.GetAllOrders(includeItems);
				return Ok(_mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(results));
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get orders {ex}");
				return BadRequest("Failed to get orders ");
			}
		}

		[HttpGet("{id:int}")]
		public IActionResult Get(int id)
		{
			try
			{
				var order = _repository.GetOrderById(id);
				if (order != null)
				{
					return Ok(_mapper.Map<Order, OrderViewModel>(order));
				}
				else
				{
					return NotFound();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to get orders {ex}");
				return BadRequest("Failed to get orders ");
			}
		}


		[HttpPost]
		public IActionResult Post([FromBody]OrderViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var newOrder = _mapper.Map<OrderViewModel, Order>(model);

					if(newOrder.OrderDate == DateTime.MinValue)
					{
						newOrder.OrderDate = DateTime.Now;
					}

					_repository.AddEntity(newOrder);
					if (_repository.SaveAll())
					{
						var vm = _mapper.Map<Order, OrderViewModel>(newOrder);

						return Created($"/api/orders/{vm.OrderId}", vm);
					}
				}
				else
				{
					return BadRequest(ModelState);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"Failed to save a new order: {ex}");

			}

			return BadRequest("Failed to save new order");
		}

	}
}
