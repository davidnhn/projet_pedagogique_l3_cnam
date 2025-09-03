using Godot;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

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
		// Load configuration from environment variables or optional .env file under BaseDeDonnees
		var envFromFile = LoadDotEnv("res://BaseDeDonnees/.env");

		string dbHost = GetConfigValue("DB_HOST", envFromFile, "192.168.240.136");
		string dbPort = GetConfigValue("DB_PORT", envFromFile, "5432");
		string dbUsername = GetConfigValue("DB_USER", envFromFile, "postgres");
		string dbPassword = GetConfigValue("DB_PASSWORD", envFromFile, "example_password");
		string dbName = GetConfigValue("DB_NAME", envFromFile, "game_db");

		GD.Print($"DB config → Host={dbHost}, Port={dbPort}, User={dbUsername}, Db={dbName}");

		var connectionString = new NpgsqlConnectionStringBuilder
		{
			Host = dbHost,
			Port = int.TryParse(dbPort, out var portValue) ? portValue : 5432,
			Username = dbUsername,
			Password = dbPassword,
			Database = dbName,
			Timeout = 10,
			CommandTimeout = 10,
			KeepAlive = 30,
			Pooling = true,
			MaxPoolSize = 20,
			SslMode = SslMode.Disable
		}.ToString();

		try
		{
			_connection = new NpgsqlConnection(connectionString);
			_connection.Open();
			GD.Print($"Successfully connected to PostgreSQL at {dbHost}:{dbPort}/{dbName} as {dbUsername}.");
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Failed to connect to PostgreSQL at {dbHost}:{dbPort}. Error: {ex.Message}");
		}
	}

	private bool EnsureConnection()
	{
		try
		{
			if (_connection == null)
			{
				ConnectToDatabase();
			}
			if (_connection != null && _connection.State != ConnectionState.Open)
			{
				_connection.Open();
			}
			return _connection != null && _connection.State == ConnectionState.Open;
		}
		catch (Exception ex)
		{
			GD.PrintErr($"EnsureConnection failed: {ex.Message}");
			return false;
		}
	}

	private static string GetConfigValue(string key, Dictionary<string, string> fileEnv, string fallback)
	{
		string envValue = System.Environment.GetEnvironmentVariable(key);
		if (!string.IsNullOrEmpty(envValue))
		{
			return envValue;
		}
		if (fileEnv != null && fileEnv.TryGetValue(key, out var fileValue) && !string.IsNullOrEmpty(fileValue))
		{
			return fileValue;
		}
		return fallback;
	}

	private static Dictionary<string, string> LoadDotEnv(string resPath)
	{
		try
		{
			string absolutePath = ProjectSettings.GlobalizePath(resPath);
			if (!FileAccess.FileExists(resPath))
			{
				return null;
			}
			var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			using var file = FileAccess.Open(resPath, FileAccess.ModeFlags.Read);
			while (!file.EofReached())
			{
				string line = file.GetLine();
				if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
				{
					continue;
				}
				int eq = line.IndexOf('=');
				if (eq <= 0)
				{
					continue;
				}
				string key = line.Substring(0, eq).Trim();
				string value = line.Substring(eq + 1).Trim().Trim('"');
				dict[key] = value;
			}
			return dict;
		}
		catch
		{
			return null;
		}
	}

	public bool PlayerExists(string pseudo)
	{
		if (!EnsureConnection())
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
		if (!EnsureConnection())
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
		if (!EnsureConnection())
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
				if (result != null && int.TryParse(result.ToString(), out int playerId))
				{
					return playerId;
				}
				return -1;
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
		if (!EnsureConnection())
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
		if (!EnsureConnection())
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
		if (!EnsureConnection())
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

	public void AddWordToLexique(string word, string category)
	{
		if (!EnsureConnection())
		{
			GD.PrintErr("Database connection is not available.");
			return;
		}

		try
		{
			var query = "INSERT INTO Lexique (mot, categorie) VALUES (@mot, @categorie) ON CONFLICT (mot, categorie) DO NOTHING;";
			using (var cmd = new NpgsqlCommand(query, _connection))
			{
				cmd.Parameters.AddWithValue("mot", word);
				cmd.Parameters.AddWithValue("categorie", category);
				cmd.ExecuteNonQuery();
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Error adding word to lexique: {ex.Message}");
		}
	}

	public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> GetLexiqueThemes()
	{
		var lexiqueThemes = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>();
		if (!EnsureConnection())
		{
			GD.PrintErr("Database connection is not available.");
			return lexiqueThemes;
		}

		try
		{
			var query = "SELECT mot, categorie FROM Lexique;";
			using (var cmd = new NpgsqlCommand(query, _connection))
			{
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						var mot = reader.GetString(0);
						var categorie = reader.GetString(1);
						if (!lexiqueThemes.ContainsKey(categorie))
						{
							lexiqueThemes[categorie] = new System.Collections.Generic.List<string>();
						}
						lexiqueThemes[categorie].Add(mot);
					}
				}
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Error getting lexique themes: {ex.Message}");
		}

		return lexiqueThemes;
	}

	public override void _ExitTree()
	{
		if (_connection != null)
		{
			_connection.Close();
			_connection.Dispose();
		}
	}

	// Méthodes de sauvegarde
	public int SaveGame(int idJoueur, Vector2 position, int idZone)
	{
		if (!EnsureConnection())
		{
			GD.PrintErr("Database connection is not available.");
			return -1;
		}

		try
		{
			var query = @"
				INSERT INTO Sauvegarde (idJoueur, positionXJoueur, positionYJoueur, idZone, dateSauvegarde) 
				VALUES (@idJoueur, @posX, @posY, @idZone, @date) 
				RETURNING idSauvegarde;";
			
			using (var cmd = new NpgsqlCommand(query, _connection))
			{
				cmd.Parameters.AddWithValue("idJoueur", idJoueur);
				cmd.Parameters.AddWithValue("posX", (int)position.X);
				cmd.Parameters.AddWithValue("posY", (int)position.Y);
				cmd.Parameters.AddWithValue("idZone", idZone);
				cmd.Parameters.AddWithValue("date", DateTime.Now);
				
				var result = cmd.ExecuteScalar();
				if (result != null && int.TryParse(result.ToString(), out int saveId))
				{
					return saveId;
				}
				return -1;
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Error saving game: {ex.Message}");
			return -1;
		}
	}

	public int GetZoneIdByName(string zoneName)
	{
		if (!EnsureConnection())
		{
			GD.PrintErr("Database connection is not available.");
			return -1;
		}

		try
		{
			var query = "SELECT idZone FROM Zone WHERE nomZone = @zoneName;";
			using (var cmd = new NpgsqlCommand(query, _connection))
			{
				cmd.Parameters.AddWithValue("zoneName", zoneName);
				var result = cmd.ExecuteScalar();
				if (result != null && int.TryParse(result.ToString(), out int zoneId))
				{
					return zoneId;
				}
				return -1;
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Error getting zone ID: {ex.Message}");
			return -1;
		}
	}

	public Godot.Collections.Dictionary LoadMostRecentSave(int idJoueur)
	{
		if (!EnsureConnection())
		{
			GD.PrintErr("Database connection is not available.");
			return null;
		}

		try
		{
			var query = @"
				SELECT s.positionXJoueur, s.positionYJoueur, z.nomZone 
				FROM Sauvegarde s
				JOIN Zone z ON s.idZone = z.idZone
				WHERE s.idJoueur = @idJoueur 
				ORDER BY s.dateSauvegarde DESC 
				LIMIT 1;";

			using (var cmd = new NpgsqlCommand(query, _connection))
			{
				cmd.Parameters.AddWithValue("idJoueur", idJoueur);
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						var saveData = new Godot.Collections.Dictionary();
						
						var posX = reader.GetInt32(0);
						var posY = reader.GetInt32(1);
						saveData["position"] = new Vector2(posX, posY);
						saveData["scene_name"] = reader.GetString(2);
						
						GD.Print($"Loaded save data: Position=({posX}, {posY}), Scene={reader.GetString(2)}");
						return saveData;
					}
				}
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Error loading most recent save: {ex.Message}");
		}

		return null;
	}

	public Godot.Collections.Dictionary LoadMostRecentSaveFromAnyPlayer()
	{
		if (!EnsureConnection())
		{
			GD.PrintErr("Database connection is not available.");
			return null;
		}

		try
		{
			var query = @"
				SELECT s.positionXJoueur, s.positionYJoueur, z.nomZone 
				FROM Sauvegarde s
				JOIN Zone z ON s.idZone = z.idZone
				ORDER BY s.dateSauvegarde DESC 
				LIMIT 1;";

			using (var cmd = new NpgsqlCommand(query, _connection))
			{
				using (var reader = cmd.ExecuteReader())
				{
					if (reader.Read())
					{
						var saveData = new Godot.Collections.Dictionary();
						
						var posX = reader.GetInt32(0);
						var posY = reader.GetInt32(1);
						saveData["position"] = new Vector2(posX, posY);
						saveData["scene_name"] = reader.GetString(2);
						
						GD.Print($"Loaded save data from any player: Position=({posX}, {posY}), Scene={reader.GetString(2)}");
						return saveData;
					}
				}
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Error loading most recent save from any player: {ex.Message}");
		}

		return null;
	}
} 