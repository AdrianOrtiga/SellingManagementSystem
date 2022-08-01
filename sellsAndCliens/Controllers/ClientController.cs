using Microsoft.AspNetCore.Mvc;
using SellingManagementSystem.Models;

namespace SellingManagementSystem.Controllers
{
    public class ClientController : Controller
    {
        // GET: ClientController
        public ActionResult Index()
        {
            using (SellingDBContext db = new SellingDBContext())
            {
                var clientList = db.Clients.ToList();
                return View(clientList);
            }

        }

        // GET: ClientController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClientController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClientController/Create
        [HttpPost]
        public ActionResult Create(Client client)
        {
            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    db.Clients.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Client");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: ClientController/Edit/5
        public ActionResult Edit(int id)
        {

            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    var client = db.Clients.Find(id);
                    return View(client);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Client");
            }
        }

        // POST: ClientController/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Client client)
        {
            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    db.Clients.Update(client);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Client");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // Get: ClientController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                using (SellingDBContext db = new SellingDBContext())
                {
                    var client = db.Clients.Find(id);
                    db.Clients.Remove(client);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Client");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Client");
            }
        }
    }
}
