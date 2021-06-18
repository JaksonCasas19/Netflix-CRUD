using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieGR8.Helper;
using MovieGR8.Models;
using Newtonsoft.Json.Linq;

namespace MovieGR8.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        LoginContext lcx;


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, LoginContext _lcx)
        {
            _logger = logger;
            lcx = _lcx;
        }

        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult>Registro()
        {
            return View();
        }

        
        [BindProperty]
        public Usuarios Usuario { get; set; }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registrar()
        {
            /*Verificar si el usuario existe*/
            var result = await lcx.Usuarios.Where(x => x.Nombre == Usuario.Nombre).SingleOrDefaultAsync();
            if(result != null)
            {
                return BadRequest(new JObject(){
                    { "StatusCode", 400},
                    { "Message","El usuario ya existe" }
                        
                   });
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    /*Me va seleccionar todos los errores del modelo*/
                    return BadRequest(ModelState.SelectMany(x=>x.Value.Errors.Select(y=>y.ErrorMessage)).ToList());
                }
                else
                {
                    var hash = HashHelper.Hash(Usuario.Clave);
                    Usuario.Clave = hash.Password;
                    Usuario.Sal = hash.Salt;
                    lcx.Usuarios.Add(Usuario);
                    await lcx.SaveChangesAsync();
                    Usuario.Clave = "";
                    Usuario.Sal = "";
                    return Created($"/Usuarios/{Usuario.IdUsuario}",Usuario);
                }
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
