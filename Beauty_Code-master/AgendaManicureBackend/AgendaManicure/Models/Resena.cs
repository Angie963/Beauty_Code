using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgendaManicure.Models
{
    public class Resena
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // Calificación 1 a 5
        [BsonElement("calificacion")]
        public int Calificacion { get; set; }

        [BsonElement("comentarios")]
        public string Comentarios { get; set; } = string.Empty;

        // Fecha de creación de la reseña
        [BsonElement("fecha")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Servicio reseñado
        [BsonElement("servicio_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ServicioId { get; set; } = null!;

        // Usuario que hizo la reseña
        [BsonElement("usuario_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; } = null!;

        // Timestamp interno
        [BsonElement("creadoEn")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}

