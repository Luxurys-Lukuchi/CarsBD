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

    // Класс, отвечающий за взаимодействие с базой данных
    public class DatabaseManager
    {
        private readonly string connectionString; // Строка подключения к базе данных

        // Конструктор класса DatabaseManager
        public DatabaseManager(string connectionString)
        {
            this.connectionString = connectionString; // Инициализация строки подключения
        }

        // Метод для сохранения списка автомобилей в базу данных
        public void SaveCars(List<Car> cars)
        {
            try
            {
                // Создание подключения к базе данных PostgreSQL
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open(); // Открытие соединения

                    // Проход по списку автомобилей для сохранения каждого из них в базе данных
                    foreach (Car car in cars)
                    {
                        string sql = "";

                        if (car.ID == 0)
                        {
                            // Если ID автомобиля равен 0, то он считается новым и будет добавлен в базу данных
                            sql = $"INSERT INTO public.\"Машина\" (\"Марка\", \"Модель\", \"Номер\", \"Статус\") VALUES ('{car.Марка}', '{car.Модель}', '{car.Номер}', '{car.Статус}')";
                        }
                        else
                        {
                            // В противном случае, осуществляется обновление данных о существующем автомобиле
                            sql = $"UPDATE public.\"Машина\" " +
                                 $"SET \"Марка\" = '{car.Марка}', " +
                                 $"\"Модель\" = '{car.Модель}', " +
                                 $"\"Номер\" = '{car.Номер}', " +
                                 $"\"Статус\" = '{car.Статус}' " +
                                 $"WHERE \"ID\" = {car.ID}";
                        }

                        // Выполнение SQL-запроса
                        using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                        {
                            cmd.ExecuteNonQuery(); // Выполнение запроса без возврата данных
                        }
                    }
                }

                MessageBox.Show("Данные успешно сохранены в базе данных PostgreSQL.");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных в базу данных PostgreSQL: {ex.Message}");
            }
        }

        // Метод для удаления автомобиля из базы данных по его ID
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
                MessageBox.Show("Машина успешно удалена из базы данных.");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка при удалении машины из базы данных: {ex.Message}");
            }
        }

        // Метод для удаления автомобиля из базы данных по его номеру
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
                MessageBox.Show("Машина успешно удалена из базы данных.");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка при удалении машины из базы данных: {ex.Message}");
            }
        }

        // Метод для проверки уникальности номера автомобиля в базе данных
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
                        return count == 0; // Возвращается true, если количество записей с заданным номером равно 0
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка при проверке уникальности номера машины в базе данных PostgreSQL: {ex.Message}");
                return false;
            }
        }

        // Метод для поиска автомобилей в базе данных по указанным параметрам
        public List<Car> SearchCars(string marca, string model, string number, string status)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM public.\"Машина\" WHERE ";
                    List<string> conditions = new List<string>();

                    // Формирование SQL-запроса с учетом указанных параметров поиска
                    if (!string.IsNullOrEmpty(marca))
                        conditions.Add($"\"Марка\" LIKE '%{marca}%'");
                    if (!string.IsNullOrEmpty(model))
                        conditions.Add($"\"Модель\" LIKE '%{model}%'");
                    if (!string.IsNullOrEmpty(number))
                        conditions.Add($"\"Номер\" LIKE '%{number}%'");
                    if (!string.IsNullOrEmpty(status))
                        conditions.Add($"\"Статус\" LIKE '%{status}%'");

                    sql += string.Join(" AND ", conditions); // Объединение условий запроса с помощью оператора AND

                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                    {
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        List<Car> foundCars = new List<Car>();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            // Создание объектов Car на основе данных из базы и добавление их в список найденных автомобилей
                            foundCars.Add(new Car
                            {
                                ID = Convert.ToInt32(row["ID"]),
                                Марка = row["Марка"].ToString(),
                                Модель = row["Модель"].ToString(),
                                Номер = row["Номер"].ToString(),
                                Статус = row["Статус"].ToString()
                            });
                        }
                        return foundCars; // Возврат списка найденных автомобилей
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка при поиске машин в базе данных: {ex.Message}");
                return new List<Car>(); // В случае ошибки возвращается пустой список
            }
        }
    }
}



