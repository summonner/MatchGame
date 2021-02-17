using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Summoner.MatchGame {
	public sealed class Game : MonoBehaviour {
		private Board board;
		private InputReceiver receiver;
		[SerializeField] private BoardLayout layout;

		async void Start() {
			board = layout.Generate();
			if ( board == null ) {
				throw new System.ArgumentException( "board is null" );
			}

			receiver = board.GetComponentInChildren<InputReceiver>();

			await Task.Delay( 1000 );
			var fillBoard = new FillBoard( board );

			while ( true ) {
				await fillBoard.Do();

				var input = await WaitInput();

				input.Apply( board );
			//	if ( board.Match() == false ) {
			//		Undo( input );
			//	}
			}
		}

		public bool burst = true;
		private async Task<ICommand> WaitInput() {
			if ( burst ) {
				var wait = new TaskCompletionSource<CubeCoordinate>();
				receiver.onClick += wait.SetResult;
				var selected = await wait.Task;
				receiver.onClick -= wait.SetResult;

				return new Burst( selected );
			}
			else {
				var wait = new TaskCompletionSource<KeyValuePair<CubeCoordinate, CubeCoordinate>>();
				InputReceiver.OnDragDelegate onReceive = ( selected, direction ) => {
					wait.SetResult( new KeyValuePair<CubeCoordinate, CubeCoordinate>( selected, direction ) );
				};

				receiver.onDrag += onReceive;
				var received = await wait.Task;
				receiver.onDrag -= onReceive;

				return new Line( received.Key, received.Value );
			}
		}
	}
}