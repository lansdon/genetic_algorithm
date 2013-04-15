using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ga
{
	class Statistics
	{
		public static string FILE_BEST_RECORD = "_best_record.txt";
		public static string FILE_BEST_SETTINGS = "_settings.txt";
		public static string FILE_BEST_GEN_AVGS = "_generation_avgs.txt";
		public static string FILE_BEST_GEN_BEST = "_generation_best.txt";

//		public static double BEST_LMSE { set; get; }				// Best LMSE Out of any generation
		public static double mutations { set; get; }				// Total Mutations
		private List<double> _generation_avgs = new List<double>();	// list of generation avgs
		private List<double> _generation_best = new List<double>();	// list of generation best
		public Equation_Parameters best_record = new Equation_Parameters();						// best record w/ id and lmse and coefficients
		private Stopwatch stopWatch = new Stopwatch();

		public Statistics()
		{
//			BEST_LMSE = 666;
			mutations = 0;

			stopWatch.Start();
		}


		// Keep a record of the best LMSE as well as an array of the best per generation
		public void doUpdateForGeneration(ref List<Equation_Parameters> population)
		{
			population.Sort();
			Equation_Parameters generation_best = population[0];
			double generation_avg_lmse = 0;
			for (int count = 0; count < population.Count(); ++count)
			{
				generation_avg_lmse += population[count].lmse;
			}
			generation_avg_lmse /= population.Count();

			if (generation_best.lmse < best_record.lmse) { best_record = generation_best; }		// Update best_lmse
			_generation_avgs.Add(generation_avg_lmse);
			_generation_best.Add(generation_best.lmse);
		}


		public void print()
		{
			System.Console.WriteLine("\n***************** Genetic Algorithm Results ******************", null);
			System.Console.WriteLine("Population Cap={0}", Program.MAX_POPULATION);
			System.Console.WriteLine("Generation Cap={0}", Program.GENERATION_CAP);
			System.Console.WriteLine("Mutation Rate={0}", Program.MUTATION_RATE);
			System.Console.WriteLine("Mutation Range= +/-{0}", (Program.MUTATION_RANGE/2));
			System.Console.WriteLine("Breeder Percent= +/-{0}", Program.BREEDER_PERCENT);
			System.Console.WriteLine("Total children={0}", Program.CHILD_COUNT);
			System.Console.WriteLine("Recorded Generations={0}", _generation_best.Count());
			System.Console.WriteLine("Total Mutations={0}", mutations);
			System.Console.WriteLine("*************************** Top Record **************************", Program.CHILD_COUNT);
			best_record.print();

			stopWatch.Stop();
			// Get the elapsed time as a TimeSpan value.
			TimeSpan ts = stopWatch.Elapsed;

			// Format and display the TimeSpan value. 
//			string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
//				ts.Hours, ts.Minutes, ts.Seconds,
//				ts.Milliseconds / 10);
//			Console.WriteLine("Elapsed Time: " + elapsedTime);
			Console.WriteLine("Elapsed Time: " + ts.Seconds);

//			int best_gen = -1;
//			for (int count = 0; count < _generation_best.Count(); ++count)
//			{
//				System.Console.WriteLine("Generation {0} Best = {1}", count, _generation_best[count]);
//				System.Console.WriteLine("Generation {0} Avg = {1}", count, _generation_avgs[count]);
//				if (_generation_best[count] == BEST_LMSE) { best_gen = count; }
//			}
//			System.Console.WriteLine("Best Generation = {0}",best_gen);

		}



		/*
		 * Save statistics data to files
		 * 
		 * Some files are onliy updated if the new record outperformed the saved best
		 *  - "_best_record.txt"
		 *  - "_best_settings.txt"
		 *  - "_
		 * 
		 */

		public void save(List<Coord_Pair> knownData)
		{
			// Top Record
			double saved_best_lsme = 100.0;
			if(System.IO.File.Exists(FILE_BEST_RECORD))
			{
				using (System.IO.StreamReader file = new System.IO.StreamReader(FILE_BEST_RECORD))
				{
					saved_best_lsme = Convert.ToDouble(file.ReadLine());
				}
			}

			// Only update saved files when we have a improved result
			if (best_record.lmse < saved_best_lsme)
			{

				System.Console.WriteLine("Updating save files...");

				// BEST RECORD
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(FILE_BEST_RECORD))
				{
					file.WriteLine(best_record.lmse);
					file.WriteLine(best_record.id);
					file.WriteLine(best_record.a);
					file.WriteLine(best_record.b);
					file.WriteLine(best_record.c);
					file.WriteLine(best_record.d);
					file.WriteLine(best_record.e);
					file.WriteLine(best_record.f);
					file.WriteLine(best_record.g);
					file.WriteLine(best_record.h);
					file.WriteLine(best_record.i);
					file.WriteLine(best_record.j);
					file.WriteLine();
				}
				best_record.calculate_lmse(ref knownData, true);


				// BEST Settings
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(FILE_BEST_SETTINGS))
				{
					file.WriteLine("\n******** Genetic Algorithm Results *********");
					file.WriteLine("Population Cap={0}", Program.MAX_POPULATION);
					file.WriteLine("Generation Cap={0}", Program.GENERATION_CAP);
					file.WriteLine("Mutation Rate={0}", Program.MUTATION_RATE);
					file.WriteLine("Mutation Range= +/-{0}", (Program.MUTATION_RANGE / 2));
					file.WriteLine("Breeder Percent= +/-{0}", Program.BREEDER_PERCENT);
					file.WriteLine("Total children={0}", Program.CHILD_COUNT);
					file.WriteLine("Recorded Generations={0}", _generation_best.Count());
					file.WriteLine("Total Mutations={0}", mutations);
				}

				// Generation Averages
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(FILE_BEST_GEN_AVGS))
				{
					_generation_avgs.ForEach(file.WriteLine);
				}


				// Generation Best
				using (System.IO.StreamWriter file = new System.IO.StreamWriter(FILE_BEST_GEN_BEST))
				{
					_generation_best.ForEach(file.WriteLine);
				}
				



			}
			


		}
	}
}
