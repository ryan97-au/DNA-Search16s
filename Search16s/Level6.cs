using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search16s
{
	// Derived class, Level6 inherits the members of the base class Level
	class Level6 : Level
	{
		public string GivenWord { get; set; }
		public Level6(string filename, string givenWord) : base(filename) // filename is from base class
		{
			this.GivenWord = givenWord; // givenWord is in Level6 class
		}

		// Override the abstract method of base class
		// for searching the sequence-ids by the given word
		public override void LevelSearch()
		{
			bool IsGivenWordFound = false;
			const char DELIM = ' ';
			// Read the file
			FileStream inFile = new FileStream(this.Filename, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(inFile);
			string recordIn;
			string[] fields;
			string[] long_fields; // The fields which contain all info of each ID
			recordIn = reader.ReadLine();
			while (recordIn != null)
			{
				long_fields = recordIn.Split('>'); // Long_fields array is derived from using ‘>’ to split recordIn
				if (recordIn.Contains(this.GivenWord)) // Check if the metadata lines contain the given word
				{
					for (int i = 1; i < long_fields.Length; i++) // The first element of the long_fields array is none, so start from 1
					{
						if (long_fields[i].Contains(this.GivenWord)) // Check if each long_field contains the givenword
						{
							fields = long_fields[i].Split(DELIM); // Split each long_field which contains the givenword
							Console.WriteLine(fields[0]); // Delete the '>' and write the ids on the console
						}
						else
						{
							Console.WriteLine(this.GivenWord.TrimStart('>'));
							break;
						}
					}
					IsGivenWordFound = true; // Givenword is found
				}
				recordIn = reader.ReadLine();
			}
			reader.Close();
			inFile.Close();
			if (!IsGivenWordFound) // If the given word is not found
			{
				Console.WriteLine("Error, the given word {0} not found.", this.GivenWord);
			}
		}
	}
}
