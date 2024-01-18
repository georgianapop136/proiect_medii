using CafeApp.Data;
using CafeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CafeApp.Controllers
{
    public class BaristaController : Controller
    {
        private readonly DataContext _context;
        public BaristaController(DataContext cont)
        {
            _context = cont;
        }


        //SHOW ORDERS
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OrderList()
        {
          
            var orders = await _context.Orders.ToListAsync();
            return View(orders);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OrderDetails(int? id)
        {

            var order = await _context.Orders.FindAsync(id);

            List<OrderProduct> orderProducts = _context.OrderProducts
            .Where(op => op.OrderId == id)
            .ToList();

            List<int> productIds = orderProducts.Select(op => op.ProductId).ToList();

            List<Product> products = _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToList();

            Dictionary<int, int> productIdQuantityPairs = orderProducts
                .ToDictionary(op => op.ProductId, op => op.Quantity);

            var tuple = Tuple.Create(order, products, productIds, productIdQuantityPairs);
            return View(tuple);
        }

        // GET: Barista/ProductList
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ProductList()
        {

            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var order = await _context.Orders.FindAsync(id);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int orderId)
        {
            List<OrderProduct> orderProducts = await _context.OrderProducts
            .Where(op => op.OrderId == orderId)
            .ToListAsync();
            _context.OrderProducts.RemoveRange(orderProducts);

            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
                _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return RedirectToAction("OrderList", "Barista");
        }

        [Authorize]
        public IActionResult Chat()
        {
            return View();
        }

    }
}