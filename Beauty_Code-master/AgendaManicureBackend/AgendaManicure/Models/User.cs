using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgendaManicure.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [BsonElement("contrasena")]
        public string Contrasena { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("telefono")]
        public string Telefono { get; set; } = string.Empty;

        [BsonElement("tipo_documento")]
        public string TipoDocumento { get; set; } = string.Empty;

        [BsonElement("numero_documento")]
        public string NumeroDocumento { get; set; } = string.Empty;

        [BsonElement("roles")]
        public List<string> Roles { get; set; } = new List<string>();

        [BsonElement("creadoEn")]
        public DateTime CreadoEn { get; set; } = DateTime.Now;
    }
}

