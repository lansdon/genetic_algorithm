﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Diagnostics;


namespace ga
{
    class Program
    {
		// ********************** Settings ***********************
		public static uint MAX_POPULATION { set; get; }	// Max population    > 0
		public static double MUTATION_RATE { set; get; }   // Percent of coefficients that undergo mutation  [0.0, 1.0]
		public static double MUTATION_RANGE { set; get; }	// Maximum amount a coefficient can be mutated based on original max potential values (a-e = [0,10], f-i = [0,1.0])
		public static uint GENERATION_CAP { set; get; }		// Number of generations to spawn
		public static uint CHILD_COUNT { set; get; }		// Used for ID's of eqaation objects
		public static double BREEDER_PERCENT { set; get; }		// Percent of population used and retained as breeders

        static void Main(string[] args)
        {			
			// ********************** Settings ***********************
			MAX_POPULATION = 25;		// Max population    > 0
			MUTATION_RATE = 0.01;   // Percent of coefficients that undergo mutation  [0.0, 1.0]
			MUTATION_RANGE = 0.0025;	// Maximum amount a coefficient can be mutated based on original max potential values (a-e = [0,10], f-i = [0,1.0])
			GENERATION_CAP = 100;		// Number of generations to spawn
			BREEDER_PERCENT = 0.15;
			
			// ******************** Vars ***************************
            List<Coord_Pair> _knownData;              // Stored list of X, F(X) pairs from file
            List<Equation_Parameters> population = new List<Equation_Parameters>((int)MAX_POPULATION);		// Total Population of coefficient objects
			Genetics ga;								// GENETIC ALGORITHM CLASS
			Statistics stats = new Statistics();		// Statistics Tracking
			Random rand = new Random();					// Store the number generator


            // Load CSV File
            CsvReader csv = new CsvReader(new StreamReader("ConcErlangData.csv"));
			_knownData = new List<Coord_Pair>(csv.GetRecords<Coord_Pair>());

			// Program Loop
			do
			{
				population.Clear();
				stats = new Statistics();
				ga = new Genetics(ref _knownData, ref rand);
	
				// Initial population
				for (uint count = 0; count < 1000; ++count)
				{
					population.Add(new Equation_Parameters(ref _knownData, ref rand));
				}
				stats.doUpdateForGeneration(ref population);


				// Genetic Algorithm
				for (int gen = 0; gen < GENERATION_CAP; ++gen)
				{
					//				System.Console.WriteLine("Pop Size={0}", population.Count());
					ga.nextGeneration(ref population);
					stats.doUpdateForGeneration(ref population);
				}
				stats.print();
				stats.save(_knownData);

				System.Console.WriteLine("Run again? (y/n)");
			} while (Console.ReadKey().KeyChar == 'y');

        }

		static public void printPopulationList(string title, ref List<Equation_Parameters> list)
		{
			System.Console.WriteLine("***** {0} *****", title);
			foreach (Equation_Parameters eq in list)
			{
				System.Console.WriteLine("LMSE={0}", eq.lmse);
			}
		}



    } // END PROGRAM CLASS

}
