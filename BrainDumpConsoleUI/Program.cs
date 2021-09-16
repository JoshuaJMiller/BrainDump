using System;
using System.Diagnostics;
using System.Security;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestsLibrary;
using ConsoleHelperLibrary;
using NotesManagerLibrary;

namespace BrainDumpConsoleUI
{
    class Program
    {
        private static string Title = "---------------- BrainDump 1.0 ----------------";
        private static string DefaultFolderName = "BrainDump";
        public static string NoteLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private static FileManager FM = new FileManager();
        private static List<string> TOC = new List<string>();
        private static StringFudger stringFudger = new StringFudger();

        static void Main(string[] args)
        {
            try
            {
                RunIntro(Title);
                InitializeFileManager();
                TOC = FM.getTOC();
                LaunchHome();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //RunTests();
        }
        private static void RunTests()
        {
            //List<string> stringList = new List<string> { "ricky", "bobby", "sue" };
            //List<int> intList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 };
            // ConsoleUIControllerTest ConsoleUIControllerTest1 = new ConsoleUIControllerTest();
            Printer Printer1 = new Printer();

            //Printer1.PrintList<string>(stringList);
            //Printer1.PrintList(intList);
            //Console.WriteLine("----------------");

            //RecursiveFileProcessor fileProcessor = new RecursiveFileProcessor();
            //List<string> Paths = new List<string> { "C:\\Users\\joshu\\source\\repos\\Education\\C#\\Demo\\TextFiles\\test100.txt", "C:\\Users\\joshu\\source\\repos\\Education\\C#\\Demo\\TextFiles\\test9101.txt", "C:\\Users\\joshu\\source\\repos\\Education\\C#\\Demo\\TextFiles\\test5.txt", "C:\\Users\\joshu\\source\\repos\\Education\\C#\\Demo\\TextFiles\\test9101.txt", "C:\\Users\\joshu\\source\\repos\\Education\\C#\\Demo\\TextFiles\\TestFolder" };
            ////fileProcessor.run(Paths);
            //fileProcessor.ProcessDirectory("C:\\Users\\joshu\\source\\repos\\Education\\C#\\Demo\\TextFiles\\TestFolder");

            //ConsoleUIControllerTest1.LaunchExplorer();
            //ConsoleUIControllerTest1.OpenTextWithProgram("C:\\Program Files\\Sublime Text\\sublime_text.exe");


            //set TargetDirectory
            //fileProcessor.TargetDirectory = "C:\\Users\\joshu\\source\\repos\\Education\\C#\\Demo\\TextFiles";
            //fileProcessor.ProcessDirectory(fileProcessor.PrintPath, fileProcessor.TargetDirectory);
            //Console.WriteLine(fileProcessor.TargetDirectory);
            //fileProcessor.ProcessDirectory(fileProcessor.TargetDirectory);

            FileManager FM = new FileManager();
            FM.TargetDirectory = "C:\\Users\\joshu\\source\\repos\\Education\\C#\\Demo\\TextFiles";
            //FM.ProcessDirectory(PrintFileName, FM.TargetDirectory);
            FM.ProcessDirectory(FM.AddFileNameToTableOfContents, FM.TargetDirectory);
            List<string> TOC = new List<string>(FM.getTOC());
            Printer1.PrintList(TOC);

        }

        private static void InitializeFileManager()
        {
            // FUTURE: add local folder with xml containing recent directories used
            //
            // if target directory xml exists, initialize FM with the last directory notes were saved to
            if (/*some local xml exists*/false)
            {
                string currentDirectory = /*parsingFuntionToGetDir();*/
                NoteLocation = currentDirectory;
                FM.Init(NoteLocation);
            }

            // if not, use default and add BrainDump folder
            else
            {
                FM.Init(NoteLocation, DefaultFolderName);
                NoteLocation = FM.TargetDirectory;
            }
        }
        private static void RunIntro(string p_text)
        {
            Console.Write(new string(' ', (Console.WindowWidth - p_text.Length) / 2));
            Console.WriteLine(p_text);
            Console.WriteLine();
        }

        private static void LaunchHome()
        {
            Console.WriteLine($"Working in: {NoteLocation}");
            Console.WriteLine();
            Console.WriteLine("\tMenu:");
            Console.WriteLine();
            CommandsAvailable();
            ReadCommand();
        }

        private static void ReadCommand()
        {
            Console.Write("> ");
            string Command = Console.ReadLine();

            switch (Command.ToLower())
            {

                case "new":
                    NewNote();
                    //Main(null);
                    break;
                default:
                    CommandsAvailable();
                    //Main(null);
                    break;
            }
        }

        private static void NewNote()
        {
            Console.WriteLine($"\n\tType folder path or hit enter to save new note here: {NoteLocation}");
            Console.Write("> ");
            string entry = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entry))
            {
                NoteLocation = entry;
            }

            Console.WriteLine("\n\tEnter filename: ");
            Console.Write("> ");
            string filename = Console.ReadLine();

            Console.WriteLine("\n\tEnter text: (enter \"^S\" to save)");
            Console.Write("> ");
            string text = createTextToWrite("^S");
            //trim white space?


            if (string.IsNullOrWhiteSpace(filename))
            {
                FM.AddTextFile(NoteLocation, text);
            }
            else
            {
                FM.AddTextFile(NoteLocation, text, filename);
            }
        }

        private static string createTextToWrite(string p_saveKey)
        {
            string buff = "";
            string text = "";
            
            while (!buff.Contains(p_saveKey) && !buff.Contains(p_saveKey.ToLower()))
            {
                buff = Console.ReadLine();
                text += buff;
                if (text != "")
                {
                    text += "\n";
                }
                if (text.Contains(p_saveKey))
                {
                    text = stringFudger.removeSubString(text, p_saveKey);
                }
                else if (text.Contains(p_saveKey.ToLower()))
                {
                    text = stringFudger.removeSubString(text, p_saveKey.ToLower());
                }
            }

            return text;
        }

        private static void CommandsAvailable()
        {
            DisplayTOC();
            Console.WriteLine("\tnew - Create a new note");
            //Console.WriteLine("\tcd - Change directory");
        }
        private static void DisplayTOC()
        {
            if (TOC == null || !TOC.Any())
            {
                Console.WriteLine("No notes yet! (enter \"new\")");
            }
            else
            {
                Printer TOCPrinter = new Printer();
                TOCPrinter.PrintIndentedList<string>(TOC);
            }
        }
    }
    
}
