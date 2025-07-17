using Godot;
using Npgsql;
using System;

public partial class DatabaseManager : Node
{
    public static DatabaseManager Instance { get; private set; }

    private NpgsqlConnection _connection;

    public override void _EnterTree()
    {
        if (Instance == null)
        {
            Instance = this;
            ConnectToDatabase();
        }
        else
        {
            QueueFree(); 
        }
    }

    private void ConnectToDatabase()
    {
        var connectionString = "Host=localhost;Port=5432;Username=postgres;Password=example_password;Database=game_db;";

        try
        {
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
            GD.Print("Successfully connected to PostgreSQL.");
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Failed to connect to PostgreSQL: {ex.Message}");
        }
    }
    
    public bool PlayerExists(string pseudo)
    {
        if (_connection == null)
        {
            GD.PrintErr("Database connection is not available.");
            return false;
        }

        try
        {
            using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM Joueur WHERE pseudoJoueur = @pseudo", _connection))
            {
                cmd.Parameters.AddWithValue("pseudo", pseudo);
                var result = (long)cmd.ExecuteScalar();
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error checking if player exists: {ex.Message}");
            return false;
        }
    }

    public int CreatePlayer(string pseudo)
    {
        if (_connection == null)
        {
            GD.PrintErr("Database connection is not available.");
            return -1;
        }

        try
        {
            var query = "INSERT INTO Joueur (pseudoJoueur) VALUES (@pseudo) RETURNING idJoueur;";
            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("pseudo", pseudo);
                return (int)cmd.ExecuteScalar();
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error creating player: {ex.Message}");
            return -1;
        }
    }

    public int GetPlayerByName(string pseudo)
    {
        if (_connection == null)
        {
            GD.PrintErr("Database connection is not available.");
            return -1;
        }

        try
        {
            var query = "SELECT idJoueur FROM Joueur WHERE pseudoJoueur = @pseudo;";
            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("pseudo", pseudo);
                var result = cmd.ExecuteScalar();
                return result != null ? (int)result : -1;
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error getting player by name: {ex.Message}");
            return -1;
        }
    }

    public int GetCharacterByPlayerId(int idJoueur)
    {
        if (_connection == null)
        {
            GD.PrintErr("Database connection is not available.");
            return -1;
        }

        try
        {
            var query = "SELECT idPersonnage FROM Personnage WHERE idJoueur = @idJoueur;";
            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("idJoueur", idJoueur);
                var result = cmd.ExecuteScalar();
                return result != null ? (int)result : -1;
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error getting character by player id: {ex.Message}");
            return -1;
        }
    }

    public void UpdatePlayerCustomNames(int idJoueur, string nom1, string nom2, string nom3, string metier, string objet, string lieu, string extra1, string extra2, string extra3)
    {
        if (_connection == null)
        {
            GD.PrintErr("Database connection is not available.");
            return;
        }

        try
        {
            var query = @"
                UPDATE Joueur 
                SET 
                    nomPropre1 = @nom1, 
                    nomPropre2 = @nom2, 
                    nomPropre3 = @nom3, 
                    nomMetier = @metier, 
                    nomObjetInsolite = @objet, 
                    nomLieu = @lieu,
                    extraNom1 = @extra1,
                    extraNom2 = @extra2,
                    extraNom3 = @extra3
                WHERE idJoueur = @idJoueur;";
            
            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("idJoueur", idJoueur);
                cmd.Parameters.AddWithValue("nom1", nom1);
                cmd.Parameters.AddWithValue("nom2", nom2);
                cmd.Parameters.AddWithValue("nom3", nom3);
                cmd.Parameters.AddWithValue("metier", metier);
                cmd.Parameters.AddWithValue("objet", objet);
                cmd.Parameters.AddWithValue("lieu", lieu);
                cmd.Parameters.AddWithValue("extra1", extra1);
                cmd.Parameters.AddWithValue("extra2", extra2);
                cmd.Parameters.AddWithValue("extra3", extra3);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error updating player custom names: {ex.Message}");
        }
    }

    public int CreateCharacter(int idJoueur, string nomPersonnage, int idTypePersonnage)
    {
        if (_connection == null)
        {
            GD.PrintErr("Database connection is not available.");
            return -1;
        }

        try
        {
            var query = "INSERT INTO Personnage (idJoueur, nomPersonnage, idTypePersonnage, vie, experience, etatPersonnage) VALUES (@idJoueur, @nomPersonnage, @idTypePersonnage, 100, 0, 'normal') RETURNING idPersonnage;";
            using (var cmd = new NpgsqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("idJoueur", idJoueur);
                cmd.Parameters.AddWithValue("nomPersonnage", nomPersonnage);
                cmd.Parameters.AddWithValue("idTypePersonnage", idTypePersonnage);
                return (int)cmd.ExecuteScalar();
            }
        }
        catch (Exception ex)
        {
            GD.PrintErr($"Error creating character: {ex.Message}");
            return -1;
        }
    }

    public override void _ExitTree()
    {
        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
            GD.Print("Database connection closed.");
        }
    }
} 