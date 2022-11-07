using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace EFcore
{
    internal class Menu
    {
        int selectIndex = 0;

        public int DisplayOptions()
        {
            string[] options = { "Registrera ny student", "Ändra Student", "Lista av alla studenter", "Avsluta program" };

            Run(options);
            return selectIndex;
        }
        private int Run(string[] options)
        {
            ConsoleKey KeyPress;
            do
            {
                Clear();
                WriteLine("-----------------------");
                WriteLine("Välj altenativ i menyn.");
                WriteLine("-----------------------");
                for (int i = 0; i < options.Length; i++)
                {
                    string preSelect;

                    if (i == selectIndex)
                    {
                        preSelect = ">";
                        ForegroundColor = ConsoleColor.White;
                        BackgroundColor = ConsoleColor.DarkBlue;
                    }
                    else
                    {
                        preSelect = " ";
                        BackgroundColor = ConsoleColor.Black;
                        ForegroundColor = ConsoleColor.Gray;
                    }
                    WriteLine($"{preSelect}[{options[i]}]");
                }
                ResetColor();

                KeyPress = ReadKey().Key;
               
                if (KeyPress == ConsoleKey.UpArrow)
                {
                    selectIndex--;
                    if (selectIndex == -1)
                    {
                        selectIndex = options.Length - 1;
                    }
                }
                else if (KeyPress == ConsoleKey.DownArrow)
                {
                    selectIndex++;
                    if (selectIndex == options.Length)
                    {
                        selectIndex = 0;
                    }
                }
            } while (KeyPress != ConsoleKey.Enter);
            return selectIndex;
        }
        public void AddStudent(Student students, StudentDbContext dbCtx)
        {
            Clear();
            while (true)
            {
                WriteLine("Ny student");
                WriteLine("----------");
                Write("Ange förnamn: ");
                var firstName = ReadLine();
                if (firstName == "")
                {
                    Clear();
                    WriteLine("Du måste ange ett namn!");
                }
                else
                {
                    Write("Ange efternamn: ");
                    var lastName = ReadLine();
                    Write("Ange stad: ");
                    var city = ReadLine();
                    students = new()
                    {
                        FirstName = TitleCase(firstName),
                        LastName = TitleCase(lastName),
                        City = TitleCase(city)
                    };
                    WriteLine("Tryck enter för att bekräfta / ESC för att avbryt!");
                    ConsoleKey KeyPress = ReadKey().Key;
                    if (KeyPress == ConsoleKey.Enter)
                    {
                        dbCtx = new();
                        dbCtx.Add(students);
                        dbCtx.SaveChanges();
                        WriteLine();
                        WriteLine("Ny student sparade!");
                        WriteLine("Tryck valfri knapp åter till meny...");
                        ReadLine();
                        break;
                    }
                    else if (KeyPress == ConsoleKey.Escape)
                    {
                        WriteLine();
                        WriteLine(" Avbryt!");
                        WriteLine("Tryck valfri knapp åter till meny...");
                        ReadLine();
                        break;
                    }
                }
            }
        }
        public void ChangeStudent(StudentDbContext dbCtx, Student students)
        {
            Clear();
            while (true)
            {
                try
                {
                    string[] options = { "Ändra förnamn", "Ändra efternamn", "Ändra stad", "Radera" };
                    //WriteLine("{0, -6} {1, -15} {2, -15} {3, -10}", "Id", "Namn", "Efternamn", "Stad");
                    //WriteLine("-------------------------------------------");
                    //dbCtx.Students.OrderBy(s => s.StudentId).ToList().ForEach(s => WriteLine("{0, -6} {1, -15} {2, -15} {3, -10}", s.StudentId, s.FirstName, s.LastName, s.City));
                    //WriteLine("-------------------------------------------");

                    Write("Ange student id: ");
                    int search = Convert.ToInt32(ReadLine());
                    var std = dbCtx.Students.Where(s => s.StudentId == search).First<Student>();
                    
                    int selectIndex = Run(options);
                    WriteLine($"\nID {std.StudentId} Namn: {std.FirstName} {std.LastName}  Stad: {std.City}");
                    if (selectIndex == 0)
                    {
                        Write("\nFörnamn: ");
                        var firstName = ReadLine();
                        std.FirstName = TitleCase(firstName);
                        if (firstName == "")
                        {
                            Clear();
                            WriteLine("Du måste ha ett förnamn!");
                            WriteLine("Tryck valfri knapp åter till meny...");
                            ReadLine();
                        }
                        else
                            Confirmation(dbCtx);
                        break;
                    }
                    else if (selectIndex == 1)
                    {
                        Write("\nEfternamn: ");
                        var lastName = ReadLine();
                        std.LastName = TitleCase(lastName);
                        Confirmation(dbCtx);
                        break;
                    }
                    else if (selectIndex == 2)
                    {
                        Write("\nStad: ");
                        var city = ReadLine();
                        std.City = TitleCase(city);
                        Confirmation(dbCtx);
                        break;
                    }
                    else if (selectIndex == 3)
                    {
                        WriteLine("\nÄr du säkert på att radera denna student?\n");
                        dbCtx.Remove(std);
                        Confirmation(dbCtx);
                        break;
                    }
                }
                catch
                {
                    Clear();
                    WriteLine("Id hitta inte i databas");
                }
            }
        }
        public void ListOfStudents(StudentDbContext dbCtx)
        {
            Clear();
            WriteLine("{0, -6} {1, -15} {2, -15} {3, -10}", "Id", "Namn", "Efternamn", "Stad");
            WriteLine("-------------------------------------------");
            dbCtx.Students.OrderBy(s => s.StudentId).ToList().ForEach(s => WriteLine("{0, -6} {1, -15} {2, -15} {3, -10}", s.StudentId, s.FirstName, s.LastName, s.City));
            WriteLine("-------------------------------------------");
            Write("Tryck på valfri knapp för att gå tillbaka...");
            ReadKey();
        }
        static void Confirmation(StudentDbContext dbCtx)
        {
            while (true)
            {
                WriteLine("Tryck enter för att bekräfta / ESC för att avbryt.");
                ConsoleKey KeyPress = ReadKey().Key;
                
                if (KeyPress == ConsoleKey.Enter)
                {
                    dbCtx.SaveChanges();
                    WriteLine("\nÄndringen sparade.");
                    WriteLine("Tryck valfi knapp återgå till meny...");
                    ReadKey();
                    break;
                    
                }
                else if (KeyPress == ConsoleKey.Escape)
                {
                    WriteLine("\nÄndringen avbryts!");
                    WriteLine("Tryck valfri knapp återgå till meny...");
                    ReadKey();
                    break;
                }
            }
        }
        static string TitleCase(string? data)
        {
            TextInfo info = new CultureInfo("en-En", false).TextInfo;
            string txt = info.ToTitleCase(data!);
            return txt;
        }

        
    }
}
