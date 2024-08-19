using Npgsql;
using System.Data;

namespace Api.Models
{
    public class DataBase
    {
        private NpgsqlDataSource dataSource;
        public static async Task<DataBase> BuildDataBaseAsync()
        {
            DataBase db = new DataBase();

            var tables = await db.GetAllTables();

            await db.ExecuteSql("CREATE SCHEMA IF NOT EXISTS dev;");

            if (!tables.Contains("dev.messages"))
            {
                await db.ExecuteSql(@"CREATE TABLE dev.messages (
                                        id bigserial primary key,
                                        text varchar(128) NOT NULL,
                                        serial_number bigserial NOT NULL,
                                        created_at TIMESTAMP DEFAULT NOW()
                                    );");
            }

            return db;
        }
        private DataBase()
        {
            var connectionString = "Host=postgres_db;Username=postgres;Password=123;Database=postgres";
            dataSource = NpgsqlDataSource.Create(connectionString);
        }

        public async Task<List<string>> GetAllTables()
        {
            DataTable tables = await ExecuteSql("SELECT * FROM pg_catalog.pg_tables");

            return tables.Rows.Cast<DataRow>().Select(c => c["schemaname"] + "." + c["tablename"]).ToList();
        }

        public async Task<List<Message>> GetAllMessages()
        {
            DataTable dt = await ExecuteSql("SELECT * FROM dev.messages;");

            return dt.Rows.Cast<DataRow>().Select(c => Message.FromDataRow(c)).ToList();
        }

        public async Task<List<Message>> GetMessages(DateTime from, DateTime to)
        {
            DataTable dt = await ExecuteSql($"SELECT * FROM dev.messages where created_at BETWEEN '{from}' and '{to}';");

            return dt.Rows.Cast<DataRow>().Select(c => Message.FromDataRow(c)).ToList();
        }

        public async Task AddMessage(Message message)
        {
            await ExecuteSql($"INSERT INTO dev.messages(text, serial_number, created_at) VALUES ('{message.Text}', {message.SerialNumber}, '{message.CreatedAt}')");
        }

        private async Task<DataTable> ExecuteSql(string sql)
        {
            await using var command = dataSource.CreateCommand(sql);
            await using var reader = await command.ExecuteReaderAsync();

            DataTable dt = new DataTable();
            dt.Load(reader);

            return dt;
        }
    }
}
