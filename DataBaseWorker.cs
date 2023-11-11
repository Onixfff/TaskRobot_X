using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;

namespace Solution1
{
    public class DataBaseWorker
    {
        private string _connectString;
        private MySqlConnection _myConnection;
        private string _sqlServerName = "127.0.0.1";
        private string _uId = "root";
        private string _password = "12345";
        private string _dataBaseName;

        public DataBaseWorker(string dataBaseName)
        {
            _dataBaseName = dataBaseName;
            _connectString = $"server={_sqlServerName};" +
                $"uid={_uId};" +
                $"pwd={_password};" +
                $"database={_dataBaseName}";

            using(_myConnection = new MySqlConnection(_connectString)) 
            {
                _myConnection.Close();
            }

            CheckDataBase($"server={_sqlServerName};" +$"uid={_uId};" + $"pwd={_password};");

            _myConnection.Close();

        }

        private void CheckDataBase(string connectString)
        {
            if (!DatabaseExists(connectString))
            {
                CreateDatabase(connectString);
                Console.WriteLine("База данных создана успешно");
            }
            else
            {
                Console.WriteLine("База данных уже существует");
            }

            _myConnection.Close();
        }

        public void CheckTable(string tableName = "test_table")
        {
            if (!TableExists(tableName))
            {
                CreateTable(tableName);
                Console.WriteLine("Таблица создана успешно.");
            }
            else
            {
                Console.WriteLine("Таблица уже существует.");
            }
        }

