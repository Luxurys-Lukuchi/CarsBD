using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

//SearchCarWindow.xaml.cs
// Класс SearchCarWindow отвечает за окно поиска автомобилей
namespace CarsBD
{
    public partial class SearchCarWindow : Window
    {
        private readonly DatabaseManager dbManager; // Менеджер базы данных для выполнения операций поиска

        // Конструктор класса SearchCarWindow
        public SearchCarWindow(DatabaseManager dbManager)
        {
            InitializeComponent(); // Инициализация компонентов окна
            this.dbManager = dbManager; // Установка менеджера базы данных
        }

        // Обработчик события нажатия кнопки поиска
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение значений полей поиска из текстовых полей
            string marca = marcaTextBox.Text;
            string model = modelTextBox.Text;
            string number = numberTextBox.Text;
            string status = statusTextBox.Text;

            // Выполнение поиска машин по указанным параметрам с использованием менеджера базы данных
            List<Car> foundCars = dbManager.SearchCars(marca, model, number, status);

            // Передача найденных машин обратно в главное окно через DialogResult
            DialogResult = true;
            Tag = foundCars; // Добавление списка найденных машин в свойство Tag окна
            Close(); // Закрытие окна после выполнения поиска
        }

        // Обработчик события нажатия кнопки отмены
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Закрытие окна
        }
    }
}


