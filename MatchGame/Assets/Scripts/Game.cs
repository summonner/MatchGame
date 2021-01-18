using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class Game : MonoBehaviour {
		private Board board;

		public void Init( Board board ) {
			this.board = board;
		}

		IEnumerator Start() {
			Debug.Assert( board != null );

			//while ( true ) {
			//	do {
					StartCoroutine( board.Fill() );
			//	} while ( board.Match() );

			//	var input = await WaitInput();

			//	Process( input );
			//	if ( board.Match() == false ) {
			//		Undo( input );
			//	}
			//}
			yield break;
		}

	}
}