using MongoDB.Driver;
using AgendaManicure.Models;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using BCrypt.Net;

namespace AgendaManicure.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IOptions<MongoDBSettings> mongoSettings)
        {
            var client = new MongoClient(mongoSettings.Value.ConnectionString);
            var database = client.GetDatabase(mongoSettings.Value.DatabaseName);

            // Usar el nombre de la colección desde appsettings.json
            var collectionName = mongoSettings.Value.Collections.Users;
            _usersCollection = database.GetCollection<User>(collectionName);

            // Crear índice único en email
            var indexKeys = Builders<User>.IndexKeys.Ascending(u => u.Email);
            var indexOptions = new CreateIndexOptions { Unique = true };
            _usersCollection.Indexes.CreateOne(new CreateIndexModel<User>(indexKeys, indexOptions));
        }

        // GET ALL
        public async Task<List<User>> GetAllAsync()
        {
            return await _usersCollection.Find(_ => true).ToListAsync();
        }

        // GET BY ID
        public async Task<User> GetByIdAsync(string id)
        {
            return await _usersCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        // CREATE
        public async Task CreateAsync(User user)
        {
            // Hashear contraseña
            user.Contrasena = BCrypt.Net.BCrypt.HashPassword(user.Contrasena);

            user.CreadoEn = DateTime.UtcNow;

            await _usersCollection.InsertOneAsync(user);
        }

        // UPDATE
        public async Task UpdateAsync(string id, User updateUser)
        {
            updateUser.Id = id;

            // Si actualiza contraseña -> volver a hashear
            if (!string.IsNullOrEmpty(updateUser.Contrasena))
            {
                updateUser.Contrasena = BCrypt.Net.BCrypt.HashPassword(updateUser.Contrasena);
            }

            await _usersCollection.ReplaceOneAsync(u => u.Id == id, updateUser);
        }

        // DELETE
        public async Task DeleteAsync(string id)
        {
            await _usersCollection.DeleteOneAsync(u => u.Id == id);
        }
    }
}
