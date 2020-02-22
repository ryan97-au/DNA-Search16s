using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search16s
{
	// Define a base class, also is an abstract class
	public abstract class Level
	{
		public string Filename { get; set; }
		public Level(string filename)
		{
			this.Filename = filename;
		}

		// An abstract method for different requirements of search in different levels (different derived classes)
		public abstract void LevelSearch();

		// Read the number of lines in the file
		public static int ReadFileLines(string path)
		{
			int line_count = 0; // The number of lines
			FileStream inFile = new FileStream(path, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(inFile);
			while (reader.ReadLine() != null)
			{
				line_count++; // Get the number of lines
			}
			reader.Close();
			inFile.Close();
			return line_count;
		}
	}
}
