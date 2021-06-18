using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieGR8.Models;

namespace MovieGR8.Controllers
{
    /*[Authorize(Policy = "MayorDeEdad")]*/
    [Authorize(Roles = "Administrador")]
    public class PostsController : Controller
    {

        private readonly IConfiguration _configuration;

        public PostsController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("PeliculaViewAll", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }


        public List<SelectListItem> ObtenerListado()
        {

            return new List<SelectListItem>(){
            new SelectListItem() {
            Text = "Acción",
            Value = "Acción"
           /* Selected = true*/
            },
            new SelectListItem() {
            Text = "Animación",
            Value = "Animación"
            },
             new SelectListItem() {
            Text = "Comedia",
            Value = "Comedia"
            },
              new SelectListItem() {
            Text = "Adultos",
            Value = "Adultos"
            },
            new SelectListItem() {
            Text = "Aventura",
            Value = "Aventura"
            },
            new SelectListItem() {
            Text = "Terror",
            Value = "Terror"
           }
         };
        }



        public IActionResult AddOrEdit(int? id)
        {
            PeliculaModel peliculaModel = new PeliculaModel();
            if (id > 0)
                peliculaModel = FetchBookByID(id);
            ViewBag.MiListado = ObtenerListado();
            return View(peliculaModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("IdPelicula,Titulo,YearMovie,Director,Descripcion,Company,Categoria,Thumbnail,TrailerVideo,CreatedBy")] PeliculaModel peliculaModel)
        {
            ViewBag.MiListado = ObtenerListado();
            if (ModelState.IsValid)
            {
                String User = TempData.Peek("MyName").ToString();
                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("PeliculaAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("IdPelicula", peliculaModel.IdPelicula);
                    sqlCmd.Parameters.AddWithValue("Titulo", peliculaModel.Titulo);
                    sqlCmd.Parameters.AddWithValue("YearMovie", peliculaModel.YearMovie);
                    sqlCmd.Parameters.AddWithValue("Director", peliculaModel.Director);
                    sqlCmd.Parameters.AddWithValue("Descripcion", peliculaModel.Descripcion);
                    sqlCmd.Parameters.AddWithValue("Company", peliculaModel.Company);
                    sqlCmd.Parameters.AddWithValue("Categoria", peliculaModel.Categoria);
                    sqlCmd.Parameters.AddWithValue("Thumbnail", peliculaModel.Thumbnail);
                    sqlCmd.Parameters.AddWithValue("TrailerVideo", peliculaModel.TrailerVideo);
                    sqlCmd.Parameters.AddWithValue("CreatedBy", User);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(peliculaModel);
        }



        [NonAction]
        public PeliculaModel FetchBookByID(int? id)
        {
            PeliculaModel peliculaModel = new PeliculaModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("PeliculaViewById", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("IdPelicula", id);
                sqlDa.Fill(dtbl);
                if (dtbl.Rows.Count == 1)
                {
                    peliculaModel.IdPelicula = Convert.ToInt32(dtbl.Rows[0]["IdPelicula"].ToString());
                    peliculaModel.Titulo = dtbl.Rows[0]["Titulo"].ToString();
                    peliculaModel.YearMovie = dtbl.Rows[0]["YearMovie"].ToString();
                    peliculaModel.Descripcion = dtbl.Rows[0]["Descripcion"].ToString();
                    peliculaModel.Director = dtbl.Rows[0]["Director"].ToString();
                    peliculaModel.Company = dtbl.Rows[0]["Company"].ToString();
                    peliculaModel.Categoria = dtbl.Rows[0]["Categoria"].ToString();
                    peliculaModel.CreatedBy = dtbl.Rows[0]["CreatedBy"].ToString();
                    peliculaModel.Thumbnail = dtbl.Rows[0]["Thumbnail"].ToString();
                    peliculaModel.TrailerVideo = dtbl.Rows[0]["TrailerVideo"].ToString();

                    ViewData["Titulo"] = dtbl.Rows[0]["Titulo"].ToString();
                    ViewData["Descripcion"] = dtbl.Rows[0]["Descripcion"].ToString();
                    ViewData["Catg"] = dtbl.Rows[0]["Categoria"].ToString();
                    ViewData["Trailer"] = dtbl.Rows[0]["TrailerVideo"].ToString();

                }
                return peliculaModel;
            }
        }

    }
}
