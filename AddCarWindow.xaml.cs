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


//AddCarWindow.xaml.cs
// Класс AddCarWindow отвечает за окно добавления нового автомобиля
namespace CarsBD
{
    public partial class AddCarWindow : Window
    {
        // Свойство NewCar для доступа к созданному автомобилю
        public Car NewCar { get; private set; }

        // Конструктор класса AddCarWindow
        public AddCarWindow()
        {
            InitializeComponent(); // Инициализация компонентов окна
        }

        // Обработчик события нажатия кнопки добавления
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // Получение значений полей из текстовых полей
            string marca = marcaTextBox.Text;
            string model = modelTextBox.Text;
            string number = numberTextBox.Text;
            string status = statusTextBox.Text;

            // Проверка наличия пустых полей
            if (string.IsNullOrEmpty(marca) || string.IsNullOrEmpty(model) || string.IsNullOrEmpty(number) || string.IsNullOrEmpty(status))
            {
                MessageBox.Show("Please fill in all fields."); // Вывод сообщения об ошибке
                return; // Прекращение выполнения метода
            }

            // Создание нового объекта класса Car с заданными значениями
            NewCar = new Car { Марка = marca, Модель = model, Номер = number, Статус = status };
            DialogResult = true; // Установка результата диалога как успешный
        }

        // Обработчик события нажатия кнопки отмены
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Закрытие окна
        }
    }
}


