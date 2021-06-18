using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieGR8.Models;

namespace MovieGR8.Controllers
{
    public class UsuariosController : Controller
    {
        readonly LoginContext lcx;

        public UsuariosController(LoginContext _lcx)
        {
            lcx = _lcx;
        }
        public async Task<IActionResult> Index()
        {
            return Ok(await lcx.Usuarios.Include("Roles.Rol").ToListAsync());
        }
    }
}
