using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgendaManicure.Models
{
    public class Resena
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("calificacion")]
        public int Calificacion { get; set; }

        [BsonElement("comentarios")]
        public string Comentarios { get; set; }

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }

        [BsonElement("servicio_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ServicioId { get; set; }

        [BsonElement("usuario_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; }

        [BsonElement("creadoEn")]
        public DateTime CreadoEn { get; set; }
    }
}
