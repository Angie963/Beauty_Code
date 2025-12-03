using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgendaManicure.Models
{
    public class Cita
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        // fecha -> tipo date en MongoDB
        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }

        // hora -> string (ej. "14:30")
        [BsonElement("hora")]
        public string Hora { get; set; } = string.Empty;

        [BsonElement("estado")]
        public string Estado { get; set; } = string.Empty; // pendiente, aceptada, cancelada, etc.

        [BsonElement("pago_agenda")]
        public double PagoAgenda { get; set; } = 0.0;

        // referencia al usuario (objectId en Mongo)
        [BsonElement("usuario_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; } = null!;

        [BsonElement("creadoEn")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}
