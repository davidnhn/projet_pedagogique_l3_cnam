using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public class BotRepository : BaseRepository<Bot>
    {
        public override async Task<Bot> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM bot WHERE id_bot = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<List<Bot>> GetAllAsync()
        {
            var query = "SELECT * FROM bot";
            return await ExecuteQueryAsync(query);
        }

        public override async Task<Bot> CreateAsync(Bot bot)
        {
            var query = @"
                INSERT INTO bot (niveau_bot, dialogue_bot, antagoniste_bot, nom_bot)
                VALUES (@niveau, @dialogue, @antagoniste, @nom)
                RETURNING id_bot, niveau_bot, dialogue_bot, antagoniste_bot, nom_bot";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@niveau", bot.NiveauBot ?? ""),
                new NpgsqlParameter("@dialogue", bot.DialogueBot ?? ""),
                new NpgsqlParameter("@antagoniste", bot.AntagonisteBoot ?? ""),
                new NpgsqlParameter("@nom", bot.NomBot ?? "")
            };

            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(Bot bot)
        {
            var query = @"
                UPDATE bot 
                SET niveau_bot = @niveau, dialogue_bot = @dialogue, 
                    antagoniste_bot = @antagoniste, nom_bot = @nom
                WHERE id_bot = @id";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", bot.IdBot),
                new NpgsqlParameter("@niveau", bot.NiveauBot ?? ""),
                new NpgsqlParameter("@dialogue", bot.DialogueBot ?? ""),
                new NpgsqlParameter("@antagoniste", bot.AntagonisteBoot ?? ""),
                new NpgsqlParameter("@nom", bot.NomBot ?? "")
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
            var query = "DELETE FROM bot WHERE id_bot = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<Bot>> GetByNiveauAsync(string niveau)
        {
            var query = "SELECT * FROM bot WHERE niveau_bot = @niveau";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@niveau", niveau) };
            return await ExecuteQueryAsync(query, parameters);
        }

        public async Task<List<Bot>> GetAntagonistesAsync()
        {
            var query = "SELECT * FROM bot WHERE antagoniste_bot = 'Oui'";
            return await ExecuteQueryAsync(query);
        }

        protected override Bot MapFromReader(NpgsqlDataReader reader)
        {
            return new Bot
            {
                IdBot = reader.GetInt32(reader.GetOrdinal("id_bot")),
                NiveauBot = reader.IsDBNull(reader.GetOrdinal("niveau_bot")) ? 
                    "" : reader.GetString(reader.GetOrdinal("niveau_bot")),
                DialogueBot = reader.IsDBNull(reader.GetOrdinal("dialogue_bot")) ? 
                    "" : reader.GetString(reader.GetOrdinal("dialogue_bot")),
                AntagonisteBoot = reader.IsDBNull(reader.GetOrdinal("antagoniste_bot")) ? 
                    "" : reader.GetString(reader.GetOrdinal("antagoniste_bot")),
                NomBot = reader.IsDBNull(reader.GetOrdinal("nom_bot")) ? 
                    "" : reader.GetString(reader.GetOrdinal("nom_bot"))
            };
        }
    }
} 