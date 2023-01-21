namespace Survey.Infrastructure.DapperRepository
{
    using Dapper;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.Configuration;
    using Survey.Infrastructure.DapperRepository.StoredProcedure;
    using Survey.Infrastructure.DapperRepository.StoredProcedure.DapperDto;
    using System.ComponentModel;
    using System.Data;
    using System.Reflection;
    using System.Text;

    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        public readonly string TableName;
        public readonly string PrimaryKeyName;
        private readonly IConfiguration _configuration;

        public GenericRepository(string tableName, string primaryKeyName, IConfiguration configuration)
        {
            TableName = tableName;
            PrimaryKeyName = primaryKeyName;
            _configuration = configuration;
        }

        // GENERATE CONNECTION
        private SqlConnection SqlConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("ProjectDB"));
        }

        // CONNECTION HELPER
        private IDbConnection CreateConnection()
        {
            var conn = SqlConnection();
            conn.Open();
            return conn;
        }

        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        // ====================================== DELETE QUERY ======================================
        public async Task DeleteRowAsync(Guid id)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync($"DELETE FROM {TableName} WHERE {PrimaryKeyName}=@Id", new { Id = id });
            }
        }

        public async Task DeleteRowAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync($"DELETE FROM {TableName} WHERE {PrimaryKeyName}=@Id", new { Id = id });
            }
        }

        // ====================================== GETALL QUERY ======================================
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<T>($"SELECT * FROM {TableName}");
            }
        }

        // ====================================== GET QUERY ======================================
        public async Task<T> GetAsync(Guid id)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {TableName} WHERE {PrimaryKeyName}=@Id", new { Id = id });
                if (result == null)
                {
                    throw new KeyNotFoundException($"{TableName} with id [{id}] could not be found.");
                }

                return result;
            }
        }

        public async Task<T> GetAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.QuerySingleOrDefaultAsync<T>($"SELECT * FROM {TableName} WHERE {PrimaryKeyName}=@Id", new { Id = id });
                if (result == null)
                {
                    throw new KeyNotFoundException($"{TableName} with id [{id}] could not be found.");
                }

                return result;
            }
        }

        // ====================================== INSERT QUERY ======================================
        public async Task InsertAsync(T t)
        {
            var insertQuery = GenerateInsertQuery();

            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(insertQuery, t);
            }
        }

        // ====================================== UPDATE QUERY ======================================
        public async Task UpdateAsync(T t)
        {
            var updateQuery = GenerateUpdateQuery();

            using (var connection = CreateConnection())
            {
                await connection.ExecuteAsync(updateQuery, t);
            }
        }

        // ====================================== StoredProcedure ======================================
        public async Task StoredProcedureAsync(List<DapperCompanyCreationDto> company)
        {
            DataTable dt = company.ToDataTable();

            var parameters = new DynamicParameters();
            parameters.Add(
                "@Companies",
                dt.AsTableValuedParameter("udttCompany"));

            using (var connection = CreateConnection())
            {
                // Pass it to query
                var result = connection.Query<dynamic>("updateCompanies", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        // ------------------------------------------------------------------------------------------
        public async Task<int> SaveRangeAsync(IEnumerable<T> list)
        {
            var inserted = 0;
            var query = GenerateInsertQuery();
            using (var connection = CreateConnection())
            {
                inserted += await connection.ExecuteAsync(query, list);
            }

            return inserted;
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {TableName} ");

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {TableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);

            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append(" WHERE Id=@Id");

            return updateQuery.ToString();
        }
    }
}
