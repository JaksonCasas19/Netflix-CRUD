using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieGR8.Helper;
using MovieGR8.Models;
using MovieGR8.Models.ViewModel;
using Newtonsoft.Json.Linq;

namespace MovieGR8.Controllers
{
    public class LoginController : Controller
    {

        LoginContext lcx;

        public LoginController(LoginContext _lcx)
        {
            lcx = _lcx;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Home");
            }
            else
            {
                return View();
            }
            
        }
        [BindProperty]
        public UsuarioVM Usuario{get;set;}
        public async Task<IActionResult> Login()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new JObject(){
                    { "StatusCode", 400},
                    { "Message","El usuario ya existe" }

                   });
            }
            else
            {
                var result = await lcx.Usuarios.Include("Roles.Rol").Where(x => x.Nombre == Usuario.Nombre).SingleOrDefaultAsync();
                if(result == null)
                {
                    return NotFound(new JObject(){
                    { "StatusCode", 400},
                    { "Message","Usuario no encontrado" }

                   });
                }
                else
                {
                   
                    if(HashHelper.CheckHash(Usuario.Clave, result.Clave, result.Sal))
                    {
                        if (result.Roles.Count == 0)
                        {
                            var response = new JObject(){
                         { "StatusCode", 403},
                         { "Message","Lo siento no tiene acceso a esta pagina." }
                         };
                            return StatusCode(403, response);
                        }



                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.IdUsuario.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Name, result.Nombre));
                        TempData["MyIdUser"] = result.IdUsuario.ToString();
                        TempData["MyName"] = result.Nombre;
                        identity.AddClaim(new Claim(ClaimTypes.Email, "jaksoncasas@gmail.com"));
                        identity.AddClaim(new Claim("Dato","Valor"));
                        identity.AddClaim(new Claim("Edad", result.Edad.ToString()));

                        foreach(var Rol in result.Roles)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, Rol.Rol.Descripcion));
                        }

                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                            new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddDays(1), IsPersistent = true });
                        
                        return Ok(result);
                    }
                    else
                    {
                        var response = new JObject(){
                         { "StatusCode", 403},
                         { "Message","Usuario o contraseña no valido." }
                         };
                        return StatusCode(403, response);
                    }
                }
            }
           
        }

        /*Cerrar sesión*/
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Login");
        }

    }
}
