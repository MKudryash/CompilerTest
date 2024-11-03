//Модель для  кода инициализации переменных в генерируемом коде
public class ArgumentsFunction
{
    //Конструктор для тестов (надо наверное будет удалить)
    public ArgumentsFunction(string typeVariable, string nameVariavle)
    {
        TypeVariable = typeVariable;
        NameVariable = nameVariavle;
    }

    //Тип переменной
    public string TypeVariable { get; set; }
    //Имя переменной
    public string NameVariable { get; set; }
}