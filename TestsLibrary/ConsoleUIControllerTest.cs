using System;
using System.IO;
using System.Diagnostics;
using System.Security;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestsLibrary
{
    public class ConsoleUIControllerTest
    {

        private string TestNoteLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public void LaunchExplorer()
        {
            try
            {
                Process.Start("explorer.exe", TestNoteLocation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void OpenTextWithProgram(string p_exe)
        {
            try
            {
                // The following call to Start succeeds if test.txt exists.
                Console.WriteLine("\nTrying to launch 'OpenWithEnvStart.txt'...");
                Process.Start(p_exe, TestNoteLocation + "//" + "OpenWithProcessStart.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public class RecursiveFileProcessor
    {
        public string TargetDirectory { get; set; }
       
        public void run(List<string> args)
        {
            foreach (string path in args)
            {
                if (File.Exists(path))
                {
                    // This path is a file
                    ProcessFile(path);
                }
                else if (Directory.Exists(path))
                {
                    // This path is a directory
                    ProcessDirectory(path);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }
        }

        // Process all files in the directory passed in, recurse on any directories
        // that are found, and process the files they contain.
        

        public void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }


        // bug: TargetDirectory does not get passed in from subdirectory
        // two potential fixes (maybe)
        //  - make second param for targetDirectory
        //  - update TargetDirectory as subdirectory
        public void ProcessDirectory(Action<string> p_ProcessFile, string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                p_ProcessFile(fileName);

            // Recurse into subdirectories of this directory.

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                ProcessDirectory(p_ProcessFile, subdirectory);
            }
        }

        // Insert logic for processing found files here.
        public void ProcessFile(string path)
        {
            StreamReader SR = new StreamReader(path);
            string line = SR.ReadLine();
            while (line != null)
            {
                Console.WriteLine(line);
                line = SR.ReadLine();
            }
            SR.Close();
            Console.WriteLine("----------------");
            Console.WriteLine("Processed file '{0}'.", path);
            Console.WriteLine("----------------");
        }

        List<string> TableOfContents = new List<string>();

        public void AddFileNameToTableOfContents(string path)
        {
            // Need to save substring of entire path
        }

        public void PrintPath(string path)
        {
            //testing funtional programming by printing the path
            Console.WriteLine(path);
        }
    }
}
