using System;
using System.Collections.Generic;
using System.IO;

namespace comp110_worksheet_7
{
	public static class DirectoryUtils
	{
		// Return the size, in bytes, of the given file
		public static long GetFileSize(string filePath)
		{
			return new FileInfo(filePath).Length;
		}

		// Return true if the given path points to a directory, false if it points to a file
		public static bool IsDirectory(string path)
		{
			return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
		}

		// Return the total size, in bytes, of all the files below the given directory
		public static long GetTotalSize(string directory)
		{
            //Recursively get all directories via SearchOption.AllDirectories
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            long size = 0;
            foreach(string name in files)
            {
                FileInfo info = new FileInfo(name);
                //Add their length to size
                size += info.Length;
            }
            //Return size
            return size;
        }

		// Return the number of files (not counting directories) below the given directory
		public static int CountFiles(string directory)
		{
            //Return the length of the array returned by GetFiles
            return Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).Length;
        }

        // Return the nesting depth of the given directory. A directory containing only files (no subdirectories) has a depth of 0.
        public static int GetDepth(string directory)
		{
            //Return 0 if directory is null
            if (string.IsNullOrEmpty(directory))
                return 0;
            //Return 1 if there's no parent
            DirectoryInfo parent = Directory.GetParent(directory);
            if (parent == null)
                return 1;
            
            return GetDepth(parent.FullName) + 1;
        }

		// Get the path and size (in bytes) of the smallest file below the given directory
		public static Tuple<string, long> GetSmallestFile(string directory)
		{
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            List<Tuple<string, long>> fileSizes = new List<Tuple<string, long>>();
            //Add the name of the files and their size to the list
            foreach (string name in files)
            {
                FileInfo info = new FileInfo(name);
                fileSizes.Add(new Tuple<string, long>("/" + info.Directory.Name + "/" + info.Name, info.Length));
            }
            //Sort the list by the sizes (Item2 of the tuple)
            fileSizes.Sort(
                delegate (Tuple<string,long> a,Tuple<string,long> b)
                {
                    return a.Item2.CompareTo(b.Item2);
                }
            );
            return fileSizes[0]; //Return the first index since it will be the largest after sorting.
        }

        // Get the path and size (in bytes) of the largest file below the given directory
        public static Tuple<string, long> GetLargestFile(string directory)
		{
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            List<Tuple<string, long>> fileSizes = new List<Tuple<string, long>>();
            //Add the name of the files and their size to the list
            foreach (string name in files)
            {
                FileInfo info = new FileInfo(name);
                fileSizes.Add(new Tuple<string, long>("/" + info.Directory.Name + "/" + info.Name, info.Length));
            }
            //Sort the list by the sizes (Item2 of the tuple)
            fileSizes.Sort(
                delegate (Tuple<string, long> a, Tuple<string, long> b)
                {
                    return b.Item2.CompareTo(a.Item2);
                }
            );
            return fileSizes[0]; //Return the first index since it will be the largest after sorting.
        }

        // Get all files whose size is equal to the given value (in bytes) below the given directory
        public static IEnumerable<string> GetFilesOfSize(string directory, long size)
		{
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            List<string> filesOfSize = new List<string>();
            
            foreach (string name in files)
            {
                FileInfo info = new FileInfo(name);
                //Add their length to size
                if(info.Length==size)
                    filesOfSize.Add("/"+info.Directory.Parent.Name+"/"+info.Directory.Name+"/"+info.Name);
            }
            //Return size
            return filesOfSize;
        }
    }
}
