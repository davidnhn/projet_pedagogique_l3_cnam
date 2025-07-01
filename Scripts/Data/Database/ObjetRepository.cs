using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public class ObjetRepository : BaseRepository<Objet>
    {
        public override async Task<Objet> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM objet WHERE id_objet = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<List<Objet>> GetAllAsync()
        {
            var query = "SELECT * FROM objet";
            return await ExecuteQueryAsync(query);
        }

        public override async Task<Objet> CreateAsync(Objet objet)
        {
            var query = @"
                INSERT INTO objet (nom_objet, description_nom_objet, effet)
                VALUES (@nom, @description, @effet)
                RETURNING id_objet, nom_objet, description_nom_objet, effet";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@nom", objet.NomObjet),
                new NpgsqlParameter("@description", objet.DescriptionNomObjet ?? ""),
                new NpgsqlParameter("@effet", objet.Effet ?? "")
            };

            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(Objet objet)
        {
            var query = @"
                UPDATE objet 
                SET nom_objet = @nom, description_nom_objet = @description, effet = @effet
                WHERE id_objet = @id";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", objet.IdObjet),
                new NpgsqlParameter("@nom", objet.NomObjet),
                new NpgsqlParameter("@description", objet.DescriptionNomObjet ?? ""),
                new NpgsqlParameter("@effet", objet.Effet ?? "")
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
            var query = "DELETE FROM objet WHERE id_objet = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<Objet>> GetByEffetAsync(string effet)
        {
            var query = "SELECT * FROM objet WHERE effet ILIKE @effet";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@effet", $"%{effet}%") };
            return await ExecuteQueryAsync(query, parameters);
        }

        protected override Objet MapFromReader(NpgsqlDataReader reader)
        {
            return new Objet
            {
                IdObjet = reader.GetInt32(reader.GetOrdinal("id_objet")),
                NomObjet = reader.GetString(reader.GetOrdinal("nom_objet")),
                DescriptionNomObjet = reader.IsDBNull(reader.GetOrdinal("description_nom_objet")) ? 
                    "" : reader.GetString(reader.GetOrdinal("description_nom_objet")),
                Effet = reader.IsDBNull(reader.GetOrdinal("effet")) ? 
                    "" : reader.GetString(reader.GetOrdinal("effet"))
            };
        }
    }
} 