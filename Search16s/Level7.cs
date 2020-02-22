using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Search16s
{
	// Derived class, Level7 inherits the members of the base class Level
	class Level7 : Level
	{
		public string WildCardSequence { get; set; }
		public Level7(string filename, string wildCardSequence) : base(filename) // filename is from base class
		{
			this.WildCardSequence = wildCardSequence; // wildCardSequence is in Level7 class
		}

		// Override the abstract method of base class
		// for searching the metadata and matched partial sequences by the wild card sequence
		public override void LevelSearch()
		{
			bool IswildCardSequenceFound = false;
			FileStream inFile = new FileStream(this.Filename, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(inFile);
			string recordIn;
			recordIn = reader.ReadLine();
			while (recordIn != null)
			{
				string recordNext = reader.ReadLine();
				string[] strArr = this.WildCardSequence.Split('*'); // Split the given wild card sequence by '*'
				if (strArr.Length > 1) // The wild card sequence contains at least one '*'
				{
					string str = null;
					for (int i = 1; i < strArr.Length; i++)
					{
						str += ".*?(?=" + strArr[i] + ")"; // Use (?<=xxxxxx).*?(?=xxxxxx).*?(?=xxxxxx)......
					}
					MatchCollection matches = Regex.Matches(recordNext, @"(?<=" + strArr[0] + ")" + str);
					foreach (Match match in matches)
					{
						Console.WriteLine(recordIn); // Write the metadata on the console
						Console.WriteLine(strArr[0] + match + strArr[strArr.Length - 1]); // Write all matched partial sequences on the console
						IswildCardSequenceFound = true; // The wild card sequence is found
					}
				}
				else // The wild card sequence does not contain '*'
				{
					if (recordNext.Contains(this.WildCardSequence))
					{
						Console.WriteLine(recordIn);
						Console.WriteLine(this.WildCardSequence);
						IswildCardSequenceFound = true;
					}
				}
				recordIn = reader.ReadLine();
			}
			reader.Close();
			inFile.Close();
			if (!IswildCardSequenceFound) // The wild card sequence is not found
			{
				Console.WriteLine("No matching sequence in the {0} file.", this.Filename);
			}
		}
	}
}
