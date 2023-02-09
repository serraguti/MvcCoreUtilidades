using Azure;
using Azure.Data.Tables;

namespace MvcCoreUtilidades.Models
{
    public class Alumno : ITableEntity
    {
        public string IdAlumno { get; set; }
        public string Curso { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Nota { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
