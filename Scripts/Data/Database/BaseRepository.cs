using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly DatabaseConnection _dbConnection;

        protected BaseRepository()
        {
            _dbConnection = DatabaseConnection.Instance;
        }

        protected async Task<NpgsqlConnection> GetConnectionAsync()
        {
            return await Task.Run(() => _dbConnection.GetConnection());
        }

        protected async Task CloseConnectionAsync()
        {
            await Task.Run(() => _dbConnection.CloseConnection());
        }

        // Méthodes CRUD de base
        public abstract Task<T> GetByIdAsync(int id);
        public abstract Task<List<T>> GetAllAsync();
        public abstract Task<T> CreateAsync(T entity);
        public abstract Task<bool> UpdateAsync(T entity);
        public abstract Task<bool> DeleteAsync(int id);

        // Méthode utilitaire pour exécuter une requête SQL
        protected async Task<List<T>> ExecuteQueryAsync(string query, params NpgsqlParameter[] parameters)
        {
            var results = new List<T>();
            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(MapFromReader(reader));
                    }
                }
            }
            return results;
        }

        // Méthode abstraite pour mapper les résultats de la base de données vers l'objet
        protected abstract T MapFromReader(NpgsqlDataReader reader);
    }
} 