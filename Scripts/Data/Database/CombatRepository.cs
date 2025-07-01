using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public class CombatRepository : BaseRepository<Combat>
    {
        public override async Task<Combat> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM combat WHERE id_combat = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<List<Combat>> GetAllAsync()
        {
            var query = "SELECT * FROM combat";
            return await ExecuteQueryAsync(query);
        }

        public override async Task<Combat> CreateAsync(Combat combat)
        {
            var query = @"
                INSERT INTO combat (resultat_combat, id_zone)
                VALUES (@resultat, @idZone)
                RETURNING id_combat, resultat_combat, id_zone";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@resultat", combat.ResultatCombat ?? "En cours"),
                new NpgsqlParameter("@idZone", combat.Zone?.IdZone ?? 0)
            };

            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(Combat combat)
        {
            var query = @"
                UPDATE combat 
                SET resultat_combat = @resultat, id_zone = @idZone
                WHERE id_combat = @id";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", combat.IdCombat),
                new NpgsqlParameter("@resultat", combat.ResultatCombat ?? "En cours"),
                new NpgsqlParameter("@idZone", combat.Zone?.IdZone ?? 0)
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
            var query = "DELETE FROM combat WHERE id_combat = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<Combat>> GetByZoneIdAsync(int zoneId)
        {
            var query = "SELECT * FROM combat WHERE id_zone = @zoneId";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@zoneId", zoneId) };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<List<Combat>> GetByResultatAsync(string resultat)
        {
            var query = "SELECT * FROM combat WHERE resultat_combat = @resultat";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@resultat", resultat) };
            return await ExecuteQueryAsync(query, parameters);
        }

        protected override Combat MapFromReader(NpgsqlDataReader reader)
        {
            return new Combat
            {
                IdCombat = reader.GetInt32(reader.GetOrdinal("id_combat")),
                ResultatCombat = reader.IsDBNull(reader.GetOrdinal("resultat_combat")) ? 
                    "En cours" : reader.GetString(reader.GetOrdinal("resultat_combat"))
                // Note: La relation Zone devra être chargée séparément
            };
        }
    }
} 