using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search16s
{
	// Derived class, Level5 inherits the members of the base class Level
	class Level5 : Level
	{
		public string PartialSequence { get; set; }
		public Level5(string filename, string partialSequence) : base(filename) // filename is from base class
		{
			this.PartialSequence = partialSequence; // partialSequence is in Level5 class
		}

		// Override the abstract method of base class
		// for searching the sequence-ids by the partial sequence
		public override void LevelSearch()
		{
			bool IsSeqFound = false;
			const char DELIM = ' ';
			// Read the file
			FileStream inFile = new FileStream(this.Filename, FileMode.Open, FileAccess.Read); // The path is the fasta file itself
			StreamReader reader = new StreamReader(inFile);
			string recordIn;
			string[] fields;
			recordIn = reader.ReadLine(); // Start reading the file
			while (recordIn != null)
			{
				fields = recordIn.Split(DELIM);
				string recordNext = reader.ReadLine();
				if (recordNext.Contains(this.PartialSequence)) // Check if the metadata lines contain the given partial sequence
				{
					// Some DNA sequences have more than one sequence ID
					// Needed to show all matched IDs
					for (int i = 0; i < fields.Length; i++)
					{
						// To find the fields with ">" in these fields of each metadata line
						if (fields[i].Substring(0, 1) == ">")
						{
							Console.WriteLine(fields[i].TrimStart('>')); // Delete the '>' and write the IDs on the console
						}
					}
					IsSeqFound = true;
				}
				recordIn = reader.ReadLine(); // Read the next line
			}
			reader.Close();
			inFile.Close();
			if (!IsSeqFound) // If the sequence is not found
			{
				Console.WriteLine("Error, sequence {0} not found.", this.PartialSequence);
			}
		}
	}
}
