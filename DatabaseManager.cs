using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

//DataBaseManager.cs

// Класс DatabaseManager представляет менеджер базы данных


namespace CarsBD
{
    public class Car
    {
        public int ID { get; set; }
        public string Марка { get; set; }
        public string Модель { get; set; }
        public string Номер { get; set; }
        public string Статус { get; set; }
    }

    public class DatabaseManager
    {
        public event EventHandler<string> MessageRaised;
        private readonly string connectionString;
        private List<Car> cars;

        public DatabaseManager(string connectionString)
        {
            this.connectionString = connectionString;
            cars = new List<Car>();
            LoadCarsFromDatabase();
        }
        private void RaiseMessage(string message)
        {
            MessageRaised?.Invoke(this, message);
        }
        private void LoadCarsFromDatabase()
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM public.\"Машина\"";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                    {
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            cars.Add(new Car
                            {
                                ID = Convert.ToInt32(row["ID"]),
                                Марка = row["Марка"].ToString(),
                                Модель = row["Модель"].ToString(),
                                Номер = row["Номер"].ToString(),
                                Статус = row["Статус"].ToString()
                            });
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                RaiseMessage($"Ошибка при загрузке данных из базы данных: {ex.Message}");
            }
        }

        public void SaveCar(Car car)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "";

                    if (car.ID == 0)
                    {
                        sql = $"INSERT INTO public.\"Машина\" (\"Марка\", \"Модель\", \"Номер\", \"Статус\") VALUES ('{car.Марка}', '{car.Модель}', '{car.Номер}', '{car.Статус}')";
                    }
                    else
                    {
                        sql = $"UPDATE public.\"Машина\" " +
                             $"SET \"Марка\" = '{car.Марка}', " +
                             $"\"Модель\" = '{car.Модель}', " +
                             $"\"Номер\" = '{car.Номер}', " +
                             $"\"Статус\" = '{car.Статус}' " +
                             $"WHERE \"ID\" = {car.ID}";
                    }

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                RaiseMessage("Данные успешно сохранены в базе данных PostgreSQL.");
            }
            catch (NpgsqlException ex)
            {
                RaiseMessage($"Ошибка при сохранении данных в базе данных PostgreSQL: {ex.Message}");
            }
        }

        public void DeleteCarByID(int carID)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = $"DELETE FROM public.\"Машина\" WHERE \"ID\" = {carID}";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                RaiseMessage("Машина успешно удалена из базы данных.");
            }
            catch (NpgsqlException ex)
            {
                RaiseMessage($"Ошибка при удалении машины из базы данных: {ex.Message}");
            }
        }

        public void DeleteCarByNumber(string carNumber)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = $"DELETE FROM public.\"Машина\" WHERE \"Номер\" = '{carNumber}'";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                RaiseMessage("Машина успешно удалена из базы данных.");
            }
            catch (NpgsqlException ex)
            {
                RaiseMessage($"Ошибка при удалении машины из базы данных: {ex.Message}");
            }
        }

        public bool IsCarNumberUnique(string number)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = $"SELECT COUNT(*) FROM public.\"Машина\" WHERE \"Номер\" = '{number}'";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count == 0;
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                RaiseMessage($"Ошибка при проверке уникальности номера машины в базе данных PostgreSQL: {ex.Message}");
                return false;
            }
        }

        public List<Car> SearchCars(string marca, string model, string number, string status)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM public.\"Машина\" WHERE ";
                    List<string> conditions = new List<string>();

                    if (!string.IsNullOrEmpty(marca))
                        conditions.Add($"\"Марка\" LIKE '%{marca}%'");
                    if (!string.IsNullOrEmpty(model))
                        conditions.Add($"\"Модель\" LIKE '%{model}%'");
                    if (!string.IsNullOrEmpty(number))
                        conditions.Add($"\"Номер\" LIKE '%{number}%'");
                    if (!string.IsNullOrEmpty(status))
                        conditions.Add($"\"Статус\" LIKE '%{status}%'");

                    sql += string.Join(" AND ", conditions);

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                    {
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        List<Car> foundCars = new List<Car>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            foundCars.Add(new Car
                            {
                                ID = Convert.ToInt32(row["ID"]),
                                Марка = row["Марка"].ToString(),
                                Модель = row["Модель"].ToString(),
                                Номер = row["Номер"].ToString(),
                                Статус = row["Статус"].ToString()
                            });
                        }
                        return foundCars;
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                RaiseMessage($"Ошибка при поиске машин в базе данных: {ex.Message}");
                return new List<Car>();
            }
        }
    }
}




