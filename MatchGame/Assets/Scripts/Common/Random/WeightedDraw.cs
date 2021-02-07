using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Summoner.Random {
	public class WeightedDraw : IDrawer {
		private readonly IDrawer.RandomFunc random = null;
		private readonly SortedList<float, int> weights = null;

		private float max {
			get {
				return weights.Keys[weights.Count - 1];
			}
		}

		public WeightedDraw( IEnumerable<int> weights )
			: this( Convert( weights ) ) { }

		public WeightedDraw( IEnumerable<int> weights, IDrawer.RandomFunc random )
			: this( Convert( weights ), random ) { }

		private static IEnumerable<float> Convert( IEnumerable<int> weights ) {
			var converted = new List<float>( weights.Count() );
			foreach ( var weight in weights ) {
				converted.Add( weight );
			}
			return converted;
		}

		public WeightedDraw( IEnumerable<float> weights )
			: this( weights, () => UnityEngine.Random.value ) { }

		public WeightedDraw( IEnumerable<float> weights, IDrawer.RandomFunc random ) {
			this.random = random;
			this.weights = Cumulate( weights );
			Debug.Assert( this.weights.Count > 0, "No possible elements" );
		}

		private static SortedList<float, int> Cumulate( IEnumerable<float> weights ) {
			var cumulate = new SortedList<float, int>( weights.Count() );
			var sum = 0.0f;
			var i = 0;
			foreach ( var weight in weights ) {
				if ( weight > 0 ) {
					sum += weight;
					cumulate.Add( sum, i );
				}
				++i;
			}

			return cumulate;
		}

		public int Draw() {
			if ( weights.Count <= 0 ) {
				return -1;
			}

			var value = random();
			return BinarySearch( value * max );
		}

		private int BinarySearch( float value ) {
			var a = 0;
			var b = weights.Count - 1;

			while ( a <= b ) {
				var m = (a + b) / 2;
				var compared = value - weights.Keys[m];
				if ( compared.Equals( 0 ) ) {
					return weights.Values[m];
				}
				else if ( compared < 0 ) {
					b = m - 1;
				}
				else {
					a = m + 1;
				}
			}

			return weights.Values[a];
		}
	}
}