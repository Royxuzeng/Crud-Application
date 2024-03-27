using System.Collections.Generic;
using System.Linq;
using ContactManagement.Data;
using ContactManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagement.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _db;
    
        public ContactController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Contact> objContactList = _db.Contacts;
            return View(objContactList);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Contact obj)
        {
            if (ModelState.IsValid)
            {
                _db.Contacts.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Contacts.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        
        [HttpPost]
        public IActionResult Edit(Contact obj)
        {
            if (ModelState.IsValid)
            {
                _db.Contacts.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.Contacts.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Contacts.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            _db.Contacts.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        public IActionResult Search(string searchTerm)
        {
            IEnumerable<Contact> filteredContacts;

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredContacts = _db.Contacts;
            }
            else
            {
                filteredContacts = _db.Contacts.Where(c => c.Name.Contains(searchTerm));
            }

            return View("Index", filteredContacts);
        }
    }
}