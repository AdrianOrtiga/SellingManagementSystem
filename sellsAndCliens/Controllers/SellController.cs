using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SellingManagementSystem.Models;

namespace SellingManagementSystem.Controllers
{
    public class SellController : Controller
    {
        public ActionResult Index()
        {
            using (SellingDBContext db = new SellingDBContext())
            {
                var sellsList = db.Sells.Include(m => m.Client).ToList();
                return View(sellsList);
            }
        }

        // GET: SellController/Details
        [HttpGet]
        public ActionResult Details(long id)
        {
            using(SellingDBContext db = new SellingDBContext())
            {
                var sell = db.Sells.Where(s => s.Id == id).Include(s => s.Client).Include(s => s.Concepts).First();
                if(sell == null) return View(null);

                var concepts = sell.Concepts.ToList();
                foreach(var concept in concepts)
                {
                    concept.Product = db.Products.Where(p => p.Id == concept.ProductId).FirstOrDefault();
                }

                return View(sell);
            }
        }

        // GET: SellController/Create
        [HttpGet]
        public ActionResult Create()
        {
            // search list of clients
            using (SellingDBContext db = new SellingDBContext())
            {
                getClients(db);
                return View();
            }
        }

        // POST: SellController/Create
        [HttpPost]
        public ActionResult Create(Sell sell)
        {

            if(sell.Concepts.Count == 0) return RedirectToAction("Create", "Sell");

            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    // clean products
                    var concepts = sell.Concepts.ToList();
                    foreach (var concept in concepts)
                    {
                        concept.Product = null;
                    }

                    db.Sells.Add(sell);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Sell");
                }
            }
            catch 
            {
                return RedirectToAction("Create", "Sell");
            }
        }

        // GET: SellController/Edit/5
        public ActionResult Edit(long id)
        {

            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    var sell = db.Sells.Where(s => s.Id == id).Include(s => s.Concepts).FirstOrDefault();

                    getClients(db);
                    getProducts(db);

                    return View(sell);
                }
            }
            catch 
            {
                return RedirectToAction("Index", "Sell");
            }
        }

        // POST: SellController/Edit/5
        [HttpPost]
        public ActionResult Edit(Sell sell)
        {
            using (SellingDBContext db = new SellingDBContext())
            {
                using(var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var sellConcepts = sell.Concepts.ToList();
                        var conceptsToRemove = db.Concepts
                                                    .Where(c => c.SellId == sell.Id && !sell.Concepts.Contains(c))
                                                    .ToList();

                        foreach (var concept in conceptsToRemove)
                        {
                            db.Concepts.Remove(concept);
                        }
                        db.SaveChanges();

                        // clean products in order to not update de products
                        foreach(var concept in sell.Concepts)
                        {
                            concept.Product = null;
                        }

                        db.Sells.Update(sell);
                        db.SaveChanges();
                        transaction.Commit();
                        return RedirectToAction("Index", "Sell");
                    } catch (Exception ex)
                    {
                        return RedirectToAction("Edit", "Sell");
                    }
                }
            }
        }

        // Get: SellController/Delete/5
        public ActionResult Delete(long id)
        {
            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    
                    var sell = db.Sells.Find(id);

                    var concepts = getSellConcepts(db, id);
                    foreach (var concept in concepts)
                    {
                        db.Concepts.Remove(concept);
                    }

                    db.Sells.Remove(sell);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Sell");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Sell");
            }
        }

        public ActionResult AddNewConcept(Sell sell)
        {
            sell.Concepts.Add(new Concept());
            return FillConceptFields(sell);
        }

        // Post: SellController/DeleteConcept/5
        public ActionResult DeleteConcept(Sell sell)
        {
            // get all concepts from the form
            var formData = Request.HttpContext.Request.Form;
            var concepts = new List<Concept>();
            
            
            for (int i = sell.Concepts.Count;i< formData.Count(); i++)
            {
                var input = formData["concepts[" + i.ToString() + "].Id"];

                if (input.Count > 0)
                {
                    var concept = new Concept();
                    concept.Id = Convert.ToInt32(formData["concepts[" + i.ToString() + "].Id"][0]);
                    concept.ProductId = Convert.ToInt32(formData["concepts[" + i.ToString() + "].ProductId"][0]);
                    concept.PricePerUnit = Convert.ToDecimal(formData["concepts[" + i.ToString() + "].PricePerUnit"][0]);
                    concept.Quantity = Convert.ToInt32(formData["concepts[" + i.ToString() + "].Quantity"][0]);

                    sell.Concepts.Add(concept);
                }
            }

            return FillConceptFields(sell);
        }

        public ActionResult FillConceptFields(Sell sell)
        {
            using (SellingDBContext db = new SellingDBContext())
            {
                var concepts = sell.Concepts.ToList();
                foreach (var concept in concepts)
                {
                    var id = concept.ProductId;
                    if (id == 0) continue;

                    var product = db.Products.Find(id);
                    if (product == null) continue;
                    concept.Product = product;
                    concept.PricePerUnit = product.PricePerUnit;
                }

                getProducts(db);
                return PartialView("_Concept", sell);
            }
        }

        public ActionResult GetConceptsForEditView(Sell sell)
        {
            using(SellingDBContext db = new SellingDBContext())
            {
                var concepts = getSellConcepts(db, sell.Id);
                foreach (var concept in concepts)
                {
                    sell.Concepts.Add(concept);
                }

                getProducts(db);
                return PartialView("_Concept", sell);
            }
        }

        private void getClients(SellingDBContext db)
        {
            var clientList = db.Clients.ToList();
            ViewData["clients"] = clientList;
        }

        private void getProducts(SellingDBContext db)
        {
            var productList = db.Products.ToList();
            ViewData["products"] = productList;
        }

        private async Task<List<Concept>> getConceptsAsync(SellingDBContext db, long id)
        {
            return await db.Concepts
                    .Where(c => c.SellId.Equals(id))
                    .ToListAsync();
        }

        private List<Concept> getSellConcepts(SellingDBContext db, long id)
        {
            return db.Concepts
                    .Where(c => c.SellId.Equals(id))
                    .ToList();
        }
    }
}
