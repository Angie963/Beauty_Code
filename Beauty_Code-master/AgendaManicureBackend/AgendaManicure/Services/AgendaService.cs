using MongoDB.Driver;
using AgendaManicure.Models;
using Microsoft.Extensions.Options;

namespace AgendaManicure.Services
{
    public class AgendaService
    {
        private readonly IMongoCollection<Cita> _agendaCollection;

        public AgendaService(IOptions<MongoDBSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var db = client.GetDatabase(mongoSettings.Value.DatabaseName);

            // toma el nombre de la colección desde appsettings.json -> Collections.Citas (valor "agenda")
            var collectionName = mongoSettings.Value.Collections.Citas;
            _agendaCollection = db.GetCollection<Cita>(collectionName);
        }

        public async Task<List<Cita>> GetAllAsync()
        {
            return await _agendaCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Cita?> GetByIdAsync(string id)
        {
            return await _agendaCollection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Cita>> GetByUsuarioIdAsync(string usuarioId)
        {
            return await _agendaCollection.Find(c => c.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task CreateAsync(Cita cita)
        {
            cita.CreadoEn = DateTime.UtcNow;
            await _agendaCollection.InsertOneAsync(cita);
        }

        public async Task<bool> UpdateAsync(string id, Cita citaUpdate)
        {
            citaUpdate.Id = id;
            var result = await _agendaCollection.ReplaceOneAsync(c => c.Id == id, citaUpdate);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _agendaCollection.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
