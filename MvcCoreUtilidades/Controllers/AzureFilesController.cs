using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Services;

namespace MvcCoreUtilidades.Controllers
{
    public class AzureFilesController : Controller
    {
        private ServiceStorageFiles service;

        public AzureFilesController(ServiceStorageFiles service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index(string fileName)
        {
            //SI NOS ENVIAN EL NOMBRE, LO LEEMOS
            if (fileName != null)
            {
                string data = await this.service.ReadFileAsync(fileName);
                ViewData["DATA"] = data;
            }
            List<string> files = await this.service.GetFilesAsync();
            return View(files);
        }

        public async Task<IActionResult> DeleteFile(string filename)
        {
            await this.service.DeleteFileAsync(filename);
            return RedirectToAction("Index");
        }

        public IActionResult UploadFiles()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(IFormFile file)
        {
            //RECUPERAMOS EL NOMBRE DEL FICHERO A SUBIR
            string fileName = file.FileName;
            //LEEMOS EL STREAM DE IFormFile Y LO SUBIMOS 
            //A AZURE FILES
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadFileAsync(fileName, stream);
            }
            ViewData["MENSAJE"] = "Archivo subido en Azure Files";
            return View();
        }
    }
}
