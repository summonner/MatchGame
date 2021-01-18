using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class Game : MonoBehaviour {
		private Board board;

		public void Init( Board board ) {
			this.board = board;
		}

		async void Start() {
			Debug.Assert( board != null );

			var fillBoard = new FillBoard();
			//while ( true ) {
			//	do {
				await fillBoard.Do( board );
			//	} while ( board.Match() );

			//	var input = await WaitInput();

			//	Process( input );
			//	if ( board.Match() == false ) {
			//		Undo( input );
			//	}
			//}
		}

	}
}