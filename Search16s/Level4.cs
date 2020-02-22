using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search16s
{
	// Derived class, Level4 inherits the members of the base class Level and Level3 class
	class Level4 : Level3
	{
		public string Indexfilename { get; set; }
		// filename is from base class, queryName and resultName are from Level3 class
		public Level4(string filename, string queryName, string resultName, string indexFilename) : base(filename, queryName, resultName)
		{
			this.Indexfilename = indexFilename; // indexFilename is in Level4 class
		}

		// Override the abstract method of base class
		// for searching the sequences by the indexfile, which is generated from the IndexSequence16s project
		public override void LevelSearch()
		{
			string[] query_id = QueryFileArray(); // Get the IDs array from the base class - Level3 class method
			string line; // Line of text
			int counter = 0; // Line counter in text file
			int position = 0; // File position of first line
			List<int> pos = new List<int>(); // An array to keep positions in
			List<int> size = new List<int>(); // An array to keep size in
			string indexfilepath = "../../../IndexSequence16s/bin/Debug/" + this.Indexfilename; // The path of indexfile which is from the other project
			int CurrentIDIndex = 0; // The index of the current line, which contains the searched ID
			int NextIDIndex = 0; // The index of the next line
			int pos_num; // The pos of next id

			// Read the index file
			StreamReader reader = new StreamReader(indexfilepath);
			while ((line = reader.ReadLine()) != null)
            {
                pos.Insert(counter,position); // Store line position
                size.Insert(counter, line.Length + 2); // Store line size
                counter++;
                position = position + line.Length + 2; // Add 2 for '\r\n' character in file
            }
			reader.Close();

			// Write into the result file
			FileStream outFile = new FileStream(this.ResultName, FileMode.Create, FileAccess.Write); // The path is the result file itself
			StreamWriter writer = new StreamWriter(outFile);
			// Use the parallel arrays index to access a line directly 
			using (FileStream inFile = new FileStream(indexfilepath, FileMode.Open, FileAccess.Read))
			{
				for (int n = 0; n < counter; n++)
				{
					byte[] bytess = new byte[size[n]];
					inFile.Seek(pos[n], SeekOrigin.Begin); // Seek to line n
					inFile.Read(bytess, 0, size[n]); // Get the data off disk

					for (int i = 0; i < query_id.Length; i++)
					{
						if (Encoding.Default.GetString(bytess).Contains(query_id[i])) // Check if the current line contains the queried ids
						{
							CurrentIDIndex = int.Parse(Encoding.Default.GetString(bytess).Substring(12)); // Get the index of the queried ids
							pos_num = n + 1;
							for (int a = 0; a < 999; a++)
							{
								byte[] bytesss = new byte[size[pos_num]];
								inFile.Seek(pos[pos_num], SeekOrigin.Begin); // Seek to line pos_num which might be n + 1, + 2 or n + 3 ...
								inFile.Read(bytesss, 0, size[pos_num]);
								NextIDIndex = int.Parse(Encoding.Default.GetString(bytesss).Substring(12));
								// Due to one line might have more than one IDs and they have the same line index
								// So, needed to check if the NextIndex is equal to the currentIndex
								// If the NextIndex is equal to the currentIndex, return 0 and keep doing the loop until find next different one
								if (NextIDIndex == CurrentIDIndex)
								{
									pos_num++;
									a = 0;
								}
								else
									a = 999; // If find the next different line index, out of the loop
							}
							// Read the file and find the content of specific location
							using (FileStream fs = new FileStream(this.Filename, FileMode.Open, FileAccess.Read))
							{
								byte[] bytes = new byte[NextIDIndex - CurrentIDIndex]; // The length of bytes array 
								fs.Seek(CurrentIDIndex, SeekOrigin.Begin);
								fs.Read(bytes, 0, bytes.Length); // Only read the specific length
								int id_count = DuplicateIDCount(query_id, query_id[i]); // Check the number of the current queried id
								for (int j = 0; j < id_count; j++)
								{
									writer.Write(Encoding.Default.GetString(bytes)); // Write in to the result file
								}
							}
							string RemoveID = query_id[i];
							query_id = query_id.Where(val => val != RemoveID).ToArray(); // Delete the ID from the ID array
						}
					}
				}
			}
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
