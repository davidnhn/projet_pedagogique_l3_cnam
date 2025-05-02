using Godot;
using Npgsql;
using System;

namespace JeuVideo.Data.Database
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        private NpgsqlConnection _connection;
        private readonly string _connectionString;

        private DatabaseConnection()
        {
            // Récupération des variables d'environnement
            string host = System.Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            string port = System.Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            string database = System.Environment.GetEnvironmentVariable("DB_NAME") ?? "jeuvideo";
            string username = System.Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
            string password = System.Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "postgres";

            _connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
        }

        public static DatabaseConnection Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabaseConnection();
                }
                return _instance;
            }
        }

        public NpgsqlConnection GetConnection()
        {
            if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            {
                _connection = new NpgsqlConnection(_connectionString);
                _connection.Open();
            }
            return _connection;
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
    }
} 