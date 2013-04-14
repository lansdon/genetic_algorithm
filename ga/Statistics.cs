using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ga
{
	class Statistics
	{
		public static double BEST_LMSE { set; get; }		// Best LMSE Out of any generation
		public static double mutations { set; get; }		// Total Mutations
		private List<double> _generation_avgs = new List<double>();
		private List<double> _generation_best = new List<double>();

		public Statistics()
		{
			BEST_LMSE = 666;
			mutations = 0;
		}


		// Keep a record of the best LMSE as well as an array of the best per generation
		public void doUpdateForGeneration(ref List<Equation_Parameters> population)
		{
			population.Sort();
			double generation_best_lmse = population[0].lmse;
			double generation_avg_lmse = 0;
			for (int count = 0; count < population.Count(); ++count)
			{
				generation_avg_lmse += population[count].lmse;
			}
			generation_avg_lmse /= population.Count();

			if (generation_best_lmse < BEST_LMSE) { BEST_LMSE = generation_best_lmse; }		// Update best_lmse
			_generation_avgs.Add(generation_avg_lmse);
			_generation_best.Add(generation_best_lmse);
		}


		public void print()
		{
			System.Console.WriteLine("\n***************** Genetic Algorithm Results ******************", null);
			System.Console.WriteLine("Population Cap={0}", Program.MAX_POPULATION);
			System.Console.WriteLine("Generation Cap={0}", Program.GENERATION_CAP);
			System.Console.WriteLine("Mutation Rate={0}", Program.MUTATION_RATE);
			System.Console.WriteLine("Mutation Range= +/-{0}", (Program.MUTATION_RANGE/2));
			System.Console.WriteLine("Total children={0}", Program.CHILD_COUNT);
			System.Console.WriteLine("Recorded Generations={0}", _generation_best.Count());
			System.Console.WriteLine("*************************** Rseults **************************", Program.CHILD_COUNT);
			System.Console.WriteLine("BEST LMSE = {0}", BEST_LMSE);
			System.Console.WriteLine("Total Mutations={0}", mutations);

//			int best_gen = -1;
//			for (int count = 0; count < _generation_best.Count(); ++count)
//			{
//				System.Console.WriteLine("Generation {0} Best = {1}", count, _generation_best[count]);
//				System.Console.WriteLine("Generation {0} Avg = {1}", count, _generation_avgs[count]);
//				if (_generation_best[count] == BEST_LMSE) { best_gen = count; }
//			}
//			System.Console.WriteLine("Best Generation = {0}",best_gen);

		}


	}
}
