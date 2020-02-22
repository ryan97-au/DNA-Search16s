using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search16s
{
	// Derived class, Level3 inherits the members of the base class
	class Level3 : Level
	{
		public string QueryName { get; set; }
		public string ResultName { get; set; }
		public Level3(string filename, string queryName, string resultName) : base(filename) // filename is from base class
		{
			this.QueryName = queryName;
			this.ResultName = resultName; // queryName and resultName are in Level3 class
		}

		// Read the IDs in the query file and store the IDs into an array
		public string[] QueryFileArray()
		{
			int lines = ReadFileLines(this.QueryName); // Get the number of lines of the query file
			string[] IDsArray = new string[lines]; // The array is for storing the IDs from the query file
			int counter = 0;
			// Read the query file
			FileStream inFile = new FileStream(this.QueryName, FileMode.Open, FileAccess.Read); // The path is the query file itself
			StreamReader reader = new StreamReader(inFile);
			string recordIn; // The record of query file
			string[] fields; // The fields of query file
			recordIn = reader.ReadLine(); // Start reading the query file
			while (recordIn != null)
			{
				fields = recordIn.Split();
				// The first field of each line in the query file is the sequence ID. Store the ID to the array
				IDsArray[counter] = fields[0];
				recordIn = reader.ReadLine(); // Read the next line
				counter++;
			}
			reader.Close();
			inFile.Close();
			return IDsArray;
		}

		// Count the duplicate ids in the IDs array
		public int DuplicateIDCount(string[] IdArray, string id)
		{
			// Set the initial number of the duplicate ID
			int count = 0;
			// Count the duplicate IDs
			var queryIDs = IdArray.GroupBy(t => t.Trim()).Select(t => new { count = t.Count(), key = t.Key }).ToArray();
			foreach (var q in queryIDs)
				if (q.key == id)
					count = q.count; // Get the number of the duplicate ID
			return count;
		}

		// Override the abstract method of base class
		// for searching the sequences by the sequence ids which are in the query file
		// and writing the found sequences to the result file
		public override void LevelSearch()
		{
			string[] query_id = QueryFileArray(); // Get the IDs array from the above method
			const char DELIM = ' ';
			// Write into the result file
			FileStream outFile = new FileStream(this.ResultName, FileMode.Create, FileAccess.Write); // The path is the result file itself
			StreamWriter writer = new StreamWriter(outFile);
			// Read the DNA sequence file
			FileStream fs = new FileStream(this.Filename, FileMode.Open, FileAccess.Read); // The path is the file itself
			StreamReader file = new StreamReader(fs);
			string recordIn;
			string[] fields;
			while ((recordIn = file.ReadLine()) != null)
			{
				string recordNext = file.ReadLine(); // The line after the recordIn
				fields = recordIn.Split(DELIM);
				// Some DNA sequences have more than one sequence ID
				// Compare each field of each metadata line
				for (int i = 0; i < fields.Length; i++)
				{
					// To find the fields with ">" in these fields of each metadata line
					if (fields[i].Substring(0, 1) == ">")
					{
						for (int j = 0; j < query_id.Length; j++)
						{
							// Remove the ">" of each field and compare them with all IDs in query_id array
							if (fields[i].TrimStart('>') == query_id[j])
							{
								int id_count = DuplicateIDCount(query_id, query_id[j]);
								for (int x = 0; x < id_count; x++)
								{
									writer.WriteLine(recordIn); // Write the metadata line into file
									writer.WriteLine(recordNext); // Write the DNA sequence into file
								}
								string RemoveID = query_id[j];
								query_id = query_id.Where(val => val != RemoveID).ToArray(); // Delete the ID from the ID array
							}
						}
					}
				}
			}
			file.Close();
			fs.Close();
			writer.Close();
			outFile.Close();
			for (int i = 0; i < query_id.Length; i++)
			{
				// Write the not found sequence IDs in the console
				Console.WriteLine("Error, sequence {0} not found.", query_id[i]);
			}
		}
	}
}
