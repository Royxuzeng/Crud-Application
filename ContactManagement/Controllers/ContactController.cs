using System;
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
        public IActionResult Index(string sortOrder)
        {
            ViewData["NameSortParm"] = sortOrder == "name_desc" ? "name_asc" : "name_desc";
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "date_asc" : "date_desc";

            IEnumerable<Contact> contacts = _db.Contacts;

            switch (sortOrder)
            {
                case "name_desc":
                    contacts = contacts.OrderByDescending(c => c.Name);
                    break;
                case "name_asc":
                    contacts = contacts.OrderBy(c => c.Name);
                    break;
                case "date_desc":
                    contacts = contacts.OrderByDescending(c => c.CreatedDateTime);
                    break;
                case "date_asc":
                    contacts = contacts.OrderBy(c => c.CreatedDateTime);
                    break;
            }

            return View(contacts);
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
                TempData["success"] = "Contact created successfully";
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
                TempData["success"] = "Contact updated successfully";
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
            TempData["success"] = "Contact deleted successfully";
            return RedirectToAction("Index");
        }
    }
}