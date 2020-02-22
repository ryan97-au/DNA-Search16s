using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search16s
{
    class Program
    {
		static void Main(string[] args)
        {
			try
			{
				// Invoke the ExecuteStatement method from Statement class
				Statement.ExecuteStatement(args);
			}
			catch (IndexOutOfRangeException) // Index out of range
			{
				Console.WriteLine("Please enter enough arguments." + InputInCorrectFormat.Format);
			}
			catch (FormatException) // The format of entered arguments is incorrect
			{
				Console.WriteLine("Please enter the valid arguments." + InputInCorrectFormat.Format);
			}

            // Suspend the screen.
            Console.ReadLine();
        }
    }
}
