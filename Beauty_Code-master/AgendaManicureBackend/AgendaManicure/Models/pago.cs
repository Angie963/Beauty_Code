using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgendaManicure.Models
{
    public class Pago
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // Monto del pago
        [BsonElement("monto")]
        public decimal Monto { get; set; }

        // Método de pago (tarjeta, efectivo, transferencia...)
        [BsonElement("metodo")]
        public string Metodo { get; set; } = string.Empty;

        // Estado del pago (pendiente, completado, cancelado)
        [BsonElement("estado")]
        public string Estado { get; set; } = string.Empty;

        // Fecha del pago
        [BsonElement("fecha")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Fecha { get; set; }

        // REFERENCIA AL USUARIO
        [BsonElement("usuario_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UsuarioId { get; set; }

        // REFERENCIA AL SERVICIO (opcional)
        [BsonElement("servicio_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ServicioId { get; set; }

        // Campo opcional utilizado para pasarelas (ej: referencia de MercadoPago)
        [BsonElement("referencia_externa")]
        public string? ReferenciaExterna { get; set; }

        // Fecha de creación
        [BsonElement("creadoEn")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? CreadoEn { get; set; }
    }
}
