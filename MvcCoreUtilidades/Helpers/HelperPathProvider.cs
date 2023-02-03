namespace MvcCoreUtilidades.Helpers
{
    public enum Folders
    {
        Uploads, Imagenes, Documentos, Temporal
    }

    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnvironment;
        private string HostUrl;

        public HelperPathProvider
            (IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this.hostEnvironment = hostEnvironment;
            this.HostUrl = httpContextAccessor.HttpContext.Request.Host.Value;
        }

        public string GetWebHostUrl()
        {
            return "https://" + this.HostUrl + "/";
        }

        public string GetNameFolder(Folders folders)
        {
            if (folders == Folders.Uploads)
            {
                return "uploads";
            }else if (folders == Folders.Documentos)
            {
                return "documents";
            }else if (folders == Folders.Imagenes)
            {
                return "images";
            }else if (folders == Folders.Temporal)
            {
                return "temp";
            }
            return "";
        }

        //TENDREMOS UN METODO QUE NOS DEVOLVERA LA RUTA DEPENDIENDO
        //DE LA CARPETA SELECCIONADA
        public string GetMapPath(Folders folder, string filename)
        {
            string carpeta = "";
            if (folder == Folders.Uploads)
            {
                carpeta = "uploads";
            }else if (folder == Folders.Documentos)
            {
                carpeta = "documents";
            }else if (folder == Folders.Imagenes)
            {
                carpeta = "images";
            }else if (folder == Folders.Temporal)
            {
                carpeta = "temp";
            }
            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, filename);
            return path;
        }
    }
}
