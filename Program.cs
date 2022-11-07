using System.Globalization;
using System.Runtime.CompilerServices;
using static System.Console;

namespace EFcore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Student students = new();
            StudentDbContext dbCtx = new();
            Menu selections = new();
            StudentsList[] studentsLists = {};
            while (true)
            {   
                int selectedIndex = selections.DisplayOptions();

                switch (selectedIndex)
                {
                    case 0:
                        selections.AddStudent(students, dbCtx);
                        break;
                    case 1:
                        selections.ChangeStudent(dbCtx, students);
                        break;
                    case 2:
                        selections.ListOfStudents(dbCtx);
                        break;
                    case 3:
                        WriteLine("\nProgram avslutas...");
                        Environment.Exit(0);
                        break;
                }
            }
        }

    }
}
