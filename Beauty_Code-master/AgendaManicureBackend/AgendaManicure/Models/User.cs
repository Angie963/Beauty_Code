using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgendaManicure.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("contrasena")]
        public string Contrasena { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("telefono")]
        public string Telefono { get; set; }

        [BsonElement("tipo_documento")]
        public string TipoDocumento { get; set; }

        [BsonElement("numero_documento")]
        public string NumeroDocumento { get; set; }

        [BsonElement("roles")]
        public List<string> Roles { get; set; }

        [BsonElement("creadoEn")]
        public DateTime CreadoEn { get; set; }
    }
}
