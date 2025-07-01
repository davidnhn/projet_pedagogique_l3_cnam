using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public class QueteRepository : BaseRepository<Quete>
    {
        public override async Task<Quete> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM quete WHERE id_quete = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<List<Quete>> GetAllAsync()
        {
            var query = "SELECT * FROM quete";
            return await ExecuteQueryAsync(query);
        }

        public override async Task<Quete> CreateAsync(Quete quete)
        {
            var query = @"
                INSERT INTO quete (nom_quete, description_quete, etat_quete)
                VALUES (@nom, @description, @etat)
                RETURNING id_quete, nom_quete, description_quete, etat_quete";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@nom", quete.NomQuete),
                new NpgsqlParameter("@description", quete.DescriptionQuete ?? ""),
                new NpgsqlParameter("@etat", quete.EtatQuete ?? "Non commencé")
            };

            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(Quete quete)
        {
            var query = @"
                UPDATE quete 
                SET nom_quete = @nom, description_quete = @description, etat_quete = @etat
                WHERE id_quete = @id";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", quete.IdQuete),
                new NpgsqlParameter("@nom", quete.NomQuete),
                new NpgsqlParameter("@description", quete.DescriptionQuete ?? ""),
                new NpgsqlParameter("@etat", quete.EtatQuete ?? "Non commencé")
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
            var query = "DELETE FROM quete WHERE id_quete = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<Quete>> GetByEtatAsync(string etat)
        {
            var query = "SELECT * FROM quete WHERE etat_quete = @etat";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@etat", etat) };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<List<Quete>> GetByPersonnageIdAsync(int personnageId)
        {
            var query = @"
                SELECT q.* 
                FROM quete q 
                INNER JOIN accomplir a ON q.id_quete = a.id_quete 
                WHERE a.id_personnage = @personnageId";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@personnageId", personnageId) };
            return await ExecuteQueryAsync(query, parameters);
        }

        protected override Quete MapFromReader(NpgsqlDataReader reader)
        {
            return new Quete
            {
                IdQuete = reader.GetInt32(reader.GetOrdinal("id_quete")),
                NomQuete = reader.GetString(reader.GetOrdinal("nom_quete")),
                DescriptionQuete = reader.IsDBNull(reader.GetOrdinal("description_quete")) ? 
                    "" : reader.GetString(reader.GetOrdinal("description_quete")),
                EtatQuete = reader.IsDBNull(reader.GetOrdinal("etat_quete")) ? 
                    "Non commencé" : reader.GetString(reader.GetOrdinal("etat_quete"))
            };
        }
    }
} 