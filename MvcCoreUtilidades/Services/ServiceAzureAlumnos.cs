using Azure;
using Azure.Data.Tables;
using MvcCoreUtilidades.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MvcCoreUtilidades.Services
{
    public class ServiceAzureAlumnos
    {
        //NO LO VAMOS A CREAR EN EL CONSTRUCTOR
        //PORQUE NECESITAMOS EL TOKEN PARA ACCEDER 
        //A LOS DATOS DE DICHA TABLA
        private TableClient tableClient;
        private string urlApi;

        //TENDREMOS UN METODO PARA GENERAR EL TOKEN
        //DESDE EL API
        public ServiceAzureAlumnos(string urlApi)
        {
            this.urlApi = urlApi;
        }

        //METODO PARA RECUPERAR EL TOKEN A PARTIR DE UN CURSO
        //QUE RECIBIREMOS
        public async Task<string> GetTokenAsync(string curso)
        {
            string request = "/api/TableToken/GenerateToken/" + curso;
            using (WebClient client = new WebClient())
            {
                client.Headers["content-type"] = "application/json";
                Uri uri = new Uri(this.urlApi + request);
                //DESCARGAMOS TODO EL CONTENIDO JSON DE LA RESPUESTA
                string contenido =
                    await client.DownloadStringTaskAsync(uri);
                //CONVERTIMOS EL STRING A OBJETO JSON
                JObject jobject = JObject.Parse(contenido);
                string token = jobject.GetValue("token").ToString();
                return token;
            }
        }

        //TENDREMOS UN METODO QUE LEERA EL SERVICIO AZURE STORAGE
        //Y DEVOLVERA TODOS LOS ALUMNOS
        //PARA ACCEDER AL SERVICIO NECESITAMOS EL TOKEN DE ACCESO
        public async Task<List<Alumno>> GetAlumnosAsync(string token)
        {
            Uri uriToken = new Uri(token);
            //CON EL TOKEN ACCEDEMOS A LA TABLA
            this.tableClient = new TableClient(uriToken);
            //LEEMOS LOS DATOS
            List<Alumno> alumnos = new List<Alumno>();
            var consulta = this.tableClient.QueryAsync<Alumno>(filter: "");
            await foreach (Alumno al in consulta)
            {
                alumnos.Add(al);
            }
            return alumnos;
        }

        //METODO PARA ELIMINAR UN ALUMNO
        public async Task DeleteAlumnoAsync
            (string token, string partitionkey, string rowkey)
        {
            Uri uri = new Uri(token);
            //CREAMOS TABLE CLIENT
            this.tableClient = new TableClient(uri);
            //ELIMINAMOS AL ALUMNO
            await this.tableClient.DeleteEntityAsync(partitionkey, rowkey);
        }
    }
}
