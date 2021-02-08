using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Summoner.Util.Extension;
using System.Linq;

namespace Summoner.MatchGame {

	public interface IBoard {
		IEnumerable<KeyValuePair<CubeCoordinate, ICell>> cells { get; }
		ICell this[CubeCoordinate coord] { get; }
		bool HasCell( CubeCoordinate coord );
		void Swap( CubeCoordinate from, CubeCoordinate to );
		void Drop( CubeCoordinate from, CubeCoordinate to );
		IBlock Spawn( CubeCoordinate coord, CubeCoordinate offset );
		void Destroy( IEnumerable<CubeCoordinate> coords );
		void Destroy( CubeCoordinate coord );

		Task WaitAnim();
	}

	public class Board : MonoBehaviour, IBoard {

		private IDictionary<CubeCoordinate, Cell> cells = null;
		private CoordConverter converter;
		private GameObject template;
		private AnimScheduler anim;

		public void Init( IDictionary<CubeCoordinate, Cell> cells, CoordConverter converter ) {
			this.cells = cells;
			this.converter = converter;
			template = Resources.Load<GameObject>( "Block/Block" );
			anim = gameObject.GetOrAddComponent<AnimScheduler>();
		}

		private Block SpawnBlock( CubeCoordinate coord ) {
			var block = Instantiate( template );
			block.transform.position = converter.Hex2World( coord );
			return block.GetComponent<Block>();
		}

		ICell IBoard.this[CubeCoordinate coord] {
			get {
				if ( cells.TryGetValue( coord, out var cell ) ) {
					return cell;
				}
				else {
					return null;
				}
			}
		}

		bool IBoard.HasCell( CubeCoordinate coord ) {
			return cells.ContainsKey( coord );
		}

		void IBoard.Swap( CubeCoordinate from, CubeCoordinate to ) {
			Swap( cells[from], cells[to] );
		}

		void IBoard.Drop( CubeCoordinate from, CubeCoordinate to ) {
			Swap( cells[from], cells[to] );
			Debug.Assert( cells[to].block != null, $"{from} -> {to}" );
			anim.Drop( cells[to] );
		}

		IBlock IBoard.Spawn( CubeCoordinate coord, CubeCoordinate offset ) {
			var block = SpawnBlock( coord + offset );
			cells[coord].block = block;
			anim.Drop( cells[coord] );
			return block;
		}

		IEnumerable<KeyValuePair<CubeCoordinate, ICell>> IBoard.cells {
			get {
				foreach ( var cell in cells ) {
					yield return new KeyValuePair<CubeCoordinate, ICell>( cell.Key, cell.Value );
				}
			}
		}

		private void Swap( Cell from, Cell to ) {
			var temp = from.block;
			from.block = to.block;
			to.block = temp;
		}

		async Task IBoard.WaitAnim() {
			TaskCompletionSource<bool> onAnimFinished = new TaskCompletionSource<bool>();
			StartCoroutine( WaitAnim( onAnimFinished ) );
			await onAnimFinished.Task;
		}

		private IEnumerator WaitAnim( TaskCompletionSource<bool> onFinished ) {
			yield return new WaitWhile( () => ( anim.isPlaying > 0 ) );
			onFinished.SetResult( true );
		}

		public void Destroy( IEnumerable<CubeCoordinate> coords ) {
			foreach ( var coord in coords ) {
				Destroy( coord );
			}
		}

		public void Destroy( CubeCoordinate coord ) {
			if ( cells.TryGetValue( coord, out var cell ) == false ) {
				return;
			}

			if ( cell.block == null ) {
				return;
			}

			Destroy( cell.block.gameObject );
			cell.block = null;
		}
	}
}