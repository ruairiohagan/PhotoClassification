using ClassificationData.Models;
using Microsoft.Data.Sqlite;

namespace ClassificationData
{
    /// <summary>
    /// High level DB oerations such as initial creation and ensuring structure is correct.
    /// </summary>
    public class DBController
    {
        public static readonly string DBFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\PhotoClassification\\PhotoClassification.db";
        /// <summary>
        /// Checks that the SQLite DB has the correct sturcture and adds any missing tables/columns
        /// </summary>
        public static void CheckDBStructures()
        {
            if (!Directory.GetParent(DBFile)?.Exists ?? false)
            {
                string settingsFolder = Directory.GetParent(DBFile)?.FullName ?? "";
                if (settingsFolder == "")
                {
                    throw new Exception("Could not find settings folder");
                }
                Directory.CreateDirectory(Directory.GetParent(DBFile)?.FullName ?? "");
            }
            using (SqliteConnection connection = new SqliteConnection($"Data Source={DBFile}"))
            {
                connection.Open();

                if (!TableExists(connection, "settings"))
                {
                    SqliteCommand cmd = connection.CreateCommand();
                    cmd.CommandText = @"
                        CREATE TABLE settings ( 
                            name TEXT PRIMARY KEY,
                            value TEXT NOT NULL,
                            comment TEXT NOT NULL
                        )";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"INSERT INTO settings (name, value, comment) VALUES 
                        ('APILocation', 'http://localhost:1234', 'Base URL for AI API (e.g. LM Studio)'),
                        ('ImageModel', 'llava-1.6-mistral-7b', 'Model used to generate classification text for images'),
                        ('ClassificationQuestion', 'What is this image?', 'Question used to prompt the AI to classify an image'),
                        ('TimeoutInMS', '60000', 'How long to wait for the classification API call to finish before throwing an error')";

                    cmd.ExecuteNonQuery();
                }

                if (!TableExists(connection, "imageFolders"))
                {
                    SqliteCommand cmd = connection.CreateCommand();
                    cmd.CommandText = @"
                        CREATE TABLE imageFolders ( 
                            folderPath TEXT PRIMARY KEY,
                            recursive INT NOT NULL
                        )";
                    cmd.ExecuteNonQuery();
                }

                if (!TableExists(connection, "images"))
                {
                    SqliteCommand cmd = connection.CreateCommand();
                    cmd.CommandText = @"
                        CREATE TABLE images ( 
                            imagePath TEXT PRIMARY KEY,
                            description TEXT NOT NULL,
                            imageDate TEXT NOT NULL,
                            descriptionDate TEXT NOT NULL,
                            modelUsed TEXT NOT NULL,
                            classificationMS INT NOT NULL
                        )";
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
        }        

        private static bool TableExists(SqliteConnection connection, string tableName)
        {
            SqliteCommand cmd = connection.CreateCommand();
            cmd.Parameters.Add("tableName", SqliteType.Text).Value = tableName;
            cmd.CommandText = @"
                SELECT name 
                FROM sqlite_master
                WHERE type = 'table'
                AND name = @tableName
            ";
            using (SqliteDataReader reader = cmd.ExecuteReader())
            {
                return (reader.HasRows);
            }
        }
    }
}
