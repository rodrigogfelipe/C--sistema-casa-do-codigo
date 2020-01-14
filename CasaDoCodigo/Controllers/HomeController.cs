using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CasaDoCodigo.Controllers
{   // Fez a requisição para HomeController
    public class HomeController : Controller
    {
        public IActionResult Index() // Passar Action Index que é ação
        {
            return View(); // Retorna na camada MVC View
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
