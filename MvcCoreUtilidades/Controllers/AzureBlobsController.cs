using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Models;
using MvcCoreUtilidades.Services;

namespace MvcCoreUtilidades.Controllers
{
    public class AzureBlobsController : Controller
    {
        private ServiceStorageBlobs service;

        public AzureBlobsController(ServiceStorageBlobs service)
        {
            this.service = service;
        }

        public async Task<IActionResult> ListContainers()
        {
            List<string> containers = await this.service.GetContainersAsync();
            return View(containers);
        }

        public IActionResult CreateContainer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContainer(string containername)
        {
            await this.service.CreateContainerAsync(containername);
            return RedirectToAction("ListContainers");
        }

        public async Task<IActionResult> DeleteContainer(string containerName)
        {
            await this.service.DeleteContainerAsync(containerName);
            return RedirectToAction("ListContainers");
        }

        //ACTIONRESULT PARA LOS BLOBS
        public async Task<IActionResult> ListBlobs(string containername)
        {
            List<BlobModel> models = await this.service.GetBlobsAsync(containername);
            ViewData["CONTAINERNAME"] = containername;
            return View(models);
        }

        public async Task<IActionResult> DeleteBlob(string nombreBlob, 
            string containerName)
        {
            await this.service.DeleteBlobAsync(containerName, nombreBlob);
            //REDIRECCIONAMOS A ListBlobs ENVIANDO EL CONTAINERNAME
            return RedirectToAction("ListBlobs",
                new { containername = containerName });
        }

        public IActionResult UploadBlob(string containerName)
        {
            ViewData["CONTAINERNAME"] = containerName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob
            (string containername, IFormFile file)
        {
            string filename = file.FileName;
            //SUBIMOS EL STREAM QUE VIENE EN file
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadBlobAsync
                    (containername, filename, stream);
            }
            //LO LLEVAMOS DE NUEVO A LA PAGINA PARA MOSTRAR
            //TODOS LOS BLOBS
            return RedirectToAction("ListBlobs",
                new { containername = containername });
        }
    }
}
