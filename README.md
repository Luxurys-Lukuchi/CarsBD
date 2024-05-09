![nPRFRjDC4C3lUGeVtQ_D2qHLrJHfzKLg2cfIzHYDlQDawNetxAvJe49gYyG593WZEF06EL2109LNiD-8NSShnsvQrKM2IxAUcT_ycHrFigqqI1r7hD486IZbzO7oOyg9EFNE6esc5mhECTHKy86Mb8zRdig-hGvYBGOmmo6DKBQyBbLJqE44PIPOjjZb430aBIyGWba52Vh0OPnXe240XR-1](https://github.com/Luxurys-Lukuchi/CarsBD/assets/146846830/a9d503cf-4342-4593-ab46-55981be52235)




  ## something 
@startuml

class MainWindow {


- connectionString: string

- autoSaveTimer: DispatcherTimer

- autoSaveEnabled: bool

- dbManager: DatabaseManager

- carsCollection: ObservableCollection<Car>

- dataTable: DataTable

+ MainWindow()

+ InitializeAutoSaveTimer()

+ HandleMessageRaised(sender: object, message: string)

+ MainWindow_Loaded(sender: object, e: RoutedEventArgs)

+ MIFLoad_Click(sender: object, e: RoutedEventArgs)

+ MIFSave_Click(sender: object, e: RoutedEventArgs)

+ AutoSaveTimer_Tick(sender: object, e: EventArgs)

+ StartAutoSave()

+ StopAutoSave()

+ MISAutosave_Checked(sender: object, e: RoutedEventArgs)

+ MISAutosave_Unchecked(sender: object, e: RoutedEventArgs)

+ LoadDataFromDatabase()

+ SaveDataToDatabase()

+ addButton_Click(sender: object, e: RoutedEventArgs)

+ removeButton_Click(sender: object, e: RoutedEventArgs)

+ searchButton_Click(sender: object, e: RoutedEventArgs)

+ clearButton_Click(sender: object, e: RoutedEventArgs)

+ MIAbProgram_Click(sender: object, e: RoutedEventArgs)

+ MIAbDeveloper_Click(sender: object, e: RoutedEventArgs)

+ TBWarning_TextChanged(sender: object, e: TextChangedEventArgs)

+ UpdateWarningText(message: string)


}

class DatabaseManager {

- connectionString: string

- cars: List<Car>

+ DatabaseManager(connectionString: string)

+ RaiseMessage(message: string)

+ LoadCarsFromDatabase()

+ SaveCar(car: Car)

+ DeleteCarByID(carID: int)

+ DeleteCarByNumber(carNumber: string)

+ IsCarNumberUnique(number: string): bool

+ SearchCars(marca: string, model: string, number: string, status: string): List<Car>

}

class Car {

+ ID: int

+ Марка: string

+ Модель: string

+ Номер: string

+ Статус: string

}

class SearchCarWindow {

- dbManager: DatabaseManager

+ SearchCarWindow(dbManager: DatabaseManager)

+ searchButton_Click(sender: object, e: RoutedEventArgs)

+ cancelButton_Click(sender: object, e: RoutedEventArgs)

}

class DeleteCarWindow {

- dbManager: DatabaseManager

+ DeleteCarWindow(dbManager: DatabaseManager)

+ deleteButton_Click(sender: object, e: RoutedEventArgs)

+ cancelButton_Click(sender: object, e: RoutedEventArgs)

}

class AddCarWindow {

- NewCar: Car

+ AddCarWindow()

+ addButton_Click(sender: object, e: RoutedEventArgs)

+ cancelButton_Click(sender: object, e: RoutedEventArgs)

}

MainWindow <-- Car : Ассоциация

MainWindow --* DatabaseManager : Композиция

MainWindow --> SearchCarWindow : Ассоциация

MainWindow --> DeleteCarWindow : Ассоциация

MainWindow --> AddCarWindow : Ассоциация

SearchCarWindow --* DatabaseManager : Композицыя

DeleteCarWindow --* DatabaseManager : Композицыя

AddCarWindow --* Car : Компазицыя

@enduml


# Пример того задать свой стил для нескольких обьектов

В XAML (Extensible Application Markup Language) можно задать стиль нескольким объектам одновременно. Это можно сделать, определив стиль в ресурсах и затем применив его к нескольким элементам интерфейса

<Window.Resources>
    
    <!-- Определение стиля -->
    
    <Style x:Key="StylePrimer" TargetType="Control">
    
        <Setter Property="FontFamily" Value="Arial"/>
        
        <Setter Property="Foreground" Value="Blue"/>
    
    </Style>

</Window.Resources>

<StackPanel>

  <!-- Применение стиля к нескольким элементам -->
    
  <Label Content="Пример текста" Style="{StaticResource StylePrimer}"/>
  
  <Button Content="Пример кнопки" Style="{StaticResource StylePrimer}"/>

</StackPanel>

В этом примере мы определяем стиль с именем "StylePrimer", который устанавливает шрифт Arial и синий цвет для всех элементов управления. Затем мы применяем этот стиль к Label и Button, используя атрибут Style.
