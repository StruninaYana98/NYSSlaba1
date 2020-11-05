using System;
using System.Collections.Generic;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading;

namespace Laba1
{
    class NYSSLaba1
    {
        public static Dictionary<int, ArrayList> people = new Dictionary<int, ArrayList>();
        static void Main(string[] args)
        {
            Console.WriteLine("             ЗДРАВСТВУЙТЕ! ВЫ ОТКРЫЛИ ТЕЛЕФОННУЮ КНИЖКУ");
            Console.WriteLine($"                       Сегодня: {DateTime.Now.ToString("dd.MM.yyyy")}");

            int n;
            do
            {
                bool flag = false;

                do
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\n______________________ВВЕДИТЕ НОМЕР ОПЕРАЦИИ______________________\n\n" +
                                  " 0 - Добавить новую запись \n" +
                                  " 1 - Редактировать существующую запись \n" +
                                  " 2 - Удалить запись \n" +
                                  " 3 - Просмотреть полную информацию по существующей учетной записи \n" +
                                  " 4 - Просмотреть краткую информацию по всем учетным записям\n" +
                                  " 5 - Закрыть телефонную книжку\n\n" +
                                  "                           ОПЕРАЦИЯ №: ");

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
                if (n == 5)
                {
                    Console.WriteLine("\n                   СЕАНС ОКОНЧЕН! ДО СВИДАНИЯ!");
                }
            } while (n != 5);
        }

