using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderManager.Data;
using OrderManager.Models;

namespace OrderManager.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderContext _context;

        public OrdersController(OrderContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string provider, string search, DateTime from, DateTime to)
        {
            var orders = await _context.Orders.ToListAsync();
            var providers = await _context.Providers.Distinct().ToListAsync();
            var items = await _context.Items.ToListAsync();

            ViewBag.Providers = new SelectList(providers, "Id", "Name", provider);

            var orderViews = new List<OrderView>();
            foreach (var order in orders)
            {
                var orderView = new OrderView(order, providers.Where(x => x.Id == order.ProviderId).FirstOrDefault(), items);
                orderViews.Add(orderView);
            }

            var orderQuery = orderViews.Select(x => x);

            if (!string.IsNullOrEmpty(search))
            {
                ViewBag.Search = search;
                orderQuery = orderQuery.Where(s => s.Number.Contains(search) ||
                                                    items.Select(x => x.Name).Contains(search));
            }

            if (!string.IsNullOrEmpty(provider) && provider != "0")
            {
                orderQuery = orderQuery.Where(s => s.ProviderId.ToString() == provider);
            }

            if (from != DateTime.MinValue || to != DateTime.MinValue)
            {
                ViewBag.DateFrom = from.Date.ToString("yyyy-MM-dd");
                ViewBag.DateTo = to.Date.ToString("yyyy-MM-dd");
                orderQuery = orderQuery.Where(s => s.Date.CompareTo(from) >= 0 && s.Date.CompareTo(to) <= 0);
            }
            else
            {
                ViewBag.DateFrom = DateTime.Now.AddDays(-30).Date.ToString("yyyy-MM-dd");
                ViewBag.DateTo = DateTime.Now.Date.ToString("yyyy-MM-dd");
            }

            orderViews = orderQuery.ToList();

            return View(orderViews);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var provider = await _context.Providers.FirstOrDefaultAsync(m => m.Id == order.ProviderId);
            var items = await _context.Items.Where(m => m.OrderId == order.Id).ToListAsync();
            var orderView = new OrderView(order, provider, items);

            return View(orderView);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewBag.Providers = new SelectList(_context.Providers.Distinct(), "Id", "Name");

            var orderView = new OrderView();
            return View(orderView);
        }

        // POST: Orders/Create
        [HttpPost]
        public async Task<ActionResult> CreateOrder(Order order, List<OrderItem> items)
        {
            _context.Orders.Add(order);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            foreach (var item in items)
            {
                item.OrderId = order.Id;
            }

            _context.Items.AddRange(items);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Json(new { redirectToUrl = Url.Action("Index", "Orders") });
        }

        // GET: Orders/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            var providers = _context.Providers.Distinct().ToList();
            var provider = providers.FirstOrDefault(m => m.Id == order.ProviderId);

            var items = await _context.Items.Where(m => m.OrderId == order.Id).ToListAsync();

            var orderView = new OrderView(order, provider, items);
            ViewBag.Providers = new SelectList(providers, "Id", "Name");

            return View(orderView);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(Order order, List<OrderItem> items)
        {
            if (order.Id == 0)
            {
                return NotFound();
            }
            _context.Orders.Update(order);

            foreach (var item in items)
            {
                item.OrderId = order.Id;
            }

            var newItems = items.Where(x => x.Id == 0);
            var updatedItems = items.Where(x => x.Id != 0);

            var orderItems = _context.Items.Where(x => x.OrderId == order.Id);
            var deletedItems = orderItems.Where(x => !items.Contains(x));

            try
            {
                _context.Orders.Update(order);
                _context.Items.AddRange(newItems);
                _context.Items.RemoveRange(deletedItems);
                _context.Items.UpdateRange(updatedItems);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Json(new { redirectToUrl = Url.Action("Index", "Orders") });
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            var orderItems = _context.Items.Where(x => x.OrderId == order.Id);

            _context.Orders.Remove(order);
            _context.Items.RemoveRange(orderItems);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        public ActionResult DisplayNewItem()
        {
            return PartialView("_Item", new OrderItem());
        }
    }
}
