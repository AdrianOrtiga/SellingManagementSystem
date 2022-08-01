using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SellingManagementSystem.Models;

namespace SellingManagementSystem.Controllers
{
    public class ProductController : Controller
    {
        // GET: ProductController
        public ActionResult Index()
        {
            using (SellingDBContext db = new SellingDBContext())
            {
                var productList = db.Products.ToList();
                return View(productList);
            }
        }

        // GET: ProductController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                await using (SellingDBContext db = new SellingDBContext())
                {
                    var product = db.Products.Find(id);
                    return Ok(product);
                } 
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {

            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    var product = db.Products.Find(id);
                    return View(product);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Product");
            }
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    db.Products.Update(product);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // Get: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    var product = db.Products.Find(id);
                    db.Products.Remove(product);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Product");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Product");
            }
        }
    }
}
