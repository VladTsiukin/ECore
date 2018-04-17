using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECore.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ClaimsController : Controller
    {
        public IActionResult Index()
        {
            return View(User?.Claims);
        }
    }
}