using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexSequence16s
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Statement(args);
			}
			catch (IndexOutOfRangeException) // Index out of range
			{
				Console.WriteLine("Please enter enough arguments." + InputInvalidFormat.Format);
			}

			// Suspend the screen.
			Console.ReadLine();
		}

		static void Statement(string[] args)
		{
			InputArgs thisArgs = new InputArgs();
			thisArgs.filename = args[0];
			string filepath = "../../../Search16s/bin/Debug/" + thisArgs.filename;

			if (File.Exists(filepath)) // Check if the file exists or not
			{
				thisArgs.indexfilename = args[1];
				if (args.Length == 2)
				{
					Level4Index level4index = new Level4Index(thisArgs.filename, thisArgs.indexfilename);
					level4index.Level4IndexSequence();
				}
				else
				{
					Console.WriteLine("Please enter the correct number of arguments." + InputInvalidFormat.Format);
				}
			}
			else if (File.Exists(filepath + ".fasta") || (thisArgs.filename.Length > 3 && File.Exists("../../../Search16s/bin/Debug/" + thisArgs.filename.Substring(0, 4) + "fasta"))) // Check if the format of entered fasta file is incorrect
			{
				Console.WriteLine("The fasta file is incorrectly formatted.");
			}
			else
			{
				Console.WriteLine("The file does not exist.");
			}
		}
	}

	// The format which will be shown after error message
	public class InputInvalidFormat
	{
		public static string Format = "\nThe valid formats are:\nfilename indexfilename\nSuch as, 16S.fasta 16S.index";
	}

	public class InputArgs
	{
		public string filename;
		public string indexfilename;
	}
}
