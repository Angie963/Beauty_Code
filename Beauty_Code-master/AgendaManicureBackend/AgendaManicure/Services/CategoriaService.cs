using MongoDB.Driver;
using AgendaManicure.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaManicure.Services
{
    public class CategoriaService
    {
        private readonly IMongoCollection<Categoria> _categorias;

        public CategoriaService(IOptions<MongoDBSettings> opts)
        {
            var client = new MongoClient(opts.Value.ConnectionString);
            var db = client.GetDatabase(opts.Value.DatabaseName);
            var collectionName = opts.Value.Collections.Categorias ?? "Categorias";
            _categorias = db.GetCollection<Categoria>(collectionName);

            // índice único por nombre (opcional)
            var indexKeys = Builders<Categoria>.IndexKeys.Ascending(c => c.Nombre);
            var indexOptions = new CreateIndexOptions { Unique = true };
            _categorias.Indexes.CreateOne(new CreateIndexModel<Categoria>(indexKeys, indexOptions));
        }

        public async Task<List<Categoria>> GetAllAsync()
        {
            return await _categorias.Find(_ => true).ToListAsync();
        }

        public async Task<Categoria> GetByIdAsync(string id)
        {
            return await _categorias.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Categoria> GetByNameAsync(string nombre)
        {
            return await _categorias.Find(c => c.Nombre == nombre).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Categoria categoria)
        {
            categoria.CreadoEn = DateTime.UtcNow;
            await _categorias.InsertOneAsync(categoria);
        }

        public async Task UpdateAsync(string id, Categoria categoriaIn)
        {
            categoriaIn.Id = id;
            await _categorias.ReplaceOneAsync(c => c.Id == id, categoriaIn);
        }

        public async Task DeleteAsync(string id)
        {
            await _categorias.DeleteOneAsync(c => c.Id == id);
        }
    }
}
