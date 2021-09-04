using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TileSurvival
{
    class PerlinNoise
    {
		private static readonly int[] permutation;

		//private static readonly int[] permutation = { 151,160,137,91,90,15,					// Hash lookup table as defined by Ken Perlin.  This is a randomly
		//131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,	// arranged array of all numbers from 0-255 inclusive.
		//190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
		//88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
		//77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
		//102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
		//135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
		//5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
		//223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
		//129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
		//251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
		//49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
		//138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
		//};

		private static void CalculatePermutation(out int[] a)
		{
			var p = Enumerable.Range(0, 256).ToArray();

			var rand = new Random(Game1.seed);
			/// shuffle the array
			for (var i = 0; i < p.Length; i++)
			{
				var source = rand.Next(p.Length);

				var t = p[i];
				p[i] = p[source];
				p[source] = t;
			}
			a = new int[512];
			p.CopyTo(a, 0);
			p.CopyTo(a, 256);
			
		}
		static PerlinNoise()
        {
			CalculatePermutation(out permutation);
        }
		static float Fade(float t)
		{
			return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
		}
		static Vector2 Gradient(Vector2 p)
        {
			//Vector2 value = new Vector2(permutation[(int)p.X & 255], permutation[(int)p.Y & 255]);
			//return Vector2.Normalize(value);
			var a = Vector2.Normalize( new Vector2(permutation[permutation[permutation[(int)p.X & 255] + ((int)p.Y & 255)]], permutation[permutation[permutation[(int)p.Y & 255] + ((int)p.X & 255)]]));
			return a - a / 2;
		}
		/* 2D noise */
		public static float Noise(Vector2 p)
		{
			/* Calculate lattice points. */
			Vector2 p0 = Vector2.Floor(p);
			Vector2 p1 = p0 + new Vector2(1.0f, 0.0f);
			Vector2 p2 = p0 + new Vector2(0.0f, 1.0f);
			Vector2 p3 = p0 + new Vector2(1.0f, 1.0f);

			/* Look up gradients at lattice points. */
			Vector2 g0 = Gradient(p0);
			Vector2 g1 = Gradient(p1);
			Vector2 g2 = Gradient(p2);
			Vector2 g3 = Gradient(p3);

			float t0 = p.X - p0.X;
			float fade_t0 = Fade(t0); /* Used for interpolation in horizontal direction */

			float t1 = p.Y - p0.Y;
			float fade_t1 = Fade(t1); /* Used for interpolation in vertical direction. */

			/* Calculate dot products and interpolate.*/
			float p0p1 = (1.0f - fade_t0) * Vector2.Dot(g0, (p - p0)) + fade_t0 * Vector2.Dot(g1, (p - p1)); /* between upper two lattice points */
			float p2p3 = (1.0f - fade_t0) * Vector2.Dot(g2, (p - p2)) + fade_t0 * Vector2.Dot(g3, (p - p3)); /* between lower two lattice points */

			/* Calculate final result */
			return (1.0f - fade_t1) * p0p1 + fade_t1 * p2p3;
		}

	}
}
