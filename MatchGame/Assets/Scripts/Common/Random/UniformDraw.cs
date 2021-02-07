using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.Random {
	public class UniformDraw : IDrawer {
		private readonly IDrawer.RandomFunc random;
		private readonly int count;

		public UniformDraw( int count, IDrawer.RandomFunc random ) {
			this.count = count;
			this.random = random;
		}

		public int Draw() {
			var value = random();
			if ( value >= 1f ) {
				return count - 1;
			}

			return Mathf.FloorToInt( value * count );
		}
	}
}