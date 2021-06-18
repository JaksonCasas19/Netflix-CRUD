using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MovieGR8.Models;

namespace MovieGR8.Controllers
{
    [Authorize]
    public class PeliculaController : Controller
    {
        private readonly IConfiguration _configuration;

        public PeliculaController(IConfiguration configuration)
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

        public IActionResult Detalle(int? id)
        {
            PeliculaModel peliculaModel = new PeliculaModel();
            if (id > 0)
                peliculaModel = FetchBookByID(id);
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
                    peliculaModel.Director = dtbl.Rows[0]["Director"].ToString();

                    ViewData["GetIdMovie"] = Convert.ToInt32(dtbl.Rows[0]["IdPelicula"].ToString());


                    TempData["GetIdMoviePe"] = Convert.ToInt32(dtbl.Rows[0]["IdPelicula"].ToString());
                    ViewData["Titulo"]= dtbl.Rows[0]["Titulo"].ToString();
                    ViewData["Descripcion"] = dtbl.Rows[0]["Descripcion"].ToString();
                    ViewData["Catg"] =  peliculaModel.Director = dtbl.Rows[0]["Categoria"].ToString();
                    ViewData["Trailer"] = dtbl.Rows[0]["TrailerVideo"].ToString();
                   
       
                }




                
                using (SqlConnection sqlConnection2 = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    String User = TempData.Peek("MyIdUser").ToString();
                    DataTable dtbl2 = new DataTable();
                    sqlConnection2.Open();
                    SqlDataAdapter sqlDa1 = new SqlDataAdapter("FavoriteDetalleAll", sqlConnection2);
                    sqlDa1.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDa1.SelectCommand.Parameters.AddWithValue("@IdPelicula", ViewData["GetIdMovie"]);
                    sqlDa1.SelectCommand.Parameters.AddWithValue("@IdUsuario", User);
                    sqlDa1.Fill(dtbl2);
                    ViewData["ReturnRows"] = dtbl2.Rows[0]["StatusF"].ToString();

                }



                using (SqlConnection sqlConnection3 = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    DataTable dtbl3 = new DataTable();
                    sqlConnection3.Open();
                    SqlDataAdapter sqlDa1 = new SqlDataAdapter("SP_RecomendCategoria", sqlConnection3);
                    sqlDa1.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sqlDa1.SelectCommand.Parameters.AddWithValue("@Categoria", ViewData["Catg"]);
                    sqlDa1.Fill(dtbl3);
                    ViewData["RecomendCateg"] = dtbl3;

                }



                /*ViewData["ReturnRows"]*/
                return peliculaModel;
            }
        }


       


        public IActionResult Favorito()
        {
            DataTable dtb2 = new DataTable();
            /*String User = TempData.Peek("MyIdUser").ToString();*/

            String User = TempData.Peek("MyIdUser").ToString();



            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
           
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("SP_MiLista", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("@IdUsuario",Convert.ToInt32(User));
                sqlDa.Fill(dtb2);
            }
            return View(dtb2);
        }




        public IActionResult Delete(int? id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                String User = TempData.Peek("MyIdUser").ToString();
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("SP_DeleteFavorite", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@IdPelicula", id);
                sqlCmd.Parameters.AddWithValue("@IdUsuario", User);
                sqlCmd.Parameters.AddWithValue("@IdCond", 0);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Favorito));
        }


 
        public IActionResult AddFav()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                String User = TempData.Peek("MyIdUser").ToString();
                String GetPeliStart = TempData.Peek("GetIdMoviePe").ToString();
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("SP_DeleteFavorite", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@IdPelicula", GetPeliStart);
                sqlCmd.Parameters.AddWithValue("@IdUsuario", User);
                sqlCmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                sqlCmd.Parameters.AddWithValue("@StatusF", 1);
                sqlCmd.Parameters.AddWithValue("@IdCond", 1);


                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
