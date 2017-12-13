using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.ViewModels;
using DutchTreat.Services;
using DutchTreat.Data;
using Microsoft.AspNetCore.Authorization;

namespace DutchTreat.Controllers
{
	public class AppController : Controller
	{

		private readonly IMailService _mailService;
		private readonly IDutchRepository _repository;

		public AppController(IMailService mailService, IDutchRepository repository)
		{
			_mailService = mailService;
			_repository = repository;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet("contact")]
		public IActionResult Contact()
		{
			return View();
		}

		[HttpPost("contact")]
		public IActionResult Contact(ContactViewModel model)
		{
			if (ModelState.IsValid)
			{
				//Send the email
				_mailService.SendMesage("jfrobishow@gmail.com", model.Subject, $"From {model.Name} {model.Email}, Message: {model.Message}");
				ViewBag.UserMessage = "Mail Sent";
				ModelState.Clear();
			}
			else
			{

			}
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		[Authorize]
		public IActionResult Shop()
		{
			/*
			var results = _context.Products
				.OrderBy(p => p.Category)
				.ToList();
				return View();
				*/

			/*
			var results = from p in _context.Products
										orderby p.Category
										select p;
			return View(results.ToList());
	    */

			var results = _repository.GetAllProducts();
			return View(results);
		}

	}
}
