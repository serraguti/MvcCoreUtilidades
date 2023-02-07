using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MvcCoreUtilidades.Models;

namespace MvcCoreUtilidades.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;

        public ServiceStorageBlobs(string azurekeys)
        {
            this.client = new BlobServiceClient(azurekeys);
        }

        //METODO PARA DEVOLVER TODOS LOS CONTENEDORES
        public async Task<List<string>> GetContainersAsync()
        {
            List<string> containers = new List<string>();
            await foreach (BlobContainerItem container in this.client.GetBlobContainersAsync())
            {
                containers.Add(container.Name);
            }
            return containers;
        }

        //METODO PARA CREAR UN CONTENEDOR
        public async Task CreateContainerAsync(string containerName)
        {
            //PARA CREAR UN CONTENEDOR NECESITAMOS EL NOMBRE E INDICAR
            //EL TIPO DE ACCESO A LOS BLOBS DEL CONTENEDOR
            //EL NOMBRE DEL CONTENEDOR SIEMPRE EN MINUSCULAS
            await this.client.CreateBlobContainerAsync(containerName.ToLower()
                , PublicAccessType.Blob);
        }

        //METODO PARA ELIMINAR UN CONTENEDOR
        public async Task DeleteContainerAsync(string containerName)
        {
            await this.client.DeleteBlobContainerAsync(containerName);
        }

        //METODO PARA RECUPERAR LOS BLOBS DE UN CONTAINER
        public async Task<List<BlobModel>> GetBlobsAsync(string containerName)
        {
            //CADA VEZ QUE TRABAJAMOS CON BLOBS NECESITAMOS UN CLIENT
            //DE BLOB EN SU CONTAINER
            //PARA CREAR ESTE OBJETO ES NECESARIO EL NOMBRE DEL CONTAINER
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            List<BlobModel> blobs = new List<BlobModel>();
            await foreach (BlobItem item in containerClient.GetBlobsAsync())
            {
                //EN LA INFORMACION DE UN BLOBITEM NO VIENE LA URL, SOLAMENTE
                //EL NOMBRE
                BlobClient blobClient = containerClient.GetBlobClient(item.Name);
                //YA TENEMOS LA URL PARA RECUPERARLA
                BlobModel model = new BlobModel();
                model.Nombre = item.Name;
                model.Url = blobClient.Uri.AbsoluteUri;
                blobs.Add(model);
            }
            return blobs;
        }

        //METODO PARA ELIMINAR BLOB
        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            //NECESITAMOS EL BLOB CONTAINER CLIENT PARA ELIMINAR DE SU INTERIOR
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            //ELIMINAMOS DEL CONTENEDOR POR EL NOMBRE DEL BLOB
            await containerClient.DeleteBlobAsync(blobName);
        }

        //METODO PARA SUBIR BLOB
        public async Task UploadBlobAsync(string containerName
            , string blobName, Stream stream)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            await containerClient.UploadBlobAsync(blobName, stream);
        }
    }
}
