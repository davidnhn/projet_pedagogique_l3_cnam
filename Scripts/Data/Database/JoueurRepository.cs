using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public class JoueurRepository : BaseRepository<Joueur>
    {
        public override async Task<Joueur> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM joueur WHERE id_joueur = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<List<Joueur>> GetAllAsync()
        {
            var query = "SELECT * FROM joueur";
            return await ExecuteQueryAsync(query);
        }

        public override async Task<Joueur> CreateAsync(Joueur joueur)
        {
            var query = @"
                INSERT INTO joueur (pseudo_joueur)
                VALUES (@pseudo)
                RETURNING id_joueur, pseudo_joueur";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@pseudo", joueur.PseudoJoueur)
            };

            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(Joueur joueur)
        {
            var query = @"
                UPDATE joueur 
                SET pseudo_joueur = @pseudo
                WHERE id_joueur = @id";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", joueur.IdJoueur),
                new NpgsqlParameter("@pseudo", joueur.PseudoJoueur)
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
            var query = "DELETE FROM joueur WHERE id_joueur = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        protected override Joueur MapFromReader(NpgsqlDataReader reader)
        {
            return new Joueur
            {
                IdJoueur = reader.GetInt32(reader.GetOrdinal("id_joueur")),
                PseudoJoueur = reader.GetString(reader.GetOrdinal("pseudo_joueur"))
            };
        }
    }
} 