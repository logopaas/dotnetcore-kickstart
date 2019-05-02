using LogoPaasSampleApp.Dal.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NAFCore.DAL.EF.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace LogoPaasSampleApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Repository<Customer, long> _customerRepo;

        public CustomerController(Repository<Customer, long> customerRepo) 
        {
            _customerRepo = customerRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<Customer> lstCustomer = _customerRepo.GetAll().ToList();

            return View(lstCustomer);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create([Bind] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer = _customerRepo.Insert(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Customer customer = _customerRepo.FindById(id.Value);

            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(int id, [Bind]Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                customer=_customerRepo.Update(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Customer customer = _customerRepo.FindById(id.Value);

            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Customer customer = _customerRepo.FindById(id.Value);

            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(long id)
        {
            _customerRepo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}