using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgendaManicure.Models
{
    public class Servicio
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("descripcion")]
        public string Descripcion { get; set; }

        [BsonElement("precio")]
        public double Precio { get; set; }

        [BsonElement("duracionMinutos")]
        public int DuracionMinutos { get; set; }

        [BsonElement("categoria_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoriaId { get; set; }

        [BsonElement("activo")]
        public bool Activo { get; set; } = true;

        [BsonElement("creadoEn")]
        public DateTime CreadoEn { get; set; }
    }
}
