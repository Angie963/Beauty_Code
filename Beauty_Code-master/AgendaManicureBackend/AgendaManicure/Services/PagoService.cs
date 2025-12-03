using MongoDB.Driver;
using AgendaManicure.Models;
using Microsoft.Extensions.Options;

namespace AgendaManicure.Services
{
    public class PagoService
    {
        private readonly IMongoCollection<Pago> _pagosCollection;

        public PagoService(IOptions<MongoDBSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);

            var collectionName = mongoSettings.Value.Collections.Pagos ?? "pagos";
            _pagosCollection = database.GetCollection<Pago>(collectionName);
        }

        // GET ALL
        public async Task<List<Pago>> GetAllAsync()
        {
            return await _pagosCollection.Find(_ => true).ToListAsync();
        }

        // GET BY ID
        public async Task<Pago?> GetByIdAsync(string id)
        {
            return await _pagosCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        // CREATE
        public async Task CreateAsync(Pago pago)
        {
            pago.CreadoEn = pago.CreadoEn == default ? DateTime.UtcNow : pago.CreadoEn;
            await _pagosCollection.InsertOneAsync(pago);
        }

        // UPDATE
        public async Task UpdateAsync(string id, Pago pagoUpdate)
        {
            pagoUpdate.Id = id;
            await _pagosCollection.ReplaceOneAsync(p => p.Id == id, pagoUpdate);
        }

        // DELETE
        public async Task DeleteAsync(string id)
        {
            await _pagosCollection.DeleteOneAsync(p => p.Id == id);
        }

        // OPTIONAL: buscar pagos por usuario
        public async Task<List<Pago>> GetByUsuarioIdAsync(string usuarioId)
        {
            return await _pagosCollection.Find(p => p.UsuarioId == usuarioId).ToListAsync();
        }
    }
}
