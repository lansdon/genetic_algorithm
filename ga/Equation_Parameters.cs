﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ga
{
    class Equation_Parameters : IComparable<Equation_Parameters>
    {
		public double id { set; get; }
		public double lmse { set; get; }
		public double a { set; get; }
		public double b { set; get; }
		public double c { set; get; }
		public double d { set; get; }
		public double e { set; get; }
		public double f { set; get; }
		public double g { set; get; }
		public double h { set; get; }
		public double i { set; get; }
		public double j { set; get; }

		// Minimal constructor for building contents from client
		public Equation_Parameters()
		{
			this.id = Program.CHILD_COUNT++;
			this.lmse = 666;
		}

		public Equation_Parameters(ref List<Coord_Pair> knownData, ref Random rand)
		{
			this.id = Program.CHILD_COUNT++;
			randomize_coefficients(ref rand);
			lmse = calculate_lmse(ref knownData);
			//			print();
		}


		// Implement the generic CompareTo method 
		public int CompareTo(Equation_Parameters other)
		{
			// If other is not a valid object reference, this instance is greater. 
			if (other == null) return 1;

			return lmse.CompareTo(other.lmse);
		}


        ////////////////////////////////////////////////
        // Functions for manipulating the coefficients -------------------------------
        ////////////////////////////////////////////////

        ////////////////////////////////////////////////
        /*
         * Randomly select 5 Coefficients between 1-10 and 5 between 0-1
         * - Coeffecients are sorted from low to high for each group of 5
         */
        ////////////////////////////////////////////////
		public void randomize_coefficients(ref Random rand)
        {
			//           Random random = new Random();
            // A-E have values [0-10]
            List<double> a_e = new List<double>();
			for (int count = 0; count < 5; ++count) { a_e.Add((rand.NextDouble() * 10.0)); }
            a_e.Sort();
            a = a_e[0];
            b = a_e[1];
            c = a_e[2];
            d = a_e[3];
            e = a_e[4];
 //           System.Console.WriteLine("A={0}\nB={1}\nC={2}\nD={3}\nE={4}", a_e[0], a_e[1], a_e[2], a_e[3], a_e[4]);

            // F-J have values [0.0-1.0]
            List<double> f_j = new List<double>();
			for (int count = 0; count < 5; ++count) { f_j.Add((rand.NextDouble())); }
            f_j.Sort();
            f = f_j[0];
            g = f_j[1];
            h = f_j[2];
            i = f_j[3];
            j = f_j[4];
//            System.Console.WriteLine("F={0}\nG={1}\nH={2}\nI={3}\nJ={4}\n", f_j[0], f_j[1], f_j[2], f_j[3], f_j[4]);
        }

		public double calculate_lmse(ref List<Coord_Pair> knownData)
		{
			return calculate_lmse(ref knownData, false);
		}

		public double calculate_lmse(ref List<Coord_Pair> knownData, bool saveToFile)
        {
			double sum = 0;
			foreach (Coord_Pair kd in knownData)
			{
				double calculated = f * Math.Sin(a * kd.X) + g * Math.Sin(b * kd.X) + h * Math.Sin(c * kd.X) + i * Math.Sin(d * kd.X) + j * Math.Sin(e * kd.X);
				sum += Math.Pow((kd.Y - calculated), 2);

				if (saveToFile)
				{
					using (System.IO.StreamWriter file = new System.IO.StreamWriter(Statistics.FILE_BEST_RECORD, true))
					{
						file.WriteLine(calculated);
					}
				}
			}
			double new_lmse = sum / knownData.Count();
//			if (!(a < b && b < c && c < d && d < e && f < g && g < h && h < i && i < j))
//			{
//				new_lmse += 1.0;						// Invalid coefficients... Add lmse penalty!
//				System.Console.Write("LMSE COEFFICIENT PENALTY!");
//			}
			lmse = new_lmse;
//			System.Console.WriteLine("LMSE------------------------------>{0}", lmse);
			return lmse;
        }

		public void print()
		{
			System.Console.WriteLine("ID={0}\nLSME={1}\nA={2}\nB={3}\nC={4}\nD={5}\nE={6}\nF={7}\nG={8}\nH={9}\nI={10}\nJ={11}\n", id, lmse, a, b, c, d, e, f, g, h, i, j);
		}



		public void mutate(ref Random rand, ref List<Coord_Pair> knownData)
		{
			if (rand.NextDouble() < Program.MUTATION_RATE)
			{
				Statistics.mutations++;

				int coeff_to_mutate = rand.Next(10);
				double mutation = rand.NextDouble();

				switch (coeff_to_mutate)
				{
					case 0: a = mutation * 10.0; break;
					case 1: b = mutation * 10.0; break;
					case 2: c = mutation * 10.0; break;
					case 3: d = mutation * 10.0; break;
					case 4: e = mutation * 10.0; break;
					case 5: f = mutation; break;
					case 6: g = mutation; break;
					case 7: h = mutation; break;
					case 8: i = mutation; break;
					case 9: j = mutation; break;
				}

				if (coeff_to_mutate < 5)
				{
					List<double> to_sort = new List<double> { a, b, c, d, e };
					to_sort.Sort();
					a = to_sort[0];
					b = to_sort[1];
					c = to_sort[2];
					d = to_sort[3];
					e = to_sort[4];
				}
				else
				{
					List<double> to_sort = new List<double> { f, g, h, i, j };
					to_sort.Sort();
					f = to_sort[0];
					g = to_sort[1];
					h = to_sort[2];
					i = to_sort[3];
					j = to_sort[4];
				}
			}
		}


		public Equation_Parameters mate(Equation_Parameters rhs, ref Random rand, ref List<Coord_Pair> knownData) 
		{
			Equation_Parameters result = new Equation_Parameters();
			result.a = a;
			result.b = b;
			result.c = c;
			result.d = d;
			result.e = e;
			result.f = rhs.f;
			result.g = rhs.g;
			result.h = rhs.h;
			result.i = rhs.i;
			result.j = rhs.j;

			result.mutate(ref rand, ref knownData);
			result.calculate_lmse(ref knownData);
			return result;
		}


    }  // end Equation_Parameters class
}
