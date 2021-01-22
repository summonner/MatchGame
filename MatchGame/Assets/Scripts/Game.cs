using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Summoner.MatchGame {
	public class Game : MonoBehaviour {
		private Board board;
		private InputReceiver receiver;

		public void Init( Board board, InputReceiver receiver ) {
			this.board = board;
			this.receiver = receiver;
		}

		async void Start() {
			Debug.Assert( board != null );
			await Task.Delay( 1000 );
			var fillBoard = new FillBoard();

			while ( true ) {
				await fillBoard.Do( board );

				var input = await WaitInput();

				input.Apply( board );
			//	if ( board.Match() == false ) {
			//		Undo( input );
			//	}
			}
		}

		private async Task<ICommand> WaitInput() {
			var wait = new TaskCompletionSource<CubeCoordinate>();
			receiver.onClick += wait.SetResult;
			var selected = await wait.Task;
			receiver.onClick -= wait.SetResult;

			return new Burst( selected );
		}

		// 클릭하면 주변 블럭 제거.
		// 블럭들이 수직으로 우선 떨어지도록 추가.
	}
}