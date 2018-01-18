using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
	public class AccountController : Controller
	{

		private readonly ILogger<AccountController> _logger;

		public AccountController(ILogger<AccountController> logger)
		{
			_logger = logger;
		}

		public IActionResult Login()
		{
			if (this.User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "App");
			}
			return View();
		}

	}
}
