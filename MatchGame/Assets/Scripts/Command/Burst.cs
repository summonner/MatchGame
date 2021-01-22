using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class Burst : ICommand {
		private readonly CubeCoordinate target;
		
		public Burst( CubeCoordinate selected ) {
			target = selected;
		}

		public void Apply( IBoard board ) {
			board.Destroy( Traverser.Spiral( target, 1 ) );
		}

		public void Undo( IBoard board ) {
			// cannot undo
		}
	}
}