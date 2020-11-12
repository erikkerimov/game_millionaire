using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kursovaya.DatabaseConnections
{
    public class DatabaseMySQLProvider : IDatabaseProvider, IDisposable
    {
        DatabaseMySQLConnection _connection;
        CancellationTokenSource _tokenSource = new CancellationTokenSource();
        CancellationToken _token;
        public DatabaseMySQLProvider(DatabaseMySQLConnection connection)
        {
            _token = _tokenSource.Token;
        }

        private string GetGameDataCommand = "SELECT * FROM game_data WHERE Name = @N";

        public List<string[]> GetGameData()
        {
            var result = new List<string[]>(7);
            _connection.OpenConnection();
            var table = new DataTable();
            var command = new MySqlCommand("SELECT * FROM game_data WHERE Name = @N", _connection.GetConnection());
            command.Parameters.Add("@N", MySqlDbType.VarChar).Value = data_program.game_name;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new string[7]);

                result[result.Count - 1][0] = reader[0].ToString();
                result[result.Count - 1][1] = reader[1].ToString();
                result[result.Count - 1][2] = reader[2].ToString();
                result[result.Count - 1][3] = reader[3].ToString();
                result[result.Count - 1][4] = reader[4].ToString();
                result[result.Count - 1][5] = reader[5].ToString();
                result[result.Count - 1][6] = reader[6].ToString();
            }
            reader.Close();
            return result;
        }

        public int GetLevel(string characterName)
        {
            using (var command = GetCommand(GetLevelCharacterCommandStr, CommandType.Text))
            {
                command.Parameters.Add("name", MySqlDbType.String);
                command.ExecuteReaderAsync(_token).ContinueWith(task =>
                {
                    var result = task.Result;
                    result.
                });
            }
        }

        private MySqlCommand GetCommand(string commandText, CommandType type)
        {
            var command = new MySqlCommand();
            command.CommandType = type;
            command.CommandText = commandText;
            command.CommandTimeout = 60;//todo add to config
            command.Connection = _connection.GetConnection();
            return command;
        }

        private const string GetLevelCharacterCommandStr = "SELECT level FROM game_data WHERE c_Name = @name";

        public void Dispose()
        {
            _tokenSource.Cancel();
        }
    }
}