        bool TableExists(string tableName)
        {
            using (_myConnection = new MySqlConnection(_connectString))
            {
                _myConnection.Open();

                string checkTableQuery = $"SHOW TABLES LIKE '{tableName}'";
                MySqlCommand command = new MySqlCommand(checkTableQuery, _myConnection);

                using (var reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public void ImportXml(List<Card> cards)
        {
            InsertData(cards);
        }

        public DataTable LoadDataFromDatabase(DataTable dataTable, string tableName = "test_table")
        {
            using (MySqlConnection connection = new MySqlConnection(_connectString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = $"SELECT * FROM {tableName}";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection))
                    {
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка при проверке базы данных: {ex.Message}");
                    return null;
                }
            }
        }


        private void InsertData(List<Card> cards, string tableName = "test_table")
        {
            foreach (var card in cards)
            {
                string insertDataQuery = $"INSERT INTO {tableName}" +
                    $" (cardCode, startDate, finishDate, lastName, firstName, surName, fullName, genderId, birthday, phoneHome, phoneMobil, email, city, street, house, apartment, alltaddress, cardType, ownerguId, cardper, turnover) " +
                    $"VALUES (@cardCode, @startDate, @finishDate, @lastName, @firstName, @surName, @fullName, @genderId, @birthday, @phoneHome, @phoneMobil, @email, @city, @street, @house, @apartment, @alltaddress, @cardType, @ownerguId, @cardper, @turnover)";

                using (MySqlConnection connection = new MySqlConnection(_connectString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(insertDataQuery, connection))
                    {
                        //command.Parameters.AddWithValue("@cardCode", card.cardCode);
                        CheckParametrInput("@cardCode", card.cardCode, command, false);
                        CheckParametrInput("@startDate", card.startDate, command);
                        CheckParametrInput("@finishDate", card.finishDate,command);
                        CheckParametrInput("@lastName", card.lastName, command);
                        CheckParametrInput("@firstName", card.firstName, command);
                        CheckParametrInput("@surName", card.surName, command);
                        CheckParametrInput("@fullName", card.fullName, command);
                        CheckParametrInput("@genderId", card.genderId, command, false);
                        CheckParametrInput("@birthday", card.birthday, command);
                        CheckParametrInput("@phoneHome", card.phoneHome, command);
                        CheckParametrInput("@phoneMobil", card.phoneMobil, command);
                        CheckParametrInput("@email", card.email, command);
                        CheckParametrInput("@city", card.city, command);
                        CheckParametrInput("@street", card.street, command);
                        CheckParametrInput("@house", card.house, command);
                        CheckParametrInput("@apartment", card.apartment, command);
                        CheckParametrInput("@alltaddress", card.alltaddress, command);
                        CheckParametrInput("@cardType", card.cardType, command);
                        CheckParametrInput("@ownerguId", card.ownerguId, command);
                        CheckParametrInput("@cardper", card.cardper, command);
                        CheckParametrInput("@turnover", card.turnover, command);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void CheckParametrInput(string parametr, DateTime? value, MySqlCommand command)
        {
            if (value != null && value.Value != null)
            {
                command.Parameters.AddWithValue(parametr, value.Value);
            }
            else
            {
                command.Parameters.AddWithValue(parametr, DBNull.Value);
            }
        }

        private void CheckParametrInput<T>(string parametr, T value, MySqlCommand command, bool isHaveNull = true)
        {
            if (value != null && value.ToString() != "")
            {
                command.Parameters.AddWithValue(parametr, value);
            }
            else if (isHaveNull == false)
            {
                MessageBox.Show("Ошибка заполнения данных");
            }
            else
            {
                command.Parameters.AddWithValue(parametr, DBNull.Value);
            }
        }

        private void CreateTable(string tableName)
        {
            using (_myConnection = new MySqlConnection(_connectString))
            {
                _myConnection.Open();

                string createTableQuery = $"CREATE TABLE `{_dataBaseName}`.`{tableName}` (\r\n  `id` INT NOT NULL AUTO_INCREMENT,\r\n  `cardCode` BIGINT NOT NULL,\r\n  `startDate` DATETIME NULL,\r\n  `finishDate` DATETIME NULL,\r\n  `lastName` VARCHAR(45) NULL,\r\n  `firstName` VARCHAR(45) NULL,\r\n  `surName` VARCHAR(45) NULL,\r\n  `fullName` VARCHAR(45) NULL,\r\n  `genderId` INT NOT NULL,\r\n  `birthday` DATETIME NULL,\r\n  `phoneHome` VARCHAR(45) NULL,\r\n  `phoneMobil` VARCHAR(45) NULL,\r\n  `email` VARCHAR(45) NULL,\r\n  `city` VARCHAR(45) NULL,\r\n  `street` VARCHAR(45) NULL,\r\n  `house` VARCHAR(45) NULL,\r\n  `apartment` VARCHAR(45) NULL,\r\n  `alltaddress` VARCHAR(45) NULL,\r\n  `cardType` VARCHAR(45) NULL,\r\n  `ownerguId` VARCHAR(45) NULL,\r\n  `cardper` BIGINT NULL,\r\n  `turnover` DOUBLE NULL,\r\n  PRIMARY KEY (`id`),\r\n  UNIQUE INDEX `cardCode_UNIQUE` (`cardCode` ASC) VISIBLE);\r\n";

                MySqlCommand command = new MySqlCommand(createTableQuery, _myConnection);
                command.ExecuteNonQuery();
            }
        }

        private void CreateDatabase(string connectString)
        {
            using (_myConnection = new MySqlConnection(connectString))
            {
                try
                {
                    _myConnection.Open();

                    string createDatabaseQuery = $"CREATE DATABASE {_dataBaseName}";
                    MySqlCommand command = new MySqlCommand(createDatabaseQuery, _myConnection);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при проверке базы данных: {ex.Message}");
                }
            }        

        }

        private bool DatabaseExists(string connectString)
        {
            using (_myConnection = new MySqlConnection(connectString))
            {
                try
                {
                    _myConnection.Open();

                    string checkDatabaseQuery = $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = DATABASE()";
                    MySqlCommand command = new MySqlCommand(checkDatabaseQuery, _myConnection);

                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при проверке базы данных: {ex.Message}");
                    return false;
                }
            }
        }

        public bool SaveChangesToDatabase(DataTable dataTable, string tableName = "test_table")
        {
            using (MySqlConnection connection = new MySqlConnection(_connectString))
            {
                try
                {
                    connection.Open();

                    string selectQuery = $"SELECT * FROM {tableName}";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectQuery, connection))
                    using (MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter))
                    {
                        adapter.Update(dataTable);
                    }
                    return true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Ошибка при обновлении информации в базе данных: {ex.Message}");
                    return false;
                }
            }
        }

    }
}
