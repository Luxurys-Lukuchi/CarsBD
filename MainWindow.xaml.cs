using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
//MainWindow.xaml.cs
// Класс MainWindow представляет главное окно приложения
namespace CarsBD
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "секрет так как гитхаб ругается";
        private DispatcherTimer autoSaveTimer;
        private bool autoSaveEnabled = false;
        private DatabaseManager dbManager;

        // Конструктор класса MainWindow
        // Конструктор MainWindow
        private ObservableCollection<Car> carsCollection;
        private DataTable dataTable;

        public MainWindow()
        {
            InitializeComponent();
            dbManager = new DatabaseManager(connectionString);
            dbManager.MessageRaised += HandleMessageRaised; // Подписка на событие из DatabaseManager
            InitializeAutoSaveTimer();
            Loaded += MainWindow_Loaded;

            carsCollection = new ObservableCollection<Car>();
            dataTable = new DataTable();
        }
        // Метод инициализации таймера для автосохранения
        private void InitializeAutoSaveTimer()
        {
            autoSaveTimer = new DispatcherTimer();
            autoSaveTimer.Interval = TimeSpan.FromMinutes(1); // Интервал автосохранения - 1 минута
            autoSaveTimer.Tick += AutoSaveTimer_Tick;
        }

        // Обработчик события для обновления TextBox при получении сообщения
        private void HandleMessageRaised(object sender, string message)
        {
            UpdateWarningText(message);
        }

        // Метод для обработки события загрузки окна
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Привязка горячих клавиш к командам
            CommandBinding binding;

            // Загрузка данных по нажатию Ctrl + O
            binding = new CommandBinding(ApplicationCommands.Open, MIFLoad_Click);
            CommandBindings.Add(binding);

            // Сохранение данных по нажатию Ctrl + S
            binding = new CommandBinding(ApplicationCommands.Save, MIFSave_Click);
            CommandBindings.Add(binding);

            // Поиск по нажатию Ctrl + F
            binding = new CommandBinding(ApplicationCommands.Find, searchButton_Click);
            CommandBindings.Add(binding);

            // Добавление нового автомобиля по нажатию Ctrl + N
            binding = new CommandBinding(ApplicationCommands.New, addButton_Click);
            CommandBindings.Add(binding);


        }


        // Обработчик события клика по пункту меню MIFLoad
        private void MIFLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadDataFromDatabase(); // Загрузка данных из базы данных
        }

        // Обработчик события клика по пункту меню MIFSave
        private void MIFSave_Click(object sender, RoutedEventArgs e)
        {
            SaveDataToDatabase(); // Сохранение данных в базу данных
        }

        // Асинхронный обработчик события тика таймера автосохранения
        private async void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            await Task.Run(() => SaveDataToDatabase()); // Вызов метода сохранения данных в базу данных
        }

        // Метод запуска автосохранения
        private void StartAutoSave()
        {
            autoSaveEnabled = true;
            autoSaveTimer.Start();
        }

        // Метод остановки автосохранения
        private void StopAutoSave()
        {
            autoSaveEnabled = false;
            autoSaveTimer.Stop();
        }

        // Обработчик события выбора пункта меню MISAutosave
        private void MISAutosave_Checked(object sender, RoutedEventArgs e)
        {
            StartAutoSave(); // Запуск автосохранения
        }

        // Обработчик события отмены выбора пункта меню MISAutosave
        private void MISAutosave_Unchecked(object sender, RoutedEventArgs e)
        {
            StopAutoSave(); // Остановка автосохранения
        }

        // Метод загрузки данных из базы данных
        // Метод загрузки данных из базы данных
        private void LoadDataFromDatabase()
        {
            string errorMessage = ""; // Создаем пустую строку для хранения сообщений об ошибках

            try
            {
                // Очищаем список автомобилей перед загрузкой новых данных
                carsCollection.Clear();

                // Создание подключения к базе данных PostgreSQL
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open(); // Открытие соединения
                    string sql = "SELECT * FROM public.\"Машина\""; // SQL-запрос для выборки всех записей из таблицы "Машина"
                    using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                    {
                        // Создание адаптера данных для выполнения SQL-запроса и заполнения DataTable
                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable); // Заполнение DataTable данными из базы данных

                        // Заполнение коллекции автомобилей данными из DataTable
                        foreach (DataRow row in dataTable.Rows)
                        {
                            carsCollection.Add(new Car
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

                // Установка источника данных для DataGrid равным вашей коллекции carsCollection
                dataGrid.ItemsSource = carsCollection;
            }
            catch (NpgsqlException ex)
            {
                errorMessage = "Ошибка при загрузке данных из базы данных: " + ex.Message; // Формируем сообщение об ошибке
            }

            // После завершения блока try-catch выводим сообщение об ошибке (если оно есть)
            if (!string.IsNullOrEmpty(errorMessage))
            {
                UpdateWarningText(errorMessage); // Выводим сообщение об ошибке
            }
        }





        // Метод сохранения данных в базу данных
        private void SaveDataToDatabase()
        {
            try
            {
                foreach (Car car in carsCollection)
                {
                    dbManager.SaveCar(car);
                }
                UpdateWarningText("Данные успешно сохранены в базе данных PostgreSQL.");
            }
            catch (NpgsqlException ex)
            {
                UpdateWarningText("Ошибка при сохранении данных в базе данных PostgreSQL:");
            }
        }




        // Обработчик события клика по кнопке addButton
        // Обработчик события клика по кнопке addButton
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание и отображение окна для добавления нового автомобиля
            AddCarWindow addCarWindow = new AddCarWindow();
            addCarWindow.Owner = this;
            if (addCarWindow.ShowDialog() == true)
            {
                // Получение нового автомобиля из окна добавления
                Car newCar = addCarWindow.NewCar;

                // Проверка уникальности ID
                if (newCar.ID != 0)
                {
                    UpdateWarningText("ID должен быть уникальным.");
                    return;
                }

                // Проверка уникальности государственного номера
                if (!dbManager.IsCarNumberUnique(newCar.Номер))
                {
                    UpdateWarningText("Государственный номер должен быть уникальным.");
                    return;
                }

                // Если проверки пройдены успешно, сохраняем автомобиль в базу данных
                dbManager.SaveCar(newCar);

                // Обновляем DataGrid после добавления нового автомобиля
                LoadDataFromDatabase();
            }
        }

        // Обработчик события клика по кнопке removeButton
        // Обработчик события клика по кнопке removeButton
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание и отображение окна для удаления автомобиля
            DeleteCarWindow deleteCarWindow = new DeleteCarWindow(dbManager);
            deleteCarWindow.Owner = this;
            deleteCarWindow.ShowDialog();

            // После закрытия окна обновляем данные в DataGrid
            LoadDataFromDatabase();
        }


        // Обработчик события клика по кнопке searchButton
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            // Создание и отображение окна для поиска автомобилей
            SearchCarWindow searchCarWindow = new SearchCarWindow(dbManager);
            searchCarWindow.Owner = this;
            if (searchCarWindow.ShowDialog() == true)
            {
                // Получение найденных автомобилей из окна поиска
                List<Car> foundCars = (List<Car>)searchCarWindow.Tag;

                // Обновление DataGrid для отображения найденных автомобилей
                dataGrid.ItemsSource = foundCars;
            }
        }

        // Обработчик события клика по кнопке clearButton
        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null; // Очистка источника данных для DataGrid
            // Дополнительная обработка нажатия кнопки очистки
        }



        // Обработчик события клика по пункту меню MIAbProgram
        private void MIAbProgram_Click(object sender, RoutedEventArgs e)
        {
            // Вывод информации о программе и её возможностях
            TBWarning.Text = "Программа управления базой данных автомобилей.\n" +
                             "Версия: 1.0\n" +
                             "Описание:\n" +
                             "Данная программа предназначена для управления базой данных, содержащей информацию об автомобилях. " +
                             "Она предоставляет возможности добавления, удаления, поиска и редактирования записей о машинах. " +
                             "Вся информация хранится в базе данных PostgreSQL.\n\n" +
                             "Функциональные возможности:\n" +
                             "- Добавление новых автомобилей в базу данных.\n" +
                             "- Удаление автомобилей по их ID или номеру.\n" +
                             "- Поиск автомобилей по различным параметрам (марка, модель, номер, статус).\n" +
                             "- Автосохранение данных каждую минуту при включенной опции.\n" +
                             "- Возможность включения/выключения автоматического добавления новых записей.\n" +
                             "- Отображение информации о программе и её разработчике.\n" +
                             "- Возможность загрузки и сохранения данных из/в базу данных.";
        }

        // Обработчик события клика по пункту меню MIAbDeveloper
        private void MIAbDeveloper_Click(object sender, RoutedEventArgs e)
        {
            // Вывод информации о разработчике
            TBWarning.Text = "Разработчик: [Luxurys]\n" +
                             "Email: [email]\n" +
                             "Адрес: [China]\n\n" +
                             "Для технической поддержки, предложений и обратной связи свяжитесь с разработчиком.";
        }

        private void TBWarning_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void UpdateWarningText(string message)
        {
            TBWarning.Text = message;
        }

    }
}


