using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace AuthMasterEv.Models.ViewModel
{
    public class CustomerVM
    {
          public CustomerVM()
            {
                this.BookList = new List<int>();
            }
            public int CustomerId { get; set; }

            public string CustomerName { get; set; }
            [Required, DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

            public DateTime BuyingDate { get; set; }

            public int Phone { get; set; }

            public string Picture { get; set; }

            public IFormFile PictureFile { get; set; }

            public bool MaritialStatus { get; set; }

            public List<int> BookList { get; set; } 
      
    }
}
