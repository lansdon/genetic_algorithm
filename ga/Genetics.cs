using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ga
{
	class Genetics
	{
		// 1) Sort population
		// 2) Take top half
		// 3) Generate offspring
		// 4) Perform mutations

		private List<Coord_Pair> _knownData;

		public Genetics( ref List<Coord_Pair> knownData)
		{
			_knownData = knownData;
		}

		public void nextGeneration(ref List<Equation_Parameters> population, ref Random rand)
		{
			List<Equation_Parameters> breeders = getBreedersFromPopulation(ref population);
//			Program.printPopulationList("Breeders", ref breeders);
			List<Equation_Parameters> offspring = generateOffspring(ref breeders, ref rand);
//			Program.printPopulationList("Mutated Offspring", ref offspring);
			population = new List<Equation_Parameters>(offspring.Concat<Equation_Parameters>(breeders));
			System.Console.Write(".");
//			Program.printPopulationList("Next Generation", ref population);
		}


		/*
		 *  Determines the top portion of the population and returns a sub-list of breeders
		 */
		private List<Equation_Parameters> getBreedersFromPopulation(ref List<Equation_Parameters> population)
		{
			population.Sort();
//			Program.printPopulationList("Sorted Population", ref population);
			return population.GetRange(0, population.Count()/2);
		}




		/*
		 * Generates offspring from the list of breeders
		 * Returns list of new offspring
		 */
		private List<Equation_Parameters> generateOffspring(ref List<Equation_Parameters> breeders, ref Random rand)
		{
			List<Equation_Parameters> offspring = new List<Equation_Parameters>();

			for (int count = 0, breed_count=0; count < Program.MAX_POPULATION - breeders.Count(); ++count)
			{
				// Cycle through breeders sequentially selecting mating pairs.
				if (breed_count >= breeders.Count()) { breed_count = 0; }	// start cycling through breeders from the beginning
				Equation_Parameters P1 = breeders[breed_count++];			// Parent 1
				if (breed_count >= breeders.Count()) { breed_count = 0; }	// start cycling through breeders from the beginning
				Equation_Parameters P2 = breeders[breed_count++];			// Parent 2

				// Meiosis
				Equation_Parameters child = new Equation_Parameters(ref _knownData, ref rand);
				child.a = (rand.NextDouble() < 0.5) ? P1.a : P2.a;			// 50% chance to take coeff from P1
				child.b = (rand.NextDouble() < 0.5) ? P1.b : P2.b;			// 50% chance to take coeff from P1
				child.c = (rand.NextDouble() < 0.5) ? P1.c : P2.c;			// 50% chance to take coeff from P1
				child.d = (rand.NextDouble() < 0.5) ? P1.d : P2.d;			// 50% chance to take coeff from P1
				child.e = (rand.NextDouble() < 0.5) ? P1.e : P2.e;			// 50% chance to take coeff from P1
				child.f = (rand.NextDouble() < 0.5) ? P1.f : P2.f;			// 50% chance to take coeff from P1
				child.g = (rand.NextDouble() < 0.5) ? P1.g : P2.g;			// 50% chance to take coeff from P1
				child.h = (rand.NextDouble() < 0.5) ? P1.h : P2.h;			// 50% chance to take coeff from P1
				child.i = (rand.NextDouble() < 0.5) ? P1.i : P2.i;			// 50% chance to take coeff from P1
				child.j = (rand.NextDouble() < 0.5) ? P1.j : P2.j;			// 50% chance to take coeff from P1
				child.calculate_lmse(ref _knownData);

				offspring.Add(child);
			}

			// Mutations on offspring
			mutateOffspring(ref offspring, ref rand);

			return offspring;
		}


		/*
		 * Mutates the offspring randomly
		 * - mutationRate: percent of genes expected to mutate  (0.0-1.0)
		 * - maxMutation: maximum percentage of original max value a coefficient can be altered through mutation (0.0-1.0)
		 *  Example: Coefficients A-E have max value of 10.  setting maxMutation = 0.1 then the most a coefficient will be altered is 10 * 0.1 = +/- 0.5
		 */
		private void mutateOffspring(ref List<Equation_Parameters> offspring, ref Random rand)
		{
			// Mutation setting = % chance of a gene (coeff) mutation [0.0, 1.0]  


			foreach (Equation_Parameters child in offspring)
			{
				if (rand.NextDouble() < Program.MUTATION_RATE) mutate(child.a, 10.0, ref rand);
				if (rand.NextDouble() < Program.MUTATION_RATE) mutate(child.b, 10.0, ref rand);
				if (rand.NextDouble() < Program.MUTATION_RATE) mutate(child.c, 10.0, ref rand);
				if (rand.NextDouble() < Program.MUTATION_RATE) mutate(child.d, 10.0, ref rand);
				if (rand.NextDouble() < Program.MUTATION_RATE) mutate(child.e, 10.0, ref rand);
				if (rand.NextDouble() < Program.MUTATION_RATE) mutate(child.f, 1.0, ref rand);
				if (rand.NextDouble() < Program.MUTATION_RATE) mutate(child.g, 1.0, ref rand);
				if (rand.NextDouble() < Program.MUTATION_RATE) mutate(child.h, 1.0, ref rand);
				if (rand.NextDouble() < Program.MUTATION_RATE) mutate(child.i, 1.0, ref rand);
				if (rand.NextDouble() < Program.MUTATION_RATE) mutate(child.j, 1.0, ref rand);
			}
		}



		private double mutate(double coeff, double max_base, ref Random rand)
		{
			double delta = max_base * Program.MUTATION_RANGE;				// Max amount a mutation can be
			double mutation = rand.NextDouble() * max_base;
			mutation -= (mutation / 2);
			coeff += mutation;
			++Statistics.mutations;
			return coeff;
		}


	}



}
