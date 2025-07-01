using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JeuVideo.Data.Database
{
    public class InventaireRepository : BaseRepository<Inventaire>
    {
        public override async Task<Inventaire> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM inventaire WHERE id_inventaire = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };
            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<List<Inventaire>> GetAllAsync()
        {
            var query = "SELECT * FROM inventaire";
            return await ExecuteQueryAsync(query);
        }

        public override async Task<Inventaire> CreateAsync(Inventaire inventaire)
        {
            var query = @"
                INSERT INTO inventaire (taille_max_inventaire)
                VALUES (@tailleMax)
                RETURNING id_inventaire, taille_max_inventaire";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@tailleMax", inventaire.TailleMaxInventaire)
            };

            var results = await ExecuteQueryAsync(query, parameters);
            return results.FirstOrDefault();
        }

        public override async Task<bool> UpdateAsync(Inventaire inventaire)
        {
            var query = @"
                UPDATE inventaire 
                SET taille_max_inventaire = @tailleMax
                WHERE id_inventaire = @id";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@id", inventaire.IdInventaire),
                new NpgsqlParameter("@tailleMax", inventaire.TailleMaxInventaire)
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
            var query = "DELETE FROM inventaire WHERE id_inventaire = @id";
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@id", id) };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<Objet>> GetObjetsByInventaireIdAsync(int inventaireId)
        {
            var query = @"
                SELECT o.*, s.quantite_stocker 
                FROM objet o
                INNER JOIN stocker s ON o.id_objet = s.id_objet
                WHERE s.id_inventaire = @inventaireId";
            
            var parameters = new NpgsqlParameter[] { new NpgsqlParameter("@inventaireId", inventaireId) };
            
            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var objets = new List<Objet>();
                    while (await reader.ReadAsync())
                    {
                        objets.Add(new Objet
                        {
                            IdObjet = reader.GetInt32(reader.GetOrdinal("id_objet")),
                            NomObjet = reader.GetString(reader.GetOrdinal("nom_objet")),
                            DescriptionNomObjet = reader.IsDBNull(reader.GetOrdinal("description_nom_objet")) ? 
                                "" : reader.GetString(reader.GetOrdinal("description_nom_objet")),
                            Effet = reader.IsDBNull(reader.GetOrdinal("effet")) ? 
                                "" : reader.GetString(reader.GetOrdinal("effet"))
                        });
                    }
                    return objets;
                }
            }
        }

        public async Task<bool> AjouterObjetAsync(int inventaireId, int objetId, int quantite)
        {
            var query = @"
                INSERT INTO stocker (id_inventaire, id_objet, quantite_stocker)
                VALUES (@inventaireId, @objetId, @quantite)
                ON CONFLICT (id_inventaire, id_objet) 
                DO UPDATE SET quantite_stocker = stocker.quantite_stocker + @quantite";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@inventaireId", inventaireId),
                new NpgsqlParameter("@objetId", objetId),
                new NpgsqlParameter("@quantite", quantite)
            };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> RetirerObjetAsync(int inventaireId, int objetId, int quantite)
        {
            var query = @"
                UPDATE stocker 
                SET quantite_stocker = quantite_stocker - @quantite
                WHERE id_inventaire = @inventaireId AND id_objet = @objetId";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@inventaireId", inventaireId),
                new NpgsqlParameter("@objetId", objetId),
                new NpgsqlParameter("@quantite", quantite)
            };

            using (var connection = await GetConnectionAsync())
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                var result = await command.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        protected override Inventaire MapFromReader(NpgsqlDataReader reader)
        {
            return new Inventaire
            {
                IdInventaire = reader.GetInt32(reader.GetOrdinal("id_inventaire")),
                TailleMaxInventaire = reader.GetInt32(reader.GetOrdinal("taille_max_inventaire"))
            };
        }
    }
} 