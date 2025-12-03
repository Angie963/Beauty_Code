using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AgendaManicure.Models
{
    public class Categoria
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // nombre del campo en la colección: tipo_de_servicio
        [BsonElement("tipo_de_servicio")]
        public string TipoDeServicio { get; set; }

        [BsonElement("descripcion")]
        public string Descripcion { get; set; }

        [BsonElement("creadoEn")]
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}
