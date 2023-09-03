using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using Dashboard.Migrations;
using Microsoft.CodeAnalysis;

namespace Dashboard.Controllers.Shoping
{
    public class ShopingController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ShopingController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult ProductDetails(int id)
        {
            var ProductDetails = _context.productDetails.Where(predicate => predicate.ProductId == id).ToList();

            return View(ProductDetails);
        }
        public async Task<string> SendMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress ("Test Email", "bayader.bk@gmail.com")); //sender
            message.To.Add(MailboxAddress.Parse("bayader.k.algamdi@gmail.com")); // recvid

            message.Subject="Test Message From Bayader";
            message.Body = new TextPart("<h1> this test massage </h1>")
            {
                Text="<h1> Hi </h1>"
            };

            using (var clint = new SmtpClient())
            { 
                try
                {
                    clint.Connect("smtp.gmail.com", 587);
                    clint.Authenticate("bayader.bk@gmail.com", "zepprsvavygtmbik");
                    await clint.SendAsync(message);
                }catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                    clint.Disconnect(true);
                }
                return "Ok";
            }
        }
        [Authorize]
        public IActionResult Checkout(int id)
        {
            var user = HttpContext.User.Identity.Name;
           var ProductDetails = _context.productDetails.SingleOrDefault(predicate => predicate.ProductId == id);
           var cart = new Cart()
           {
               IdCostumers = user,
               MyProductId = ProductDetails.ProductId,
               Images = ProductDetails.Image,
               Price = ProductDetails.Price,
               Total = ProductDetails.Price ,
              ProductName = ProductDetails.ProductName,
              Qty = ProductDetails.Qty,
               Tax = 0.15
              
            };
           _context.carts.Add(cart);
            _context.SaveChanges();
            return View(cart);
        }

        public IActionResult Invoices(int id)
        {
           var user = HttpContext.User.Identity.Name;
            var Cart = _context.carts.FirstOrDefault(p => p.MyProductId == id);
            var invoice = new Invoice()
            {
                CostumerId = user,
                ProductId = Cart.MyProductId,
                Price = Cart.Price,
                Qty = Cart.Qty,
                Tax = 0.15,
                Total = Cart.Total

            };
            _context.invoices.Add(invoice);
            _context.SaveChanges();
            return View(Cart);
        }
        public IActionResult Index()
        {
            var Product = _context.products.ToList();
            return View(Product);
        }
    }
}
