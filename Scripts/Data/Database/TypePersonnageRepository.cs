using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public class TypePersonnageRepository : BaseRepository<TypePersonnage>
    {
        public override async Task<TypePersonnage> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM typepersonnage WHERE id_type_personnage = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<List<TypePersonnage>> GetAllAsync()
        {
            var query = "SELECT * FROM typepersonnage";
            return await ExecuteQueryAsync(query);
        }

        public override async Task<TypePersonnage> CreateAsync(TypePersonnage typePersonnage)
        {
            var query = @"
                INSERT INTO typepersonnage (nom_type_personnage, description_type_personnage)
                VALUES (@nom, @description)
                RETURNING id_type_personnage, nom_type_personnage, description_type_personnage";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@nom", typePersonnage.NomTypePersonnage),
                new NpgsqlParameter("@description", typePersonnage.DescriptionTypePersonnage ?? "")
            };

            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(TypePersonnage typePersonnage)
        {
            var query = @"
                UPDATE typepersonnage 
                SET nom_type_personnage = @nom, description_type_personnage = @description
                WHERE id_type_personnage = @id";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", typePersonnage.IdTypePersonnage),
                new NpgsqlParameter("@nom", typePersonnage.NomTypePersonnage),
                new NpgsqlParameter("@description", typePersonnage.DescriptionTypePersonnage ?? "")
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
            var query = "DELETE FROM typepersonnage WHERE id_type_personnage = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<TypePersonnage> GetByNomAsync(string nom)
        {
            var query = "SELECT * FROM typepersonnage WHERE nom_type_personnage = @nom";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@nom", nom) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        protected override TypePersonnage MapFromReader(NpgsqlDataReader reader)
        {
            return new TypePersonnage
            {
                IdTypePersonnage = reader.GetInt32(reader.GetOrdinal("id_type_personnage")),
                NomTypePersonnage = reader.GetString(reader.GetOrdinal("nom_type_personnage")),
                DescriptionTypePersonnage = reader.IsDBNull(reader.GetOrdinal("description_type_personnage")) ? 
                    "" : reader.GetString(reader.GetOrdinal("description_type_personnage"))
            };
        }
    }
} 