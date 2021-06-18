using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieGR8.Controllers
{
    public class ModulosController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Administrador")]
        public IActionResult Administracion()
        {
            return Content("<h1>Administracion</h1>","text/html");
        }
        [Authorize(Roles = "Gerente")]
        public IActionResult Gerencia()
        {
            return Content("<h1>Gerencia</h1>", "text/html");
        }
        [Authorize(Roles = "Secretario")]
        public IActionResult Secretaria()
        {
            return Content("<h1>Secretaria</h1>", "text/html");
        }
        [Authorize(Roles = "Cajero")]
        public IActionResult Caja()
        {
            return Content("<h1>Caja</h1>", "text/html");
        }

        /* Autrización con mas roles mas personalizada
        [Authorize(Roles = "Cajero,Gerenete")]
        public IActionResult GerenteOCaja()
        {
            return Content("<h1>Gerente o Cajero</h1>", "text/html");
        }


        [Authorize(Roles = "Cajero")]
        [Authorize(Roles = "Gerente")]
        public IActionResult GerenteCaja()
        {
            return Content("<h1>Gerente/Cajero</h1>", "text/html");
        }*/

    }
}
