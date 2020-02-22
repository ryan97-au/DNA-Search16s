using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search16s
{
	// Derived class, Level2 inherits the members of the base class
	class Level2 : Level
	{
		public string SeqId { get; set; }
		public Level2(string filename, string seqId) : base(filename) // filename is from base class
		{
			this.SeqId = seqId; // seqId is in Level2 class
		}

		// Override the abstract method of base class for searching the sequences by the sequence id
		public override void LevelSearch()
		{
			bool IsIDFound = false; // bool is for marking whether the ID is found
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
				// Some DNA sequences have more than one sequence ID
				// Compare each field of each metadata line
				for (int i = 0; i < fields.Length; i++)
				{
					// To find the fields with ">" in these fields of each metadata line
					if (fields[i].Substring(0, 1) == ">")
					{
						// Remove the ">" of each field and compare them with the given sequence ID
						if (fields[i].TrimStart('>') == this.SeqId)
						{
							Console.WriteLine(recordIn); // Write the metadata line on the console
							Console.WriteLine(reader.ReadLine()); // Write the sequence on the console
							IsIDFound = true; // The entered ID is found
						}
					}
				}
				recordIn = reader.ReadLine(); // Read the next line
			}
			reader.Close();
			inFile.Close();
			if (!IsIDFound) // If the entered ID is not found
			{
				Console.WriteLine("Error, sequence {0} not found.", this.SeqId);
			}
		}
	}
}
