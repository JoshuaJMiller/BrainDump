using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesManagerLibrary
{
    public class FileManager
    {
        public string TargetDirectory { get; set; }
        private List<string> TableOfContents = new List<string>();
        public List<string> FileNames = new List<string>();
        public int TOCCount = 0;
        private StreamWriter SW;
        private StreamReader SR;

        public void Init()
        {
            ProcessDirectory(AddFileNameToTableOfContents, TargetDirectory);
            TOCCount = 0;
        }

        public void Init(string p_targetDirectory)
        {
            // Assign directory
            TargetDirectory = p_targetDirectory;

            // Create TOC from currect directory
            ProcessDirectory(AddFileNameToTableOfContents, TargetDirectory, false);
            
            // Ensure TOCCOunt is 0
            TOCCount = 0;
        }

        public void Init(string p_targetDirectory, string p_folderName)
        {
            // Add "BrainDump" Folder if it doesn't exist
            if (!Directory.Exists(p_folderName))
            {
                TargetDirectory = CreateAndGetNewFolder(p_targetDirectory, p_folderName);
            }
            else
            {
                TargetDirectory = Path.Combine(p_targetDirectory, p_folderName);
            }

            // Create TOC from currect directory
            ProcessDirectory(AddFileNameToTableOfContents, TargetDirectory, false);

            // Ensure TOCCOunt is 0
            TOCCount = 0;
        }
        public List<string> getTOC()
        {
            return TableOfContents;
        }

        public void AddFileNameToTableOfContents(string p_path, string p_targetDirectory)
        {
            string sub = TOCCount + " - " + p_path.Substring(p_targetDirectory.Length + 1);
            TableOfContents.Add(sub);
        }

        public void AddFileNameToList(string p_path, string p_targetDirectory)
        {
            string fileName = p_path.Substring(p_targetDirectory.Length + 1);
            FileNames.Add(fileName);
        }

        public void ProcessDirectory(Action<string, string> p_ProcessFile, string p_targetDirectory, bool p_includeSubdirectories = true)
        {
            if (p_includeSubdirectories == false)
            {
                // Process the list of files found in the directory.
                string[] fileEntries = Directory.GetFiles(p_targetDirectory);
                foreach (string fileName in fileEntries)
                {
                    TOCCount++;
                    p_ProcessFile(fileName, p_targetDirectory);
                }
            }
            else
            {
                // Process the list of files found in the directory.
                string[] fileEntries = Directory.GetFiles(p_targetDirectory);
                foreach (string fileName in fileEntries)
                {
                    TOCCount++;
                    p_ProcessFile(fileName, p_targetDirectory);
                }

                // Recurse into subdirectories of this directory.
                string[] subdirectoryEntries = Directory.GetDirectories(p_targetDirectory);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    ProcessDirectory(p_ProcessFile, subdirectory);
                }
            }
        }

        private string RemoveExt(string p_filename, string p_extToRemove)
        {
            int startIndex = p_filename.IndexOf(p_extToRemove);
            if (startIndex != -1)
            {
                // found it
                // remove it
                string name = p_filename.Remove(startIndex, p_extToRemove.Length);
                return name;
            }
            else
            {
                return p_filename;
            }

        }

        public void AddTextFile(string p_directory, string p_text, string p_filename = "New Note")
        {
            // remove ".txt" if entered
            string desiredName = RemoveExt(p_filename, ".txt");

            // Create a unique name if p_filename already exists
            string filename = CreateUniqueFilename(p_directory, desiredName);

            SW = new StreamWriter($"{TargetDirectory}\\{filename}");
            try
            {
                //sw.Write(p_text);
                SW.Write(p_text);
            }
            catch (Exception)
            {
                throw;
            }
            //close stream writer regardless of exception
            finally
            {
                SW.Close();
            }
        }
        private string CreateUniqueFilename(string p_directory, string p_filename)
        {
            // clear FileNames List before processing directory into it
            if (FileNames != null && !FileNames.Any())
            {
                FileNames.Clear();
            }

            // fill up file names from onlly currect directory
            ProcessDirectory(AddFileNameToList, p_directory, false);

            // check FileNames for p_filename
            int postfix = 1;
            string name = p_filename;
            while (FileNames.IndexOf(name) != -1)
            {
                postfix++;
                name = p_filename + " (" + postfix + ")";
            }
            return name;
        }

        public string ReadTextFile(string p_path)
        {
            string text = "";



            return text;
        }

        public void CreateNewFolder(string p_directory, string p_folderName)
        {
            string pathString = Path.Combine(p_directory, p_folderName);
            Directory.CreateDirectory(pathString);
        }
        public string CreateAndGetNewFolder(string p_directory, string p_folderName)
        {
            string pathString = Path.Combine(p_directory, p_folderName);
            Directory.CreateDirectory(pathString);
            return pathString;
        }
    }
}
