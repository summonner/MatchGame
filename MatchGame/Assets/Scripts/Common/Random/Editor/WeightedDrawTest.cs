using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

namespace Summoner.Random.Test {
	public class WeightedDrawTest {

		[TestCase( new float[] { 1, 0, 0, 1 } )]
		[TestCase( new float[] { 0, 0, 0, 1 } )]
		[TestCase( new float[] { 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 100000, 0 } )]
		public void ZeroWeightTest( IList<float> weights ) {
			var drawer = new WeightedDraw( weights );
			for ( var i = 0; i < 100; ++i ) {
				var value = drawer.Draw();
				Assert.AreNotEqual( 0f, weights[value], $"{value}" );
			}
		}

		[TestCase( new[] { 0 } )]
		[TestCase( new[] { 0, 0, 0, 0 } )]
		public void OnlyZeroTest( IList<int> weights ) {
			var drawer = new WeightedDraw( weights );
			for ( var i = 0; i < 100; ++i ) {
				Assert.Less( drawer.Draw(), 0 );
			}

			LogAssert.Expect( LogType.Assert, "No possible elements" );
		}
	}
}