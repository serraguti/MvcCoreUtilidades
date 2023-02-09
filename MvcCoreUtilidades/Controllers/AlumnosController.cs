using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Models;
using MvcCoreUtilidades.Services;

namespace MvcCoreUtilidades.Controllers
{
    public class AlumnosController : Controller
    {
        private ServiceAzureAlumnos service;

        public AlumnosController(ServiceAzureAlumnos service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string curso)
        {
            //PARA PODER ACCEDER A LOS ALUMNOS, PRIMERO NECESITAMOS
            //EL TOKEN
            string token =
                await this.service.GetTokenAsync(curso);
            //LLAMAMOS AL SERVICIO DE AZURE Y RECUPERAMOS LOS DATOS
            List<Alumno> alumnos = await this.service.GetAlumnosAsync(token);
            return View(alumnos);
        }

        public async Task<IActionResult> Delete
            (string curso, string partitionkey, string rowkey)
        {
            string token =
                await this.service.GetTokenAsync(curso);
            await this.service.DeleteAlumnoAsync(token, partitionkey, rowkey);
            return RedirectToAction("Index");
        }
    }
}