        static void Entry(int i)
        {
            Console.WriteLine($"_________________________________ЗАПИСЬ №{i}_________________________________\n\n" +
                               " === Для отмены операции нажмите <Esc>, для продолжения нажмите <Enter> ===\n");
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
            } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape);
            if (key.Key == ConsoleKey.Enter)
            {
                string lastname = LastName();
                string firstname = FirstName();
                string middlename = MiddleName();
                long telephone = Telephone();
                string country = Country();
                DateTime birth = Birth();
                string organization = Organization();
                string position = Position();
                string notes = Notes();
                people.Add(i, new ArrayList() { lastname, firstname, middlename, telephone, country, birth, organization, position, notes });
            }
            else if (key.Key == ConsoleKey.Escape) { }
        }

        static string LastName()
        {
            Console.Write($"     ФАМИЛИЯ: ");

            string lastname = ReadindCorrectString("Введите корректную фамилию: ");
            RequiredStringField(lastname, "Введите корректную фамилию: ", out lastname);
            lastname = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(lastname);
            return lastname;
        }
        static string FirstName()
        {
            Console.Write("     ИМЯ: ");
            string firstname = ReadindCorrectString("Введите корректное имя: ");
            RequiredStringField(firstname, "Введите корректное имя: ", out firstname);
            firstname = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(firstname);
            return firstname;
        }
        static string MiddleName()
        {
            Console.Write("     ОТЧЕСТВО (необязательное поле): ");
            string middlename = ReadindCorrectString("Введите корректное отчество: ");

            if (middlename == "")
            {
                middlename = null;
            }
            else
            {
                middlename = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(middlename);
            }
            return middlename;
        }
        static long Telephone()
        {
            Console.Write("     НОМЕР ТЕЛЕФОНА: +7");
            long telephone = ReadingCorrectNumber("Введите корректный номер телефона: ");
            if (telephone < 0)
            {
                RequiredNumberField(telephone, "Введите корректный номер телефона: ", out telephone);
            }
            return telephone;
        }
        static string Country()
        {
            Console.Write("     СТРАНА: ");
            string country = ReadindCorrectString("Введите корректную страну: ");
            RequiredStringField(country, "Введите корректную страну: ", out country);
            country = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(country);
            return country;
        }
        static DateTime Birth()
        {
            Console.Write("     ДАТА РОЖДЕНИЯ в формате ДД.ММ.ГГГГ (необязательное поле): ");
            DateTime birth = ReadingCorrectDate("Введите корректную дату: ");
            return birth;
        }
        static string Organization()
        {
            Console.Write("     ОРГАНИЗАЦИЯ (необязательное поле): ");
            string organization = Console.ReadLine();
            if (organization == "")
            {
                organization = null;
            }
            return organization;
        }
        static string Position()
        {
            Console.Write("     ДОЛЖНОСТЬ (необязательное поле): ");
            string position = ReadindCorrectString("Введите корректную должность: ");
            if (position == "")
            {
                position = null;
            }
            return position;
        }
        static string Notes()
        {
            Console.Write("     ЗАМЕТКИ (необязательное поле): ");
            string notes = Console.ReadLine();
            if (notes == "")
            {
                notes = null;
            }
            return notes;
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
                    Console.Write("\n_______ВВЕДИТЕ НОМЕР УЧЕТНОЙ ЗАПИСИ, КОТОРУЮ ВЫ ХОТЕЛИ БЫ ОТРЕДАКТИРОВАТЬ_______\n" +
                                  $" (всего {people.Count} записей в телефонной книжке)\n\n" +
                                   $" === Для отмены операции нажмите <Esc>,  для продолжения нажмите <Enter> ===\n\n");
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
                            Console.WriteLine($" Для редактирования введите 3   | НОМЕР ТЕЛЕФОНА: +7{people[i][3]}");
                            Console.WriteLine($" Для редактирования введите 4   | СТРАНА: {people[i][4]}");
                            DateTime date = (DateTime)people[i][5];
                            if (date == default(DateTime))
                            {
                                Console.WriteLine($" Для редактирования введите 5   | ДАТА РОЖДЕНИЯ:");
                            }
                            else
                            {
                                Console.WriteLine($" Для редактирования введите 5   | ДАТА РОЖДЕНИЯ: {date.ToString("dd.MM.yyyy")}");
                            }
                            Console.WriteLine($" Для редактирования введите 6   | ОРГАНИЗАЦИЯ: {people[i][6]}");
                            Console.WriteLine($" Для редактирования введите 7   | ДОЛЖНОСТЬ: {people[i][7]}");
                            Console.WriteLine($" Для редактирования введите 8   | ЗАМЕТКИ: {people[i][8]}");
                            Console.WriteLine(" Для редактирования всех полей записи введите 9");
                            Console.WriteLine(" Для завершения редактирования записи введите 10\n");
                            Console.Write(" ВЫПОЛНИТЬ ОПЕРАЦИЮ № ");
                            string operation = Console.ReadLine();

                            if (int.TryParse(operation, out operationNumber) && (operationNumber >= 0) && (operationNumber <= 10))
                            {
                                flag = true;
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("! ! ! НЕКОРРЕКТНЫЙ НОМЕР ОПЕРАЦИИ ! ! !");
                                Console.ResetColor();
                            }

                        } while (flag == false);
                        switch (operationNumber)
                        {
                            case 0:
                                people[i][0] = LastName();
                                break;
                            case 1:
                                people[i][1] = FirstName();
                                break;
                            case 2:
                                people[i][2] = MiddleName();
                                break;
                            case 3:
                                people[i][3] = Telephone();
                                break;
                            case 4:
                                people[i][4] = Country();
                                break;
                            case 5:
                                people[i][5] = Birth();
                                break;
                            case 6:
                                people[i][6] = Organization();
                                break;
                            case 7:
                                people[i][7] = Position();
                                break;
                            case 8:
                                people[i][8] = Notes();
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
                Console.WriteLine("! ! ! РЕДАКТИРОВАНИЕ НЕВОЗМОЖНО. В ТЕЛЕФОННОЙ КНИЖКЕ НЕТ ЗАПИСЕЙ ! ! !");
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
                    Console.Write("\n_________ВВЕДИТЕ НОМЕР УЧЕТНОЙ ЗАПИСИ, КОТОРУЮ ВЫ ХОТЕЛИ БЫ УДАЛИТЬ_________\n" +
                                   $" (всего {people.Count} записей в телефонной книжке)\n\n" +
                                   $" === Для отмены операции нажмите <Esc>,  для продолжения нажмите <Enter> ===\n\n");
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
                            Console.WriteLine($"\n! ! ! Ошибка ! ! ! Запись с номером {s} отсутствует в телефонной книжке");
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
                    Console.WriteLine($"_____________________Запись №{i} успешно удалена!_____________________");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! ! ! УДАЛЕНИЕ НЕВОЗМОЖНО. В ТЕЛЕФОННОЙ КНИЖКЕ НЕТ ЗАПИСЕЙ ! ! !");
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
                                   $" === Для отмены операции нажмите <Esc>,  для продолжения нажмите <Enter> ===\n\n");
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
                    Console.WriteLine($"     НОМЕР ТЕЛЕФОНА: +7{people[i][3]}");
                    Console.WriteLine($"     СТРАНА: {people[i][4]}");
                    DateTime date = (DateTime)people[i][5];
                    if (date == default(DateTime))
                    {
                        Console.WriteLine($"     ДАТА РОЖДЕНИЯ:");
                    }
                    else
                    {
                        Console.WriteLine($"     ДАТА РОЖДЕНИЯ: {date.ToString("dd.MM.yyyy")}");
                    }
                    Console.WriteLine($"     ОРГАНИЗАЦИЯ: {people[i][6]}");
                    Console.WriteLine($"     ДОЛЖНОСТЬ: {people[i][7]}");
                    Console.WriteLine($"     ЗАМЕТКИ: {people[i][8]}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! ! ! ПРОСМОТР НЕВОЗМОЖЕН. В ТЕЛЕФОННОЙ КНИЖКЕ НЕТ ЗАПИСЕЙ ! ! !");
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
                    Console.WriteLine("+7" + people[i][3]);
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
                    if (!(Char.IsLetter(enteredString[i]) || enteredString[i] == '-' || enteredString[i] == ' '))   // покрывает случаи составных имен и названий стран
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

            return enteredString;
        }
        static long ReadingCorrectNumber(string s)
        {
            bool flag = false;
            long n = -1;
            do
            {
                string number = Console.ReadLine();

                if ((Int64.TryParse(number, out n) && number.Length == 10) || number == "")
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
                    Console.Write(" +7");
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
                else if (DateTime.TryParse(d, out date) && Regex.Match(d, @"\d{2}\.\d{2}\.\d{4}").Success)
                {
                    flag = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"! ! ! Ошибка ! ! ! {s}");
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
                Console.Write(" +7");
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
