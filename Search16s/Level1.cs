using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search16s
{
	// Derived class, Level1 inherits the members of the base class
	class Level1 : Level
	{
		public int Row { get; set; }
		public int SeqNum { get; set; }
		public Level1(string filename, int row, int seqNum) : base(filename) // filename is from base class
		{
			this.Row = row;
			this.SeqNum = seqNum; // row and seqNum are in Level1 class
		}

		// Override the abstract method of base class for searching the sequences with the line number and sequence number
		public override void LevelSearch()
		{
			int lines = ReadFileLines(this.Filename); // Get the number of lines of the input file

			if (this.Row <= lines) // Check if the entered line number exceeds maximum number
			{
				if (this.Row % 2 == 1) // Check if the entered line number is an odd number
				{
					Level1Search();
					if (this.Row + this.SeqNum * 2 - 1 > lines) // Check if the sequence of the query exceeds the maximum value
					{
						Console.WriteLine("The remaining number of lines exceeds the maximum number.");
					}
				}
				else
				{
					Console.WriteLine("The line number must be an odd number.");
				}
			}
			else
			{
				Console.WriteLine("The entered line number exceeds the maximum number.");
			}
		}

		// Level1Search is for searching the sequences with the line number and sequence number
		// Write another method to avoid the LevelSearch method being too long
		private void Level1Search()
		{
			int counter = 1; // Line number
			const char DELIM = ' ';
			// Read the file
			FileStream inFile = new FileStream(this.Filename, FileMode.Open, FileAccess.Read); // The path is file itself
			StreamReader reader = new StreamReader(inFile);
			string recordIn;
			string[] fields;
			recordIn = reader.ReadLine(); // Priming read the file
			while (recordIn != null)
			{
				fields = recordIn.Split(DELIM);
				if (fields[0].Substring(0, 1) == ">") // The metadata line starts with a '>' character
				{
					// Write the lines between "row" and "row + seq_num * 2 - 1"
					if (counter >= this.Row && counter <= this.Row + this.SeqNum * 2 - 1)
					{
						Console.WriteLine(recordIn); // Write the metadata line on the console
						Console.WriteLine(reader.ReadLine()); // Write the sequence on the console
						counter++;
					}
				}
				recordIn = reader.ReadLine(); // Read the next line
				counter++;
			}
			reader.Close();
			inFile.Close();
		}
	}
}
