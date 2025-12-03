using MongoDB.Driver;
using AgendaManicure.Models;
using Microsoft.Extensions.Options;

namespace AgendaManicure.Services
{
    public class ResenasService
    {
        private readonly IMongoCollection<Resena> _resenasCollection;
        private readonly ServicioService _servicioService;
        private readonly UserService _userService;

        public ResenasService(
            IOptions<MongoDBSettings> mongoSettings,
            ServicioService servicioService,
            UserService userService)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var db = client.GetDatabase(mongoSettings.Value.DatabaseName);

            var collectionName = mongoSettings.Value.Collections?.Resenas ?? "resenas";
            _resenasCollection = db.GetCollection<Resena>(collectionName);

            _servicioService = servicioService;
            _userService = userService;

            // índices (si quieres que se creen desde el servicio)
            var idx1 = Builders<Resena>.IndexKeys.Ascending(r => r.ServicioId);
            var idx2 = Builders<Resena>.IndexKeys.Ascending(r => r.UsuarioId);
            _resenasCollection.Indexes.CreateOne(new CreateIndexModel<Resena>(idx1, new CreateIndexOptions { Name = "idx_resena_servicio" }));
            _resenasCollection.Indexes.CreateOne(new CreateIndexModel<Resena>(idx2, new CreateIndexOptions { Name = "idx_resena_usuario" }));
        }

        public async Task<List<Resena>> GetAllAsync() =>
            await _resenasCollection.Find(_ => true).ToListAsync();

        public async Task<Resena> GetByIdAsync(string id) =>
            await _resenasCollection.Find(r => r.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Resena resena)
        {
            // Validaciones básicas
            if (resena.Calificacion < 1 || resena.Calificacion > 5)
                throw new ArgumentException("calificacion debe ser entre 1 y 5");

            // Validar existencia del servicio
            var servicio = await _servicioService.GetByIdAsync(resena.ServicioId);
            if (servicio == null)
                throw new ArgumentException("El servicio indicado no existe");

            // Validar existencia del usuario
            var usuario = await _userService.GetByIdAsync(resena.UsuarioId);
            if (usuario == null)
                throw new ArgumentException("El usuario indicado no existe");

            // Fechas
            if (resena.Fecha == default) resena.Fecha = DateTime.UtcNow;
            resena.CreadoEn = DateTime.UtcNow;

            await _resenasCollection.InsertOneAsync(resena);
        }

        public async Task UpdateAsync(string id, Resena resenaUpdate)
        {
            // Si cambian referencias, validarlas
            if (!string.IsNullOrWhiteSpace(resenaUpdate.ServicioId))
            {
                var serv = await _servicioService.GetByIdAsync(resenaUpdate.ServicioId);
                if (serv == null) throw new ArgumentException("El servicio indicado no existe");
            }

            if (!string.IsNullOrWhiteSpace(resenaUpdate.UsuarioId))
            {
                var usr = await _userService.GetByIdAsync(resenaUpdate.UsuarioId);
                if (usr == null) throw new ArgumentException("El usuario indicado no existe");
            }

            resenaUpdate.Id = id;
            await _resenasCollection.ReplaceOneAsync(r => r.Id == id, resenaUpdate);
        }

        public async Task DeleteAsync(string id) =>
            await _resenasCollection.DeleteOneAsync(r => r.Id == id);
    }
}
