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


//DeleteCarWindow.xaml.cs
// Класс DeleteCarWindow отвечает за окно удаления автомобиля
namespace CarsBD
{
    public partial class DeleteCarWindow : Window
    {
        private readonly DatabaseManager dbManager; // Менеджер базы данных для выполнения операций удаления

        // Конструктор класса DeleteCarWindow
        public DeleteCarWindow(DatabaseManager dbManager)
        {
            InitializeComponent(); // Инициализация компонентов окна
            this.dbManager = dbManager; // Установка менеджера базы данных
        }

        // Обработчик события нажатия кнопки удаления
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выбранного метода удаления (по ID или по номеру)
            if (deleteByIDRadioButton.IsChecked == true)
            {
                int carID;
                // Попытка преобразования введенного текста в число
                if (int.TryParse(carIdentifierTextBox.Text, out carID))
                {
                    // Удаление автомобиля по ID с использованием менеджера базы данных
                    dbManager.DeleteCarByID(carID);
                    Close(); // Закрытие окна после успешного удаления
                }
                else
                {
                    MessageBox.Show("Введите корректный ID машины."); // Вывод сообщения об ошибке в случае некорректного ввода
                }
            }
            else if (deleteByNumberRadioButton.IsChecked == true)
            {
                string carNumber = carIdentifierTextBox.Text;
                // Проверка наличия номера автомобиля
                if (!string.IsNullOrEmpty(carNumber))
                {
                    // Удаление автомобиля по номеру с использованием менеджера базы данных
                    dbManager.DeleteCarByNumber(carNumber);
                    Close(); // Закрытие окна после успешного удаления
                }
                else
                {
                    MessageBox.Show("Введите корректный номер машины."); // Вывод сообщения об ошибке в случае некорректного ввода
                }
            }
            else
            {
                MessageBox.Show("Выберите метод удаления."); // Вывод сообщения об ошибке при отсутствии выбранного метода
            }
        }

        // Обработчик события нажатия кнопки отмены
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Закрытие окна
        }
    }
}


