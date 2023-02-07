using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace MvcCoreUtilidades.Services
{
    public class ServiceStorageFiles
    {
        //TODO FUNCIONA MEDIANTE RECURSOS COMPARTIDOS
        //NECESITAMOS EL NOMBRE DE NUESTRO RECURSO
        private ShareDirectoryClient root;

        //NECESITAMOS DE UNAS KEYS PARA PODER
        //ACCEDER AL SERVICIO
        public ServiceStorageFiles(string keys)
        {
            //LA MAYORIA DE SERVICIOS TRABAJAN CON UN CLIENTE
            //QUE NOS DA ACCESO A LOS RECURSOS
            ShareClient client =
                new ShareClient(keys, "ejemplofiles");
            //ACCEDEMOS A LA RUTA DE DICHO DIRECTORIO
            this.root = client.GetRootDirectoryClient();
        }

        //METODO PARA RECUPERAR TODOS LOS FICHEROS DE AZURE FILES
        public async Task<List<string>> GetFilesAsync()
        {
            List<string> files = new List<string>();
            //RECORREMOS TODOS LOS FICHEROS/DIRECTORIOS
            await foreach (ShareFileItem item in this.root.GetFilesAndDirectoriesAsync())
            {
                files.Add(item.Name);
            }
            return files;
        }

        //METODO PARA LEER FILES
        public async Task<string> ReadFileAsync(string fileName)
        {
            ShareFileClient file = this.root.GetFileClient(fileName);
            //DESCARGAMOS EL FICHERO EN MEMORIA
            ShareFileDownloadInfo data = await file.DownloadAsync();
            //DENTRO DE DATA Y SU VALUE, TENEMOS EL CONTENIDO
            //DICHO CONTENIDO NOS LO OFRECE EN FLUJO DE DATOS
            Stream stream = data.Content;
            //DEBEMOS LEER EL CONTENIDO Y EXTRAER LOS DATOS STRING
            string contenido = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                contenido = await reader.ReadToEndAsync();
            }
            return contenido;
        }

        //METODO PARA SUBIR FILES
        public async Task UploadFileAsync(string fileName
            , Stream stream)
        {
            ShareFileClient file = this.root.GetFileClient(fileName);
            //DEBEMOS CREAR EL FICHERO INDICANDO EL TAMAÑO
            await file.CreateAsync(stream.Length);
            //SUBIMOS LOS DATOS AL FICHERO CREADO
            await file.UploadAsync(stream);
        }

        //METODO PARA ELIMINAR FILE
        public async Task DeleteFileAsync(string fileName)
        {
            ShareFileClient file = this.root.GetFileClient(fileName);
            await file.DeleteAsync();
        }
    }
}
