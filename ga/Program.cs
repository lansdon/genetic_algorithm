using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Diagnostics;
using System.Threading;


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
		public static double BREEDER_RETENTION { set; get; }		// Percent of population used and retained as breeders
		public static int THREAD_MIN { set; get; }

		static public void printPopulationList(string title, ref ThreadSafeAddList<Equation_Parameters> list)
		{
			System.Console.WriteLine("***** {0} *****", title);
			foreach (Equation_Parameters eq in list)
			{
				System.Console.WriteLine("LMSE={0}", eq.lmse);
			}
		}



		static void Main(string[] args)
		{
			// ********************** Settings ***********************
			MAX_POPULATION = 100;		// Max population    > 0
			MUTATION_RATE = 0.2;   // Percent of coefficients that undergo mutation  [0.0, 1.0]
//			MUTATION_RANGE = 1;	// Maximum amount a coefficient can be mutated based on original max potential values (a-e = [0,10], f-i = [0,1.0])
			GENERATION_CAP = 1000;		// Number of generations to spawn
			BREEDER_PERCENT = 0.5;		// Percent of population selected for breeding
			BREEDER_RETENTION = 1;	// Percent of breeders that is retained in the population
			THREAD_MIN = 1;

			// ******************** Vars ***************************
			List<Coord_Pair> _knownData;              // Stored list of X, F(X) pairs from file
			ThreadSafeAddList<Equation_Parameters> population = new ThreadSafeAddList<Equation_Parameters>();		// Total Population of coefficient objects
//			Genetics ga;								// GENETIC ALGORITHM CLASS
			Statistics stats = new Statistics();		// Statistics Tracking
			Random rand = new Random();					// Store the number generator


			// Load CSV File
			CsvReader csv = new CsvReader(new StreamReader("ConcErlangData.csv"));
			_knownData = new List<Coord_Pair>(csv.GetRecords<Coord_Pair>());

			// Program Loop
			do
			{
				stats = new Statistics();		// Statistics Tracking
				population.Clear();

				// Initial population
				for (uint count = 0; count < MAX_POPULATION; ++count)
				{
					population.Add(new Equation_Parameters(ref _knownData, ref rand));
				}

				int breeder_upper_bounds = (int)((double)(MAX_POPULATION) * BREEDER_PERCENT);

				// Genetic Algorithm Loop - Until all generations are done
				for (int gen = 0; gen < GENERATION_CAP; ++gen)
				{
					// THREAD SETTINGS
					int minWorker, minIOC;
					ThreadPool.GetMinThreads(out minWorker, out minIOC);// Get the current settings.
					if (!ThreadPool.SetMinThreads(THREAD_MIN, minIOC))
					{
						System.Console.WriteLine("Error setting min threads");
					}

					population.Sort();
					population.RemoveRange(breeder_upper_bounds, population.Count - breeder_upper_bounds);
					int num_to_spawn = (int)(Program.MAX_POPULATION - (breeder_upper_bounds)) / 2;


					// Concurrent loop - // Repopulate with offspring until population is max again
					Parallel.For(0, num_to_spawn, count =>
					{

							int P1_index = rand.Next(breeder_upper_bounds);		// rand parents from population
							int P2_index = rand.Next(breeder_upper_bounds);

							Equation_Parameters child1 = population[P1_index].mate(population[P2_index], ref rand, ref _knownData);
							Equation_Parameters child2 = population[P2_index].mate(population[P1_index], ref rand, ref _knownData);
							
							population.Add(child1);
							population.Add(child2);

					});
				
					stats.doUpdateForGeneration(population);
				}

				stats.print();
				stats.save(_knownData);
//				printPopulationList("Final Population", ref population);

				System.Console.WriteLine("Run again? (y/n)");
			} while (Console.ReadKey().KeyChar == 'y');
	
		}


    } // END PROGRAM CLASS

}
