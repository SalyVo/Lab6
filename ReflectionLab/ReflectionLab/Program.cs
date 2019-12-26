using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ReflectionLab
{
    class Program
    {
        public class User
        {
            public string Name { get; set; }
            [NumValidation(18)]
            public int Age { get; set; }
            [NumValidation(5)]
            public int ChildCount { get; set; }
            public User(string n, int a)
            {
                Name = n;
                Age = a;
            }
            public User()
            {
                Name = "Игорь";
                Age = 22;
            }
            public void Display()
            {
                Console.WriteLine("Имя: " + Name + ", Возраст:" + Age);
            }
            public int Payment(int hours, int perhour)
            {
                return hours * perhour;
            }
        }
        public static void FieldPropertiesConstructorsInfo<T>(T obj) where T : class
        {
            Type t = typeof(T);
            Console.WriteLine("\n*** Конструкторы ***\n");
            ConstructorInfo[] constructors = t.GetConstructors();
            foreach (ConstructorInfo info in constructors)
            {
                Console.WriteLine("--> Количество параметров: " + info.GetParameters().Count());
                // Вывести параметры конструкторов
                ParameterInfo[] p = info.GetParameters();
                for (int i = 0; i < p.Length; i++)
                {
                    Console.Write(p[i].ParameterType.Name + " " + p[i].Name);
                    if (i + 1 < p.Length) Console.Write(", ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n*** Поля ***\n");
            FieldInfo[] fieldNames = t.GetFields();
            foreach (FieldInfo fil in fieldNames)
                Console.Write("--> " + fil.FieldType + " " + fil.Name + "\n");
            Console.WriteLine("\n*** Свойства ***\n");
            PropertyInfo[] propertyNames = t.GetProperties();
            foreach (PropertyInfo property in propertyNames)
                Console.Write("--> " + property.PropertyType + " " + property.Name + "\n");
        }

        // Данный метод выводит информацию о содержащихся в классе методах
        public static void MethodReflectInfo<T>(T obj) where T : class
        {
            Type t = typeof(T);
            // Получаем коллекцию методов
            MethodInfo[] MArr = t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            Console.WriteLine("*** Список методов класса {0} ***\n", obj.ToString());

            // Вывести методы
            foreach (MethodInfo m in MArr)
            {
                Console.Write(" --> " + m.ReturnType.Name + " \t" + m.Name + "(");
                // Вывести параметры методов
                ParameterInfo[] p = m.GetParameters();
                for (int i = 0; i < p.Length; i++)
                {
                    Console.Write(p[i].ParameterType.Name + " " + p[i].Name);
                    if (i + 1 < p.Length) Console.Write(", ");
                }
                Console.Write(")\n");
            }

        }
        [AttributeUsage(AttributeTargets.Property)]
        public class NumValidationAttribute : System.Attribute
        {
            public int Age { get; set; }

            public NumValidationAttribute()
            { }

            public NumValidationAttribute(int age)
            {
                Age = age;
            }
        }
        static void Main(string[] args)
        {
            User user = new User();
            MethodReflectInfo<User>(user);
            FieldPropertiesConstructorsInfo<User>(user);
            User oleg = new User("Олег", 35);
            Console.WriteLine();
            oleg.Display();
            ValidateUser(oleg);

            //------------------- рефлексия вызов метода
            Type t = typeof(User);
            MethodInfo methodInfo = t.GetMethod("Payment");
            object[] parametersArray = new object[] { 30, 400 };
            Console.WriteLine("Результат вызова метода Payment с параметрами 30 и 400 = " + methodInfo.Invoke(oleg, parametersArray));
            Console.ReadKey();
        }
        static void ValidateUser(User user)
        {
            Type t = typeof(User);
            object[] p = t.GetProperties();
            Console.WriteLine("-------------------------");
            Console.WriteLine("Значения атрибутов:");
            foreach (PropertyInfo i in p)
            {

                object[] attrs = i.GetCustomAttributes(false);
                foreach (NumValidationAttribute attr in attrs)
                {
                    Console.WriteLine(attr.Age);
                }
            }
        }
    }
}
