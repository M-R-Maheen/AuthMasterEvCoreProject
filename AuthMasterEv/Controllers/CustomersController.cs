using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthMasterEv.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.Data;
using System.IO;
using AuthMasterEv.Models.ViewModel;

namespace AuthMasterEv.Controllers
{
    //[Authorize(Roles = "Admin,Ececutive")]
    public class CustomersController : Controller
    {
        private readonly BookDbContext _context;
        private readonly IWebHostEnvironment _he;

        public CustomersController(BookDbContext context, IWebHostEnvironment he)
        {
            _context = context;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_context.Customers.Include(x => x.BookEntries).ThenInclude(b => b.Book).ToList());
        }
        public IActionResult AddNewBook(int? id)
        {
            ViewBag.Books = new SelectList(_context.Books.ToList(), "BookId", "BookName", id.ToString() ?? "");
            ViewBag.Prices = new SelectList(_context.Prices.ToString().ToList(), "BookId", "Price", id.ToString() ?? "");
            return PartialView("_AddNewBook");
        }
        //public IActionResult AddNewPrice(int? id)
        //{
        //    ViewBag.Prices = new SelectList(_context.Prices.ToString().ToList(), "BookId", "Price", id.ToString() ?? ""); 
        //    return PartialView("_AddedPrice");
        //}
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CustomerVM customerVM, int[] BookId)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer()
                {
                    CustomerName = customerVM.CustomerName,
                    BuyingDate = customerVM.BuyingDate,
                    Phone = customerVM.Phone.ToString(),
                    MaritialStatus = customerVM.MaritialStatus
                };
                //Img
                string webroot = _he.WebRootPath;
                string folder = "Images";
                string imgFileName = Path.GetFileName(customerVM.PictureFile.FileName);
                string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                using (var stream = new FileStream(fileToWrite, FileMode.Create))
                {
                    await customerVM.PictureFile.CopyToAsync(stream);
                    customer.Picture = "/" + folder + "/" + imgFileName;
                }
                foreach (var item in BookId)
                {
                    BookEntry bookEntry = new BookEntry()
                    {
                        Customer = customer,
                        CustomerId = customer.CustomerId,
                        BookId = item
                    };
                    _context.BookEntries.Add(bookEntry);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            Customer customer = _context.Customers.First(x => x.CustomerId == id);
            var bookList = _context.BookEntries.Where(x => x.CustomerId == id).Select(x => x.BookId).ToList();

            CustomerVM customerVM = new CustomerVM
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.CustomerName,
                BuyingDate = DateTime.Now,
                Phone =Int32.Parse(customer.Phone),
                MaritialStatus = customer.MaritialStatus,
                BookList = bookList,
                Picture = customer.Picture
            };
            return View(customerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerVM customerVM, int[] BookId)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer()
                {
                    CustomerId = customerVM.CustomerId,
                    CustomerName = customerVM.CustomerName,
                    BuyingDate = customerVM.BuyingDate,
                    Phone = customerVM.Phone.ToString(),
                    MaritialStatus = customerVM.MaritialStatus,
                    Picture = customerVM.Picture,
                };
                //img
                if (customerVM.PictureFile != null)
                {

                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(customerVM.PictureFile.FileName);
                    string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                    using (var stream = new FileStream(fileToWrite, FileMode.Create))
                    {
                        await customerVM.PictureFile.CopyToAsync(stream);
                        customer.Picture = "/" + folder + "/" + imgFileName;
                    }
                }
                //exists diseselist
                var existsBook = _context.BookEntries.Where(x => x.CustomerId == customerVM.CustomerId).ToList();
                foreach (var item in existsBook)
                {
                    _context.BookEntries.Remove(item);
                }
                //add new diseseList
                foreach (var item in BookId)
                {
                    BookEntry bookEntry = new BookEntry()
                    {
                        CustomerId = customer.CustomerId,
                        BookId = item
                    };
                    _context.BookEntries.Add(bookEntry);
                }
                _context.Entry(customer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();

        }
        public async Task<IActionResult> Delete(int? id)
        {
            Customer customer = _context.Customers.First(x => x.CustomerId == id);
            var bookList = _context.BookEntries.Where(x => x.CustomerId == id).ToList();
            foreach (var item in bookList)
            {
                _context.BookEntries.Remove(item);
            }
            _context.Entry(customer).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
