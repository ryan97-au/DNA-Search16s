using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search16s
{
	// The Statement class is for calling the search method of each level and showing the error message
	class Statement
	{
		// Search the sequences in different ways and showing the error message
		public static void ExecuteStatement(string[] args)
		{
			InputArgs thisArgs = new InputArgs();
			thisArgs.level = args[0];
			// Check if the entered level is valid or not
			if (thisArgs.level == "-level1" || thisArgs.level == "-level2" || thisArgs.level == "-level3" || 
				thisArgs.level == "-level4" || thisArgs.level == "-level5" || thisArgs.level == "-level6" || thisArgs.level == "-level7")
			{
				thisArgs.filename = args[1];
				if (File.Exists(thisArgs.filename)) // Check if the file exists or not
				{
					string caseSwitch = thisArgs.level;
					switch (caseSwitch)
					{
						case "-level1":
							thisArgs.row = int.Parse(args[2]);
							thisArgs.seqNum = int.Parse(args[3]);
							if (args.Length == 4) // Check if the number of entered arguments is 4
							{
								if (thisArgs.row > 0 && thisArgs.seqNum > 0) // Check if the line number and sequence number are positive number
								{
									Level level = new Level1(thisArgs.filename, thisArgs.row, thisArgs.seqNum);
									level.LevelSearch();
								}
								else
								{
									Console.WriteLine("The line number and sequence number must be positive number.");
								}
							}
							else IncorrectNumberArguments();
							break;
						case "-level2":
							thisArgs.seqId = args[2];
							if (args.Length == 3) // Check if the number of entered arguments is 3
							{
								Level level = new Level2(thisArgs.filename, thisArgs.seqId);
								level.LevelSearch();
							}
							else IncorrectNumberArguments();
							break;
						case "-level3":
							thisArgs.queryName = args[2];
							if (File.Exists(thisArgs.queryName)) // Check if the query file exists
							{
								thisArgs.resultName = args[3];
								if (args.Length == 4) // Check if the number of entered arguments is 4
								{
									Level level = new Level3(thisArgs.filename, thisArgs.queryName, thisArgs.resultName);
									level.LevelSearch();
								}
								else IncorrectNumberArguments();
							}
							else if (IsFileFormatInCorrect(thisArgs.queryName, 5, "txt")) // Check if the format of entered query file is incorrect
							{
								Console.WriteLine("The query file is incorrectly formatted.");
							}
							else
							{
								Console.WriteLine("The query file does not exist.");
							}
							break;
						case "-level4":
							thisArgs.indexfilename = args[2];
							if (File.Exists("../../../IndexSequence16s/bin/Debug/" + thisArgs.indexfilename)) // Check if the index file exists
							{
								thisArgs.queryName = args[3];
								if (File.Exists(thisArgs.queryName)) // Check if the query file exists
								{
									thisArgs.resultName = args[4];
									if (args.Length == 5) // Check if the number of entered arguments is 5
									{
										Level level = new Level4(thisArgs.filename, thisArgs.queryName, thisArgs.resultName, thisArgs.indexfilename);
										level.LevelSearch();
									}
									else IncorrectNumberArguments();
								}
								else if (IsFileFormatInCorrect(thisArgs.queryName, 5, "txt")) // Check if the format of entered query file is incorrect
								{
									Console.WriteLine("The query file is incorrectly formatted.");
								}
								else
								{
									Console.WriteLine("The query file does not exist.");
								}
							}
							else if (File.Exists("../../../IndexSequence16s/bin/Debug/" + thisArgs.indexfilename + ".index") ||
									(thisArgs.indexfilename.Length > 3 && File.Exists("../../../IndexSequence16s/bin/Debug/" + thisArgs.indexfilename.Substring(0, 4) + "index")))
							// Check if the format of entered index file is incorrect
							{
								Console.WriteLine("The index file is incorrectly formatted.");
							}
							else
							{
								Console.WriteLine("The index file does not exist.");
							}
							break;
						case "-level5":
							thisArgs.partialSeq = args[2];
							if (args.Length == 3) // Check if the number of entered arguments is 3
							{
								Level level = new Level5(thisArgs.filename, thisArgs.partialSeq);
								level.LevelSearch();
							}
							else IncorrectNumberArguments();
							break;
						case "-level6":
							thisArgs.givenWord = args[2];
							if (args.Length == 3) // Check if the number of entered arguments is 3
							{
								Level level = new Level6(thisArgs.filename, thisArgs.givenWord);
								level.LevelSearch();
							}
							else IncorrectNumberArguments();
							break;
						case "-level7":
							thisArgs.wildCardSequence = args[2];
							if (args.Length == 3) // Check if the number of entered arguments is 3
							{
								Level level = new Level7(thisArgs.filename, thisArgs.wildCardSequence);
								level.LevelSearch();
							}
							else IncorrectNumberArguments();
							break;
						default: // Do nothing
							break;
					}
				}
				else if (IsFileFormatInCorrect(thisArgs.filename, 3, "fasta")) // Check if the format of file is incorrect or not
				{
					Console.WriteLine("The file is incorrectly formatted.");
				}
				else
				{
					Console.WriteLine("The file does not exist.");
				}
			}
			else
				Console.WriteLine("Please enter a valid level." + InputInCorrectFormat.Format);
		}

		// Check if the file format is incorrect
		private static bool IsFileFormatInCorrect(string fileName, int fileLength, string fileExtension)
		{
			// filename + ".fileExtension" -----------> (e.g. 16s + ".fasta")
			// filename.Substring + "fileExtension" --> (e.g. "16s.txt".Substring + "fasta")
			if (File.Exists(fileName + "." + fileExtension) || (fileName.Length > fileLength && File.Exists(fileName.Substring(0, fileLength + 1) + fileExtension)))
			{
				return true; // If the format is incorrect, return true
			}
			else
				return false;
		}

		// Show the message when the input number of arguments is incorrect
		private static void IncorrectNumberArguments()
		{
			Console.WriteLine("Please enter the correct number of arguments." + InputInCorrectFormat.Format);
		}
	}

	// The format which will be shown after error message
	public class InputInCorrectFormat
	{
		public static string Format = "\nThe valid formats are:" +
									  "\n-level1 filename.fasta line_num sequence_num" +
									  "\n-level1 filename.fasta line_num sequence_num > outputfile.fasta" +
									  "\n-level2 filename.fasta sequence_id" +
									  "\n-level3 filename.fasta queryfile.txt resultfile.txt" +
									  "\n-level4 filename.fasta filename.index queryfile.txt resultfile.txt" +
									  "\n-level5 filename.fasta partial_sequence" +
									  "\n-level6 filename.fasta given_word" +
									  "\n-level7 filename.fasta wildCard_sequence";
	}

	public class InputArgs
	{
		public string level;
		public string filename;
		public int row;
		public int seqNum;
		public string seqId;
		public string queryName;
		public string resultName;
		public string indexfilename;
		public string partialSeq;
		public string givenWord;
		public string wildCardSequence;
	}
}
