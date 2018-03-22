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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace DutchTreat.Controllers
{
	[Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                var username = User.Identity.Name;

				var results = _repository.GetAllOrders(username, includeItems);
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
				var order = _repository.GetOrderById(User.Identity.Name, id);
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

                        //Created is a 201 http
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
