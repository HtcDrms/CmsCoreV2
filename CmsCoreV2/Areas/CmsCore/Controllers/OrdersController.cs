using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CmsCoreV2.Data;
using CmsCoreV2.Models;
using Microsoft.AspNetCore.Authorization;
using SaasKit.Multitenancy;
using Z.EntityFramework.Plus;

namespace CmsCoreV2.Areas.CmsCore.Controllers
{
    [Authorize(Roles = "ADMIN,POST")]
    [Area("CmsCore")]
    public class OrdersController : ControllerBase
    {
        

        public OrdersController(ApplicationDbContext context, ITenant<AppTenant> tenant) : base(context, tenant)
        {
          
        }

        // GET: CmsCore/Orders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SetFiltered<Order>().Where(x => x.AppTenantId == tenant.AppTenantId).Include(o => o.Customer).Include(o => o.PaymentMethod);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CmsCore/Orders/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.PaymentMethod)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: CmsCore/Orders/Create
        public IActionResult Create()
        {
            var order = new Order();
            order.CreatedBy = User.Identity.Name ?? "username";
            order.CreateDate = DateTime.Now;
            order.UpdatedBy = User.Identity.Name ?? "username";
            order.OrderDate = DateTime.Now;
            order.UpdateDate = DateTime.Now;
            order.AppTenantId = tenant.AppTenantId;
           
            ViewData["CustomerId"] = new SelectList(_context.Customers.ToList(), "Id", "Name");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods.ToList(), "Id", "Name");
            return View(order);
        }

        // POST: CmsCore/Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDate,OrderStatus,CustomerId,Email,Phone,TransactionId,PaymentMethodId,CustomerNote,Id,CreateDate,CreatedBy,UpdateDate,UpdatedBy,AppTenantId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Id", order.PaymentMethodId);
            return View(order);
        }

        // GET: CmsCore/Orders/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Id", order.PaymentMethodId);
            return View(order);
        }

        // POST: CmsCore/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("OrderDate,OrderStatus,CustomerId,Email,Phone,TransactionId,PaymentMethodId,CustomerNote,Id,CreateDate,CreatedBy,UpdateDate,UpdatedBy,AppTenantId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
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
                return RedirectToAction("Index");
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Id", order.CustomerId);
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Id", order.PaymentMethodId);
            return View(order);
        }

        // GET: CmsCore/Orders/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.PaymentMethod)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: CmsCore/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var order = await _context.Orders.SingleOrDefaultAsync(m => m.Id == id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool OrderExists(long id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
