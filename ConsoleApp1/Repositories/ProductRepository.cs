using ConsoleApp1.Models;
using Npgsql;
using PersonalRole = ConsoleApp1.enums.PersonalRole;

namespace ConsoleApp1.IRepository
{
    public class PersonalRepository : IPersonalRepository
    {
        private readonly DatabaseConfig _config;

        public PersonalRepository(DatabaseConfig config)
        {
            _config = config;
        }

        public async Task<Personal?> GetPersonalByLoginAsync(string login)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                SELECT id, f_name, l_name, s_name, login, pass, personal_role, status, photo
                FROM personal 
                WHERE login = @login AND status = true
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@login", login);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Personal
                {
                    Id = reader.GetInt32(0),
                    FName = reader.GetString(1),
                    LName = reader.GetString(2),
                    SName = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Login = reader.GetString(4),
                    Pass = reader.GetString(5),
                    PersonalRole = (PersonalRole)reader.GetInt32(6),
                    Status = reader.GetBoolean(7),
                    Photo = reader.IsDBNull(8) ? null : reader.GetString(8)
                };
            }

            return null;
        }

        public async Task<Personal?> GetPersonalByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                SELECT id, f_name, l_name, s_name, login, pass, personal_role, status, photo
                FROM personal 
                WHERE id = @id
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Personal
                {
                    Id = reader.GetInt32(0),
                    FName = reader.GetString(1),
                    LName = reader.GetString(2),
                    SName = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Login = reader.GetString(4),
                    Pass = reader.GetString(5),
                    PersonalRole = (PersonalRole)reader.GetInt32(6),
                    Status = reader.GetBoolean(7),
                    Photo = reader.IsDBNull(8) ? null : reader.GetString(8)
                };
            }

            return null;
        }

        public async Task<List<Personal>> GetAllPersonalAsync()
        {
            var personalList = new List<Personal>();
            
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                SELECT id, f_name, l_name, s_name, login, pass, personal_role, status, photo
                FROM personal 
                ORDER BY l_name, f_name
                """;

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                personalList.Add(new Personal
                {
                    Id = reader.GetInt32(0),
                    FName = reader.GetString(1),
                    LName = reader.GetString(2),
                    SName = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Login = reader.GetString(4),
                    Pass = reader.GetString(5),
                    PersonalRole = (PersonalRole)reader.GetInt32(6),
                    Status = reader.GetBoolean(7),
                    Photo = reader.IsDBNull(8) ? null : reader.GetString(8)
                });
            }

            return personalList;
        }

        public async Task<List<Personal>> GetActivePersonalAsync()
        {
            var personalList = new List<Personal>();
            
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                SELECT id, f_name, l_name, s_name, login, pass, personal_role, status, photo
                FROM personal 
                WHERE status = true
                ORDER BY l_name, f_name
                """;

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                personalList.Add(new Personal
                {
                    Id = reader.GetInt32(0),
                    FName = reader.GetString(1),
                    LName = reader.GetString(2),
                    SName = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Login = reader.GetString(4),
                    Pass = reader.GetString(5),
                    PersonalRole = (PersonalRole)reader.GetInt32(6),
                    Status = reader.GetBoolean(7),
                    Photo = reader.IsDBNull(8) ? null : reader.GetString(8)
                });
            }

            return personalList;
        }

        public async Task<List<Personal>> GetPersonalByRoleAsync(PersonalRole role)
        {
            var personalList = new List<Personal>();
            
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                SELECT id, f_name, l_name, s_name, login, pass, personal_role, status, photo
                FROM personal 
                WHERE personal_role = @role AND status = true
                ORDER BY l_name, f_name
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@role", (int)role);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                personalList.Add(new Personal
                {
                    Id = reader.GetInt32(0),
                    FName = reader.GetString(1),
                    LName = reader.GetString(2),
                    SName = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Login = reader.GetString(4),
                    Pass = reader.GetString(5),
                    PersonalRole = (PersonalRole)reader.GetInt32(6),
                    Status = reader.GetBoolean(7),
                    Photo = reader.IsDBNull(8) ? null : reader.GetString(8)
                });
            }

            return personalList;
        }

        public async Task<List<Personal>> GetPersonalByRoleAndStatusAsync(PersonalRole role, bool isActive)
        {
            var personalList = new List<Personal>();
            
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                SELECT id, f_name, l_name, s_name, login, pass, personal_role, status, photo
                FROM personal 
                WHERE personal_role = @role AND status = @isActive
                ORDER BY l_name, f_name
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@role", (int)role);
            command.Parameters.AddWithValue("@isActive", isActive);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                personalList.Add(new Personal
                {
                    Id = reader.GetInt32(0),
                    FName = reader.GetString(1),
                    LName = reader.GetString(2),
                    SName = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Login = reader.GetString(4),
                    Pass = reader.GetString(5),
                    PersonalRole = (PersonalRole)reader.GetInt32(6),
                    Status = reader.GetBoolean(7),
                    Photo = reader.IsDBNull(8) ? null : reader.GetString(8)
                });
            }

            return personalList;
        }

        public async Task<int> CreatePersonalAsync(Personal personal)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                INSERT INTO personal (f_name, l_name, s_name, login, pass, personal_role, status, photo)
                VALUES (@fName, @lName, @sName, @login, @pass, @personalRole, @status, @photo)
                RETURNING id
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@fName", personal.FName);
            command.Parameters.AddWithValue("@lName", personal.LName);
            command.Parameters.AddWithValue("@sName", personal.SName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@login", personal.Login);
            command.Parameters.AddWithValue("@pass", personal.Pass);
            command.Parameters.AddWithValue("@personalRole", (int)personal.PersonalRole);
            command.Parameters.AddWithValue("@status", personal.Status);
            command.Parameters.AddWithValue("@photo", personal.Photo ?? (object)DBNull.Value);

            return (int)await command.ExecuteScalarAsync();
        }

        public async Task<bool> UpdatePersonalAsync(Personal personal)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                UPDATE personal 
                SET f_name = @fName, l_name = @lName, s_name = @sName, 
                    login = @login, personal_role = @personalRole, status = @status, photo = @photo
                WHERE id = @id
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", personal.Id);
            command.Parameters.AddWithValue("@fName", personal.FName);
            command.Parameters.AddWithValue("@lName", personal.LName);
            command.Parameters.AddWithValue("@sName", personal.SName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@login", personal.Login);
            command.Parameters.AddWithValue("@personalRole", (int)personal.PersonalRole);
            command.Parameters.AddWithValue("@status", personal.Status);
            command.Parameters.AddWithValue("@photo", personal.Photo ?? (object)DBNull.Value);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdatePersonalStatusAsync(int personalId, bool isActive)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = "UPDATE personal SET status = @isActive WHERE id = @id";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", personalId);
            command.Parameters.AddWithValue("@isActive", isActive);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdatePersonalPasswordAsync(int personalId, string newPasswordHash)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = "UPDATE personal SET pass = @pass WHERE id = @id";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", personalId);
            command.Parameters.AddWithValue("@pass", newPasswordHash);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> IsLoginExistsAsync(string login, int? excludePersonalId = null)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            var sql = "SELECT COUNT(1) FROM personal WHERE login = @login";
            if (excludePersonalId.HasValue)
            {
                sql += " AND id != @excludeId";
            }

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@login", login);
            
            if (excludePersonalId.HasValue)
            {
                command.Parameters.AddWithValue("@excludeId", excludePersonalId.Value);
            }

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }

        public async Task<List<Personal>> SearchPersonalByNameAsync(string searchTerm)
        {
            var personalList = new List<Personal>();
            
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                SELECT id, f_name, l_name, s_name, login, pass, personal_role, status, photo
                FROM personal 
                WHERE f_name ILIKE @searchTerm OR l_name ILIKE @searchTerm OR s_name ILIKE @searchTerm
                ORDER BY l_name, f_name
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                personalList.Add(new Personal
                {
                    Id = reader.GetInt32(0),
                    FName = reader.GetString(1),
                    LName = reader.GetString(2),
                    SName = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Login = reader.GetString(4),
                    Pass = reader.GetString(5),
                    PersonalRole = (PersonalRole)reader.GetInt32(6),
                    Status = reader.GetBoolean(7),
                    Photo = reader.IsDBNull(8) ? null : reader.GetString(8)
                });
            }

            return personalList;
        }
    }
}