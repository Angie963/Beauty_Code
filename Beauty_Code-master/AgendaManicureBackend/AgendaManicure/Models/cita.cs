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

        [BsonElement("fecha")]
        public DateTime Fecha { get; set; }

        [BsonElement("hora")]
        public string Hora { get; set; } = string.Empty;

        [BsonElement("servicio_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ServicioId { get; set; } = null!;

        [BsonElement("empleado_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EmpleadoId { get; set; } = null!;

        [BsonElement("precio_servicio")]
        public double PrecioServicio { get; set; }

        [BsonElement("estado")]
        public string Estado { get; set; } = "pendiente";

        [BsonElement("pago_agenda")]
        public double PagoAgenda { get; set; } = 0.0;

        [BsonElement("usuario_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; } = null!;

        [BsonElement("creadoEn")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}

