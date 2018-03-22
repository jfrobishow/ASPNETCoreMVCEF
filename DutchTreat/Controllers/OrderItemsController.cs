using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
	[Route("api/orders/{orderid}/items")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderItemsController : Controller
	{
		private readonly IDutchRepository _repository;
		private readonly ILogger<OrderItemsController> _logger;
		private readonly IMapper _mapper;

		public OrderItemsController(IDutchRepository repository, ILogger<OrderItemsController> logger, IMapper mapper)
		{
			_repository = repository;
			_logger = logger;
			_mapper = mapper;
		}

		[HttpGet]
		public IActionResult Get(int orderId)
		{
			var order = _repository.GetOrderById(User.Identity.Name, orderId);
			if (order != null)
			{
				return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
			}
			else
			{
				return NotFound();
			}
		}

		[HttpGet("{index}")]
		public IActionResult Get(int orderId, int index)
		{
			var order = _repository.GetOrderById(User.Identity.Name, orderId);
			if (order != null)
			{
				if(index > 0 && order.Items.Count > index -1)
				{
					var item = order.Items.ElementAt(index - 1);
					return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(item));
				}
				else
				{
					return NotFound();
				}
			}
			else
			{
				return NotFound();
			}
		}

        //Implement post, put, delete to manage an order, etc.
	}
}
