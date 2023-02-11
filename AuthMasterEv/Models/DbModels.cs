using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace AuthMasterEv.Models
{
    public class Customer
    {
        public Customer()
        {
            this.BookEntries = new List<BookEntry>();   
        }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime BuyingDate { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public bool MaritialStatus { get; set; }
        //nav
        public virtual ICollection<BookEntry> BookEntries { get; set; }

    }
    public class Book
    {
        public Book()
        {
            this.BookEntries = new List<BookEntry>();
        }

        public int BookId { get; set; }

        public string BookName { get; set; }

        public double Price { get; set; }

        //nav
        public virtual ICollection<BookEntry> BookEntries { get; set; }
    }
    public class BookEntry
    {
        public int BookEntryId { get; set; }
        [ForeignKey("Customer")]

        public int CustomerId { get; set; }
        [ForeignKey("Book")]

        public int BookId { get; set; }

        //nav
        public virtual Customer Customer { get; set; }

        public virtual Book Book { get; set; }
    }
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookEntry> BookEntries { get; set; }
        public object Prices { get; internal set; }
    }
}
