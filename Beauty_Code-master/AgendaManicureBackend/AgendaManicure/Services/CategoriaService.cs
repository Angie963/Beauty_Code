using MongoDB.Driver;
using AgendaManicure.Models;
using Microsoft.Extensions.Options;

namespace AgendaManicure.Services
{
    public class CategoriaService
    {
        private readonly IMongoCollection<Categoria> _categoriasCollection;

        public CategoriaService(IOptions<MongoDBSettings> mongoSettings)
        {
            // Crear el cliente
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var db = client.GetDatabase(mongoSettings.Value.DatabaseName);

            // Obtener el nombre de la colección si está definido en appsettings.json
            var collectionName = mongoSettings.Value.Collections?.Categorias ?? "categorias";
            _categoriasCollection = db.GetCollection<Categoria>(collectionName);

            // Crear índice único (no afecta si ya existe)
            var indexKeys = Builders<Categoria>.IndexKeys.Ascending(c => c.TipoDeServicio);
            var indexOptions = new CreateIndexOptions
            {
                Name = "idx_categoria_tipo",
                Unique = true,
                Sparse = true
            };

            _categoriasCollection.Indexes.CreateOne(
                new CreateIndexModel<Categoria>(indexKeys, indexOptions)
            );
        }

        // GET ALL
        public async Task<List<Categoria>> GetAllAsync() =>
            await _categoriasCollection.Find(_ => true).ToListAsync();

        // GET BY ID
        public async Task<Categoria> GetByIdAsync(string id) =>
            await _categoriasCollection.Find(c => c.Id == id).FirstOrDefaultAsync();

        // CREATE
        public async Task CreateAsync(Categoria categoria)
        {
            categoria.CreadoEn = DateTime.UtcNow;
            await _categoriasCollection.InsertOneAsync(categoria);
        }

        // UPDATE
        public async Task UpdateAsync(string id, Categoria categoriaUpdate)
        {
            categoriaUpdate.Id = id;
            await _categoriasCollection.ReplaceOneAsync(c => c.Id == id, categoriaUpdate);
        }

        // DELETE
        public async Task DeleteAsync(string id) =>
            await _categoriasCollection.DeleteOneAsync(c => c.Id == id);
    }
}
