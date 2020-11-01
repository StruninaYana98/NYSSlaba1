using System;
using System.Collections.Generic;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Xml;

namespace ConsoleApp1
{
    class Program

    {
        public static Dictionary<int, ArrayList> people = new Dictionary<int, ArrayList>();
        static void Main(string[] args)
        {


            int n;
            do
            {
                bool flag = false;

                do
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\n___________________ВВЕДИТЕ НОМЕР ОПЕРАЦИИ___________________\n\n" +
                                  " 0 - Добавить новую запись \n" +
                                  " 1 - Редактировать существующую запись \n" +
                                  " 2 - Удалить запись \n" +
                                  " 3 - Просмотреть полную информацию по существующей учетной записи \n" +
                                  " 4 - Просмотреть краткую информацию по всем учетным записям\n" +
                                  " 5 - Закрыть телефонную книжку\n\n" +
                                  "                      ОПЕРАЦИЯ №: ");

                    string tryN = Console.ReadLine();
                    if (int.TryParse(tryN, out n) && (n >= 0) && (n <= 5))
                    {
                        flag = true;
                    }
                    Console.WriteLine();
                    Console.ResetColor();

                } while (flag == false);
                if (n == (int)Actions.Entry)
                {
                    Entry(people.Count + 1);
                }
                if (n == (int)Actions.Editing)
                {
                    Editing();
                }
                if (n == (int)Actions.Delete)
                {
                    Delete();
                }
                if (n == (int)Actions.View)
                {
                    View();
                }
                if (n == (int)Actions.ShortView)
                {
                    ShortView();
                }
            } while (n != 5);
        }

        static void Entry(int i)
        {
            Console.WriteLine($"________________________________ЗАПИСЬ №{i}________________________________\n\n" +
                               "!!!Для отмены операции нажмите <Esc>, для продолжения нажмите <Enter> !!!\n");
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape);
            if (key.Key == ConsoleKey.Enter)
            {

                Console.Write($"     ФАМИЛИЯ: ");

                string lastname = ReadindCorrectString("Введите корректную фамилию: ");
                RequiredStringField(lastname, "Введите корректную фамилию: ", out lastname);

                Console.Write("     ИМЯ: ");
                string firstname = ReadindCorrectString("Введите корректное имя: ");
                RequiredStringField(firstname, "Введите корректное имя: ", out firstname);

                Console.Write("     ОТЧЕСТВО (необязательное поле): ");
                string patronymic = ReadindCorrectString("Введите корректное отчество: ");

                if (patronymic == "")
                {
                    patronymic = null;
                }

                Console.Write("     НОМЕР ТЕЛЕФОНА: ");
                long telephone = ReadingCorrectNumber("Введите корректный номер телефона: ");
                if (telephone < 0)
                {
                    RequiredNumberField(telephone, "Введите корректный номер телефона: ", out telephone);
                }

                Console.Write("     СТРАНА: ");
                string country = ReadindCorrectString("Введите корректную страну: ");
                RequiredStringField(country, "Введите корректную страну: ", out country);

                Console.Write("     ДАТА РОЖДЕНИЯ (необязательное поле): ");
                DateTime birth = ReadingCorrectDate("Введите корректную дату: ");

                Console.Write("     ОРГАНИЗАЦИЯ (необязательное поле): ");
                string organization = Console.ReadLine();
                if (organization == "")
                {
                    organization = null;
                }
                Console.Write("     ДОЛЖНОСТЬ (необязательное поле): ");
                string position = ReadindCorrectString("Введите корректную должность: ");
                if (position == "")
                {
                    position = null;
                }
                Console.Write("     ЗАМЕТКИ (необязательное поле): ");
                string notes = Console.ReadLine();
                if (notes == "")
                {
                    notes = null;
                }
                people.Add(i, new ArrayList() { lastname, firstname, patronymic, telephone, country, birth, organization, position, notes });
            }
            else if (key.Key == ConsoleKey.Escape)
            {

            }
        }
        static void Editing()
        {
            if (people.Count > 0)
            {
                int i = 0;
                bool flag = false;
                bool isEditingPossible = false;
                do
                {
                    Console.Write("\n________ВВЕДИТЕ НОМЕР УЧЕТНОЙ ЗАПИСИ, КОТОРУЮ ВЫ ХОТЕЛИ БЫ ОТРЕДАКТИРОВАТЬ________\n" +
                                  $" (всего {people.Count} записей в телефонной книжке)\n\n" +
                                   $"! ! ! Для отмены операции нажмите <Esc>,  для продолжения нажмите <Enter> ! ! !\n\n");
                    ConsoleKeyInfo key;
                    do
                    {
                        key = Console.ReadKey();
                    } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        isEditingPossible = true;
                        Console.Write($"     ЗАПИСЬ № ");
                        string s = Console.ReadLine();
                        if (int.TryParse(s, out i) && i >= 1 && i <= people.Count)
                        {
                            flag = true;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"! ! ! Ошибка ! ! ! Запись с номером {s} отсутствует в телефонной книжке");
                            Console.ResetColor();
                        }
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        flag = true;
                        isEditingPossible = false;
                    }
                } while (flag == false);
                if (isEditingPossible)
                {
                    int operationNumber;
                    do
                    {

                        flag = false;

                        do
                        {
                            Console.WriteLine($"\n ТЕКУЩЕЕ СОСТОЯНИЕ ЗАПИСИ №{i}");
                            Console.WriteLine($" Для редактирования введите 0   | ФАМИЛИЯ: {people[i][0]}");
                            Console.WriteLine($" Для редактирования введите 1   | ИМЯ: {people[i][1]}");
                            Console.WriteLine($" Для редактирования введите 2   | ОТЧЕСТВО: {people[i][2]}");
                            Console.WriteLine($" Для редактирования введите 3   | НОМЕР ТЕЛЕФОНА: {people[i][3]}");
                            Console.WriteLine($" Для редактирования введите 4   | СТРАНА: {people[i][4]}");
                            if ((DateTime)people[i][5] == default(DateTime))
                            {
                                Console.WriteLine($" Для редактирования введите 5   | ДАТА РОЖДЕНИЯ:");
                            }
                            else
                            {
                                Console.WriteLine($" Для редактирования введите 5   | ДАТА РОЖДЕНИЯ: {people[i][5]}");
                            }
                            Console.WriteLine($" Для редактирования введите 6   | ОРГАНИЗАЦИЯ: {people[i][6]}");
                            Console.WriteLine($" Для редактирования введите 7   | ДОЛЖНОСТЬ: {people[i][7]}");
                            Console.WriteLine($" Для редактирования введите 8   | ЗАМЕТКИ: {people[i][1]}");
                            Console.WriteLine(" Для редактирования всех полей записи введите 9");
                            Console.WriteLine(" Для завершения редактирования записи введите 10\n");
                            Console.Write(" ВЫПОЛНИТЬ ОПЕРАЦИЮ № ");
                            string operation = Console.ReadLine();

                            if (int.TryParse(operation, out operationNumber) && (operationNumber >= 0) && (operationNumber <= 10))
                            {
                                flag = true;
                            }

                        } while (flag == false);
                        switch (operationNumber)
                        {
                            case 0:
                                Console.Write("\n ФАМИЛИЯ: ");
                                string lastname = ReadindCorrectString("Введите корректную фамилию: ");
                                RequiredStringField(lastname, "Введите корректную фамилию: ", out lastname);
                                people[i][0] = lastname;
                                break;
                            case 1:
                                Console.Write("\n ИМЯ: ");
                                string firstname = ReadindCorrectString("Введите корректное имя: ");
                                RequiredStringField(firstname, "Введите корректное имя: ", out firstname);
                                people[i][1] = firstname;
                                break;
                            case 2:
                                Console.Write("\n ОТЧЕСТВО (необязательное поле): ");
                                string patronymic = ReadindCorrectString("Введите корректное отчество: ");
                                if (patronymic == "")
                                {
                                    patronymic = null;
                                }
                                people[i][2] = patronymic;
                                break;
                            case 3:
                                Console.Write("\n НОМЕР ТЕЛЕФОНА: ");
                                long telephone = ReadingCorrectNumber("Введите корректный номер телефона: ");
                                if (telephone < 0)
                                {
                                    RequiredNumberField(telephone, "Введите корректный номер телефона: ", out telephone);
                                }
                                people[i][3] = telephone;
                                break;

                            case 4:
                                Console.Write("\n СТРАНА: ");
                                string country = ReadindCorrectString("Введите корректную страну: ");
                                RequiredStringField(country, "Введите корректную страну: ", out country);
                                people[i][4] = country;
                                break;
                            case 5:
                                Console.Write("\n ДАТА РОЖДЕНИЯ (необязательное поле): ");
                                DateTime birth = ReadingCorrectDate("Введите корректную дату: ");
                                people[i][5] = birth;
                                break;
                            case 6:
                                Console.Write("\n ОРГАНИЗАЦИЯ (необязательное поле): ");
                                string organization = Console.ReadLine();
                                if (organization == "")
                                {
                                    organization = null;
                                }
                                people[i][6] = organization;
                                break;
                            case 7:

                                Console.Write("\n ДОЛЖНОСТЬ (необязательное поле): ");
                                string position = ReadindCorrectString("Введите корректную должность: ");
                                if (position == "")
                                {
                                    position = null;
                                }
                                people[i][7] = position;
                                break;
                            case 8:
                                Console.Write("\n ЗАМЕТКИ (необязательное поле): ");
                                string notes = Console.ReadLine();
                                if (notes == "")
                                {
                                    notes = null;
                                }
                                people[i][8] = notes;
                                break;
                            case 9:
                                Dictionary<int, ArrayList> list = new Dictionary<int, ArrayList>();
                                list.Add(i, people[i]);
                                people.Remove(i);
                                Entry(i);
                                if (!people.ContainsKey(i))
                                {
                                    people.Add(i, list[i]);
                                }
                                list.Clear();
                                break;
                        }
                    } while (operationNumber != 10);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! ! ! РЕДАКТИРОВАНИЕ НЕВОЗМОЖНО, ТАК КАК В ТЕЛЕФОННОЙ КНИЖКЕ НЕТ ЗАПИСЕЙ ! ! !");
                Console.ResetColor();
            }
        }
        static void Delete()
        {

            if (people.Count > 0)
            {
                int i = 0;
                bool flag = false;
                bool isDeletePossible = false;
                do
                {
                    Console.Write("\n__________ВВЕДИТЕ НОМЕР УЧЕТНОЙ ЗАПИСИ, КОТОРУЮ ВЫ ХОТЕЛИ БЫ УДАЛИТЬ__________\n" +
                                   $" (всего {people.Count} записей в телефонной книжке)\n\n" +
                                   $"! ! ! Для отмены операции нажмите <Esc>,  для продолжения нажмите <Enter> ! ! !\n");
                    ConsoleKeyInfo key;
                    do
                    {
                        key = Console.ReadKey();
                    } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        isDeletePossible = true;
                        Console.Write($"     ЗАПИСЬ № ");
                        string s = Console.ReadLine();

                        if (int.TryParse(s, out i) && i >= 1 && i <= people.Count)
                        {
                            flag = true;

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"! ! ! Ошибка ! ! ! Запись с номером {s} отсутствует в телефонной книжке");
                            Console.ResetColor();
                        }
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        flag = true;
                        isDeletePossible = false;
                    }

                } while (flag == false);
                if (isDeletePossible)
                {
                    for (int k = i; k < people.Count; k++)
                    {
                        people[k] = people[k + 1];
                    }
                    people.Remove(people.Count);
                    Console.WriteLine($"__________________Запись №{i} успешно удалена!__________________");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! ! ! УДАЛЕНИЕ НЕВОЗМОЖНО, ТАК КАК В ТЕЛЕФОННОЙ КНИЖКЕ НЕТ ЗАПИСЕЙ ! ! !");
                Console.ResetColor();
            }
        }
        static void View()
        {
            if (people.Count > 0)
            {
                int i = 0;
                bool flag = false;
                bool isViewPossible = false;
                do
                {
                    Console.Write("\n__________ВВЕДИТЕ НОМЕР УЧЕТНОЙ ЗАПИСИ, КОТОРУЮ ВЫ ХОТЕЛИ БЫ ПРОСМОТРЕТЬ__________\n" +
                                   $" (всего {people.Count} записей в телефонной книжке)\n\n" +
                                   $"! ! ! Для отмены операции нажмите <Esc>,  для продолжения нажмите <Enter> ! ! !\n\n");
                    ConsoleKeyInfo key;
                    do
                    {
                        key = Console.ReadKey();
                    } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape);
                    if (key.Key == ConsoleKey.Enter)
                    {
                        isViewPossible = true;
                        Console.Write($"     ЗАПИСЬ № ");
                        string s = Console.ReadLine();

                        if (int.TryParse(s, out i) && i >= 1 && i <= people.Count)
                        {
                            flag = true;

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"! ! ! Ошибка ! ! ! Запись с номером {s} отсутствует в телефонной книжке");
                            Console.ResetColor();
                        }
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        flag = true;
                        isViewPossible = false;
                    }

                } while (flag == false);
                if (isViewPossible)
                {

                    Console.WriteLine($"\n________________________________ЗАПИСЬ №{i}________________________________\n");
                    Console.WriteLine($"     ФАМИЛИЯ: {people[i][0]}");
                    Console.WriteLine($"     ИМЯ: {people[i][1]}");
                    Console.WriteLine($"     ОТЧЕСТВО: {people[i][2]}");
                    Console.WriteLine($"     НОМЕР ТЕЛЕФОНА: {people[i][3]}");
                    Console.WriteLine($"     СТРАНА: {people[i][4]}");
                    if ((DateTime)people[i][5] == default(DateTime))
                    {
                        Console.WriteLine($"     ДАТА РОЖДЕНИЯ:");
                    }
                    else
                    {
                        Console.WriteLine($"     ДАТА РОЖДЕНИЯ: {people[i][5]}");
                    }
                    Console.WriteLine($"     ОРГАНИЗАЦИЯ: {people[i][6]}");
                    Console.WriteLine($"     ДОЛЖНОСТЬ: {people[i][7]}");
                    Console.WriteLine($"     ЗАМЕТКИ: {people[i][8]}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! ! ! ПРОСМОТР НЕВОЗМОЖЕН, ТАК КАК В ТЕЛЕФОННОЙ КНИЖКЕ НЕТ ЗАПИСЕЙ ! ! !");
                Console.ResetColor();
            }
        }
        static void ShortView()
        {
            if (people.Count > 0)
            {
                Console.WriteLine("__________________________________ПОЛНЫЙ СПИСОК ЗАПИСЕЙ__________________________________");
                Console.WriteLine(" №       ФАМИЛИЯ                       ИМЯ                           НОМЕР ТЕЛЕФОНА");
                for (int i = 1; i <= people.Count; i++)
                {
                    Console.Write($" {i}");
                    string n = Convert.ToString(i);
                    for (int k = 0; k < 8 - n.Length; k++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(people[i][0]);
                    for (int k = 0; k < 30 - people[i][0].ToString().Length; k++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(people[i][1]);
                    for (int k = 0; k < 30 - people[i][1].ToString().Length; k++)
                    {
                        Console.Write(" ");
                    }
                    Console.WriteLine(people[i][3]);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! ! ! ТЕЛЕФОННАЯ КНИЖКА ПУСТА ! ! !");
                Console.ResetColor();
            }
        }

        static string ReadindCorrectString(string s)
        {
            string enteredString;
            bool flag = false;
            do
            {
                enteredString = Console.ReadLine();
                bool isCorrect = true;
                for (int i = 0; i < enteredString.Length; i++)
                {
                    if (!(Char.IsLetter(enteredString[i]) || enteredString[i] == ' ' || enteredString[i] == '-'))
                    {
                        isCorrect = false;
                    }
                }
                if (isCorrect == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"! ! ! Ошибка ! ! ! {s}");
                    Console.ResetColor();
                }
                else
                {
                    flag = true;
                }
            } while (flag == false);
            if (enteredString.Length > 1)
            {
                enteredString = enteredString.Substring(0, 1).ToUpper() + enteredString.Substring(1, enteredString.Length - 1).ToLower();
            }
            else if (enteredString.Length == 1)
            {
                enteredString = enteredString.ToUpper();
            }
            return enteredString;
        }
        static long ReadingCorrectNumber(string s)
        {
            bool flag = false;
            long n = -1;
            do
            {
                string number = Console.ReadLine();

                if (Int64.TryParse(number, out n) || number == "")
                {
                    flag = true;
                    if (number == "")
                    {
                        n = -1;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"! ! ! Ошибка ! ! ! {s}");
                    Console.ResetColor();
                }
            } while (flag == false);

            return n;
        }
        static DateTime ReadingCorrectDate(string s)
        {
            bool flag = false;
            DateTime date;
            do
            {
                string d = Console.ReadLine();
                if (d == "")
                {
                    date = default(DateTime);
                    flag = true;
                }
                else if (DateTime.TryParse(d, out date))
                {
                    flag = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"! ! ! Ошибка ! ! ! {s}");
                    Console.ResetColor();
                }
            } while (flag == false);
            return date;
        }
        static void RequiredStringField(string s, string isnotcorrect, out string field)
        {
            while (s == "")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"~~~ Это обязательное поле! ~~~ \n! ! ! Ошибка ! ! ! {isnotcorrect}");
                Console.ResetColor();
                s = ReadindCorrectString(isnotcorrect);
            }
            field = s;
        }
        static void RequiredNumberField(long n, string isnotcorrect, out long field)
        {
            while (n < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"~~~ Это обязательное поле! ~~~ \n! ! ! Ошибка ! ! ! {isnotcorrect}");
                Console.ResetColor();
                n = ReadingCorrectNumber(isnotcorrect);
            }
            field = n;
        }
    }


    public enum Actions
    {
        Entry,
        Editing,
        Delete,
        View,
        ShortView
    }
}
