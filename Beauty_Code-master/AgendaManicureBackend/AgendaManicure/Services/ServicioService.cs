using MongoDB.Driver;
using AgendaManicure.Models;
using Microsoft.Extensions.Options;

namespace AgendaManicure.Services
{
    public class ServicioService
    {
        private readonly IMongoCollection<Servicio> _serviciosCollection;
        private readonly CategoriaService _categoriaService;

        public ServicioService(IOptions<MongoDBSettings> mongoSettings, CategoriaService categoriaService)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);

            _serviciosCollection = database.GetCollection<Servicio>("servicios");

            // Inyectamos CategoriaService para poder validar
            _categoriaService = categoriaService;
        }

        // GET ALL
        public async Task<List<Servicio>> GetAllAsync() =>
            await _serviciosCollection.Find(_ => true).ToListAsync();

        // GET by ID
        public async Task<Servicio> GetByIdAsync(string id) =>
            await _serviciosCollection.Find(s => s.Id == id).FirstOrDefaultAsync();

        // CREATE (con validación de categoría)
        public async Task CreateAsync(Servicio servicio)
        {
            // 1. Verificar si la categoría existe
            var categoria = await _categoriaService.GetByIdAsync(servicio.CategoriaId);

            if (categoria == null)
                throw new Exception("La categoría no existe. No se puede crear el servicio.");

            // 2. Si existe, continuar
            servicio.CreadoEn = DateTime.UtcNow;
            servicio.Activo = true;

            await _serviciosCollection.InsertOneAsync(servicio);
        }

        // UPDATE
        public async Task UpdateAsync(string id, Servicio servicioUpdate)
        {
            servicioUpdate.Id = id;
            await _serviciosCollection.ReplaceOneAsync(s => s.Id == id, servicioUpdate);
        }

        // DELETE
        public async Task DeleteAsync(string id) =>
            await _serviciosCollection.DeleteOneAsync(s => s.Id == id);
    }
}
