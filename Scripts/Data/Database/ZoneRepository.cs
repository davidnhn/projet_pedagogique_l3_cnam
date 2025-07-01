using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public class ZoneRepository : BaseRepository<Zone>
    {
        public override async Task<Zone> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM zone WHERE id_zone = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<List<Zone>> GetAllAsync()
        {
            var query = "SELECT * FROM zone";
            return await ExecuteQueryAsync(query);
        }

        public override async Task<Zone> CreateAsync(Zone zone)
        {
            var query = @"
                INSERT INTO zone (nom_zone, description_zone)
                VALUES (@nom, @description)
                RETURNING id_zone, nom_zone, description_zone";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@nom", zone.NomZone),
                new NpgsqlParameter("@description", zone.DescriptionZone ?? "")
            };

            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(Zone zone)
        {
            var query = @"
                UPDATE zone 
                SET nom_zone = @nom, description_zone = @description
                WHERE id_zone = @id";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", zone.IdZone),
                new NpgsqlParameter("@nom", zone.NomZone),
                new NpgsqlParameter("@description", zone.DescriptionZone ?? "")
            };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM zone WHERE id_zone = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<Zone>> GetByNomAsync(string nom)
        {
            var query = "SELECT * FROM zone WHERE nom_zone ILIKE @nom";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@nom", $"%{nom}%") };
            return await ExecuteQueryAsync(query, parameters);
        }

        protected override Zone MapFromReader(NpgsqlDataReader reader)
        {
            return new Zone
            {
                IdZone = reader.GetInt32(reader.GetOrdinal("id_zone")),
                NomZone = reader.GetString(reader.GetOrdinal("nom_zone")),
                DescriptionZone = reader.IsDBNull(reader.GetOrdinal("description_zone")) ? 
                    "" : reader.GetString(reader.GetOrdinal("description_zone"))
            };
        }
    }
} 