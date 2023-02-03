using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Helpers;

namespace MvcCoreUtilidades.Controllers
{
    public class FicherosController : Controller
    {
        private HelperPathProvider helperPath;

        public FicherosController(HelperPathProvider helperPath)
        {
            this.helperPath = helperPath;
        }

        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile archivo)
        {
            string fileName = archivo.FileName;
            string path = this.helperPath.GetMapPath(Folders.Uploads, fileName);
            //UNA VEZ QUE TENEMOS LA RUTA, UN ARCHIVO SE LEE COMO 
            //STREAM, QUE UN FLUJO DE BYTES
            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }
            ViewData["MENSAJE"] = "Fichero subido en: " + path;
            string folder = this.helperPath.GetNameFolder(Folders.Uploads);
            ViewData["URLFICHERO"] = this.helperPath.GetWebHostUrl() + folder + "/" + fileName;
            return View();
        }
    }
}
