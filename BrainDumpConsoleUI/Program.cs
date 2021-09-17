using System;
using System.IO;
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
        public static string NoteLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BrainDump");
        public static string AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AppData");
        public static string DirectoryHistoryFilename = "DirectoryHistory.txt";
        public static FileManager FM { get; set; }
        private static List<string> TOC = new List<string>();
        private static StringFudger stringFudger = new StringFudger();

        static void Main(string[] args)
        {
            try
            {
                FM = new FileManager(NoteLocation, AppDataDirectory, DirectoryHistoryFilename);
                FM.Init();
                RunIntro(Title);
                TOC = FM.getTOC();
                LaunchHome();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
            Console.WriteLine($"Working in: {FM.TargetDirectory}");
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
            Console.WriteLine($"\n\tType folder path or hit enter to save new note here: {FM.TargetDirectory}");
            Console.Write("> ");
            string entry = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entry))
            {
                NoteLocation = entry;
                FM.UpdateDirectoryHistoryFile(NoteLocation);
                FM.TargetDirectory = NoteLocation;
            }

            Console.WriteLine("\n\tEnter filename: ");
            Console.Write("> ");
            string filename = Console.ReadLine();

            Console.WriteLine("\n\tEnter text: (enter \"^S\" to save)");
            Console.Write("> ");
            string text = createTextToWrite("^S");
            // Trim white space
            //text.TrimEnd();

            if (string.IsNullOrWhiteSpace(filename))
            {
                FM.AddTextFile(FM.TargetDirectory, text);
            }
            else
            {
                FM.AddTextFile(FM.TargetDirectory, text, filename);
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
