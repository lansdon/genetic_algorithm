using System;
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

		// Implement the generic CompareTo method with the Temperature  
		// class as the Type parameter.  
		// 
		public int CompareTo(Equation_Parameters other)
		{
			// If other is not a valid object reference, this instance is greater. 
			if (other == null) return 1;

			// The temperature comparison depends on the comparison of  
			// the underlying Double values.  
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
            // F*math:sin(A*X_k) + G*math:sin(B*X_k) + H*math:sin(C*X_k) + I*math:sin(D*X_k) + J*math:sin(E*X_k)
			double sum = 0;
			foreach (Coord_Pair kd in knownData)
			{
				double calculated = f * Math.Sin(a * kd.X) + g * Math.Sin(b * kd.X) + h * Math.Sin(c * kd.X) + i * Math.Sin(d * kd.X) + j * Math.Sin(e * kd.X);
				sum += Math.Pow((kd.Y - calculated), 2);
			}

            return sum/knownData.Count();
        }

		public void print()
		{
			System.Console.WriteLine("ID={0}\nLSME={1}\nA={2}\nB={3}\nC={4}\nD={5}\nE={6}\nF={7}\nG={8}\nH={9}\nI={10}\nJ={11}\n", id, lmse, a, b, c, d, e, f, g, h, i, j);
		}

    }  // end Equation_Parameters class
}
