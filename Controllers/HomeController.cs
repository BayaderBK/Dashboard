using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Dashboard.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext context;

		public HomeController(ApplicationDbContext context)
		{
			this.context = context;
		}
		[Authorize]
		[HttpGet]
		public IActionResult Index()
		{
			var Name = HttpContext.User.Identity.Name;
			//CookieOptions options = new CookieOptions();
			//options.Expires = DateTime.Now.AddMinutes(10);
			//Response.Cookies.Append("Name", Name, options);
			//HttpContext.Session.SetString("Name", Name);
			TempData["Name"] = Name;
			ViewBag.Name = Name;
			//return View();
			var product = context.products.ToList();
			return View(product);
		}
		public IActionResult Delete(int id)
        {
            var p = context.products.SingleOrDefault(x => x.Id == id);
			if (p != null)
            {
                context.products.Remove(p);
				context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

		public IActionResult Del(int id)
		{
			var p = context.productDetails.SingleOrDefault(x => x.Id == id);
			if (p != null)
			{
				context.productDetails.Remove(p);
				context.SaveChanges();
			}
			return RedirectToAction("Index");
		}


		public IActionResult PaymentAccept()
		{
			return View();
		}
		[HttpPost]
        public IActionResult PaymentAccept(PaymentAccept paymentAccept)
        {
			if (ModelState.IsValid)
			{
				return RedirectToAction("Index");
			}
            return View();
        }
        public IActionResult AddProduct(Product product)
		{
			context.products.Add(product);
			context.SaveChanges();
			return RedirectToAction("Index");
		}

		public IActionResult AddProductDetails(ProductDetails productDetails)
		{
			context.productDetails.Add(productDetails);
			context.SaveChanges();
			return RedirectToAction("ProductDetails");
		}


		public IActionResult Edit(int id)
        {
            var product = context.products.SingleOrDefault(x => x.Id == id);
            return View(product);
        }
		[HttpPost]
        public IActionResult UpdateProducts(Product product)
        {
			Product p = context.products.SingleOrDefault(x => x.Id == product.Id) ?? new Product();
			p.ProductName = product.ProductName;
			p.Price = product.Price;

			context.SaveChanges();
			return RedirectToAction("Index");

		}

		
		public IActionResult CreatNewProduct(Product product)
		{
			context.products.Add(product);
			context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult ProductDetails(int id)

		{
			var ProductDetails = context.productDetails.Where(predicate => predicate.ProductId == id).ToList();
			var product = context.products.ToList();
			ViewBag.ProductDetails = ProductDetails;
            return View(product);
		}


		public IActionResult ProductDetails()
		{
			var product = context.products.ToList();
			var ProductDetails = context.productDetails.ToList();
			ViewBag.ProductDetails = ProductDetails;
			// ViewBag.Name = Request.Cookies["Name"];
			//ViewBag.Name = HttpContext.Session.GetString("Name");
			ViewBag.Name = TempData["Name"];
            return View(product);
		}

		public IActionResult Index(string ProductName)
		{
			var product = context.products.Where(x => x.ProductName.Contains(ProductName)).ToList();
			return View(product);

		}
		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}