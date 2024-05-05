![D5JRRJ~1](https://github.com/Luxurys-Lukuchi/CarsBD/assets/146846830/0c11d5e1-525d-47b7-9672-1dd28c32687c)



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

MainWindow *-- Car : Композиция

MainWindow --> DatabaseManager : Ассоциация

MainWindow --> SearchCarWindow : Зависимость

MainWindow --> DeleteCarWindow : Зависимость

MainWindow --> AddCarWindow : Зависимость

SearchCarWindow --> DatabaseManager : Ассоциация

DeleteCarWindow --> DatabaseManager : Ассоциация

AddCarWindow --> Car : Ассоциация

@enduml
