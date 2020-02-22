using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSequence16s
{
	class Level4Index
	{
		public string Filename { get; set; }
		public string IndexFilename { get; set; }
		public Level4Index(string filename, string indexfilename)
		{
			this.Filename = filename;
			this.IndexFilename = indexfilename;
		}

		public void Level4IndexSequence()
		{
			int counter = 0; // line counter in text file
			string line; // line of text
			int position = 0; // file position of first line
			List<int> pos = new List<int>(); // an array to keep ine positions in
			List<int> size = new List<int>(); // an array to keep ine size in
			string filepath = "../../../Search16s/bin/Debug/" + this.Filename;

			// Read the file
			StreamReader file = new StreamReader(filepath);
			while ((line = file.ReadLine()) != null)
			{
				pos.Insert(counter, position); // store line position
				size.Insert(counter, line.Length + 1); // store line size
				counter++;
				position = position + line.Length + 1; // add 1 for '\n' character in file
			}
			file.Close();

			// Write into the index file
			FileStream outFile = new FileStream(this.IndexFilename, FileMode.Create, FileAccess.Write);
			StreamWriter writer = new StreamWriter(outFile);
			// Use the parallel arrays index to access a line directly 
			using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
			{
				string[] fields;
				string metadata;
				const char DELIM = ' ';
				for (int n = 0; n < counter; n++)
				{
					byte[] bytes = new byte[size[n]];
					fs.Seek(pos[n], SeekOrigin.Begin); // Seek to line n
					fs.Read(bytes, 0, size[n]); // Get the data from each line
					metadata = Encoding.Default.GetString(bytes);
					fields = metadata.Split(DELIM);
					for (int i = 0; i < fields.Length; i++) // Some metadata lines have more than one ID, needed to compare all fields
					{
						if (fields[i].Substring(0, 1) == ">") // To find all fields with ">" in these fields of metadata line
						{
							writer.WriteLine("{0} {1}", fields[i].TrimStart('>'), pos[n]);
						}
					}
				}
			}
			writer.Close();
			outFile.Close();
		}
	}
}
