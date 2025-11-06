using System.Text.Json;
using Npgsql;

namespace ConsoleApp1;

public class DatabaseConfig
{
    public string Server { get; set; } = "";
    public string Database { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    
    public string GetConnectionString()
    {
        return $"Host={Server};Database={Database};Username={Username};Password={Password}";
    }
}

public static class ConfigLoader
{
    private static NpgsqlConnection? _connection;
    private static DatabaseConfig? _config;

    public static DatabaseConfig LoadConfig(string filePath = "database.config.json")
    {
        try
        {
            if (!File.Exists(filePath))
            {
                // Создаем конфиг по умолчанию
                var defaultConfig = new DatabaseConfig
                {
                    Server = "localhost",
                    Database = "CafeDB",
                    Username = "postgres", // Изменено с "sa" на "postgres" для PostgreSQL
                    Password = "password"
                };
                SaveConfig(defaultConfig, filePath);
                _config = defaultConfig;
                return defaultConfig;
            }

            var json = File.ReadAllText(filePath);
            _config = JsonSerializer.Deserialize<DatabaseConfig>(json) ?? new DatabaseConfig();
            return _config;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка загрузки конфигурации: {ex.Message}");
        }
    }

    public static void SaveConfig(DatabaseConfig config, string filePath = "database.config.json")
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(filePath, json);
            _config = config;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка сохранения конфигурации: {ex.Message}");
        }
    }
    
    public static NpgsqlConnection GetSqlConnection()
    {
        if (_config == null)
        {
            _config = LoadConfig();
        }

        if (_connection?.State == System.Data.ConnectionState.Open)
        {
            return _connection;
        }

        _connection?.Dispose();
        _connection = new NpgsqlConnection(_config.GetConnectionString());
        _connection.Open();
        
        return _connection;
    }

    public static void CloseConnection()
    {
        if (_connection?.State == System.Data.ConnectionState.Open)
        {
            _connection.Close();
        }
        _connection?.Dispose();
        _connection = null;
    }
}