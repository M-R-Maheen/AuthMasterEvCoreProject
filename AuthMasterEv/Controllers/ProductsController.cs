using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMasterEv.Controllers
{
    [Authorize(Roles = "Executive,Admin")] 
    //[Authorize(Policy = "EmployeeOnly")]
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult All()
        {

            return View();
        }

        public IActionResult Search()
        {

            return View();
        }
    }
}
