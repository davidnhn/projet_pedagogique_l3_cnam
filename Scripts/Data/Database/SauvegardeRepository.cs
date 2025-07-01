using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public class SauvegardeRepository : BaseRepository<Sauvegarde>
    {
        public override async Task<Sauvegarde> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM sauvegarde WHERE id_sauvegarde = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<List<Sauvegarde>> GetAllAsync()
        {
            var query = "SELECT * FROM sauvegarde";
            return await ExecuteQueryAsync(query);
        }

        public override async Task<Sauvegarde> CreateAsync(Sauvegarde sauvegarde)
        {
            var query = @"
                INSERT INTO sauvegarde (date_sauvegarde, position_x_joueur, position_y_joueur, id_joueur, id_zone)
                VALUES (@date, @posX, @posY, @idJoueur, @idZone)
                RETURNING id_sauvegarde, date_sauvegarde, position_x_joueur, position_y_joueur, id_joueur, id_zone";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@date", sauvegarde.DateSauvegarde),
                new NpgsqlParameter("@posX", sauvegarde.PositionXJoueur),
                new NpgsqlParameter("@posY", sauvegarde.PositionYJoueur),
                new NpgsqlParameter("@idJoueur", 1), // Valeur par défaut, à ajuster selon vos besoins
                new NpgsqlParameter("@idZone", sauvegarde.Zone?.IdZone ?? 0)
            };

            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(Sauvegarde sauvegarde)
        {
            var query = @"
                UPDATE sauvegarde 
                SET date_sauvegarde = @date, position_x_joueur = @posX, position_y_joueur = @posY,
                    id_joueur = @idJoueur, id_zone = @idZone
                WHERE id_sauvegarde = @id";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", sauvegarde.IdSauvegarde),
                new NpgsqlParameter("@date", sauvegarde.DateSauvegarde),
                new NpgsqlParameter("@posX", sauvegarde.PositionXJoueur),
                new NpgsqlParameter("@posY", sauvegarde.PositionYJoueur),
                new NpgsqlParameter("@idJoueur", 1), // Valeur par défaut, à ajuster selon vos besoins
                new NpgsqlParameter("@idZone", sauvegarde.Zone?.IdZone ?? 0)
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
            var query = "DELETE FROM sauvegarde WHERE id_sauvegarde = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<Sauvegarde>> GetByJoueurIdAsync(int joueurId)
        {
            var query = "SELECT * FROM sauvegarde WHERE id_joueur = @idJoueur ORDER BY date_sauvegarde DESC";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@idJoueur", joueurId) };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<Sauvegarde> GetDerniereSauvegardeByJoueurIdAsync(int joueurId)
        {
            var query = @"
                SELECT * FROM sauvegarde 
                WHERE id_joueur = @idJoueur 
                ORDER BY date_sauvegarde DESC 
                LIMIT 1";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@idJoueur", joueurId) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        protected override Sauvegarde MapFromReader(NpgsqlDataReader reader)
        {
            return new Sauvegarde
            {
                IdSauvegarde = reader.GetInt32(reader.GetOrdinal("id_sauvegarde")),
                DateSauvegarde = reader.GetDateTime(reader.GetOrdinal("date_sauvegarde")),
                PositionXJoueur = reader.GetInt32(reader.GetOrdinal("position_x_joueur")),
                PositionYJoueur = reader.GetInt32(reader.GetOrdinal("position_y_joueur"))
                // Note: Les relations Zone et Personnage devront être chargées séparément
            };
        }
    }
} 