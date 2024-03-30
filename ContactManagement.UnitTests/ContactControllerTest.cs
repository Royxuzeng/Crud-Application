using System.Collections.Generic;
using System.Linq;
using ContactManagement.Controllers;
using ContactManagement.Data;
using ContactManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;

namespace ContactManagement.UnitTests
{
    [TestFixture]
    public class ContactControllerTests
    {
        private ContactController _controller;
        private ApplicationDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryContactDatabase")
                .Options;
            
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            _controller = new ContactController(_context);
            
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());
            _controller.TempData = tempData;
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Create_WhenCalled_ShouldAddContact()
        {
            var contact = new Contact { Name = "John Doe", Number = "12345" };
            
            _controller.Create(contact);
            
            var storedContact = _context.Contacts.FirstOrDefault(c => c.Name == "John Doe");
            Assert.IsNotNull(storedContact);
            Assert.AreEqual("12345", storedContact.Number);
        }

        [Test]
        public void Delete_WhenCalled_ShouldRemoveContact()
        {
            var contact = new Contact { Name = "Jane Doe", Number = "54321" };
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            
            _controller.DeletePOST(contact.Id);
            
            var storedContact = _context.Contacts.FirstOrDefault(c => c.Name == "Jane Doe");
            Assert.IsNull(storedContact);
        }

        [Test]
        public void Index_WhenCalled_ReturnsViewWithContacts()
        {
            _context.Contacts.Add(new Contact { Name = "John Doe", Number = "12345" });
            _context.Contacts.Add(new Contact { Name = "Jane Doe", Number = "54321" });
            _context.SaveChanges();
            
            var result = _controller.Index(null);
            
            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = result as ViewResult;
            var model = viewResult.Model as IEnumerable<Contact>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count());
        }
        
        [Test]
        public void Edit_ExistingContact_ShouldUpdateContact()
        {
            var originalContact = new Contact { Name = "John Doe", Number = "12345" };
            _context.Contacts.Add(originalContact);
            _context.SaveChanges();
            
            _context.Entry(originalContact).State = EntityState.Detached;

            var updatedContact = new Contact { Id = originalContact.Id, Name = "John Updated", Number = "67890" };
            
            _controller.Edit(updatedContact);
            _context.SaveChanges();
            var result = _context.Contacts.FirstOrDefault(c => c.Id == originalContact.Id);
            
            Assert.IsNotNull(result);
            Assert.AreEqual("John Updated", result.Name);
            Assert.AreEqual("67890", result.Number);
        }
    }
}
