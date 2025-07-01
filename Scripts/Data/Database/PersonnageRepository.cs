using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public class PersonnageRepository : BaseRepository<Personnage>
    {
        public override async Task<Personnage> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM personnage WHERE id_personnage = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<List<Personnage>> GetAllAsync()
        {
            var query = "SELECT * FROM personnage";
            return await ExecuteQueryAsync(query);
        }

        public override async Task<Personnage> CreateAsync(Personnage personnage)
        {
            var query = @"
                INSERT INTO personnage (vie, experience, nom_personnage, etat_personnage, id_joueur, id_type_personnage)
                VALUES (@vie, @experience, @nom, @etat, @idJoueur, @idType)
                RETURNING id_personnage, vie, experience, nom_personnage, etat_personnage, id_joueur, id_type_personnage";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@vie", personnage.Vie),
                new NpgsqlParameter("@experience", personnage.Experience),
                new NpgsqlParameter("@nom", personnage.NomPersonnage),
                new NpgsqlParameter("@etat", personnage.EtatPersonnage ?? ""),
                new NpgsqlParameter("@idJoueur", 1), // Valeur par défaut, à ajuster selon vos besoins
                new NpgsqlParameter("@idType", personnage.TypePersonnage?.IdTypePersonnage ?? 1)
            };

            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(Personnage personnage)
        {
            var query = @"
                UPDATE personnage 
                SET vie = @vie, experience = @experience, nom_personnage = @nom, 
                    etat_personnage = @etat, id_type_personnage = @idType
                WHERE id_personnage = @id";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", personnage.IdPersonnage),
                new NpgsqlParameter("@vie", personnage.Vie),
                new NpgsqlParameter("@experience", personnage.Experience),
                new NpgsqlParameter("@nom", personnage.NomPersonnage),
                new NpgsqlParameter("@etat", personnage.EtatPersonnage ?? ""),
                new NpgsqlParameter("@idType", personnage.TypePersonnage?.IdTypePersonnage ?? 1)
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
            var query = "DELETE FROM personnage WHERE id_personnage = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<Personnage>> GetByJoueurIdAsync(int joueurId)
        {
            var query = "SELECT * FROM personnage WHERE id_joueur = @idJoueur";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@idJoueur", joueurId) };
            return await ExecuteQueryAsync(query, parameters);
        }

        protected override Personnage MapFromReader(NpgsqlDataReader reader)
        {
            return new Personnage
            {
                IdPersonnage = reader.GetInt32(reader.GetOrdinal("id_personnage")),
                Vie = reader.GetInt32(reader.GetOrdinal("vie")),
                Experience = reader.GetInt32(reader.GetOrdinal("experience")),
                NomPersonnage = reader.GetString(reader.GetOrdinal("nom_personnage")),
                EtatPersonnage = reader.GetString(reader.GetOrdinal("etat_personnage"))
                // Note: Les relations Joueur et TypePersonnage devront être chargées séparément
            };
        }
    }
} 