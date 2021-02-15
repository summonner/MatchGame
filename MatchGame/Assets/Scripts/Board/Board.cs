using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Summoner.MatchGame {

	public interface IBoard {
		IEnumerable<KeyValuePair<CubeCoordinate, ICell>> cells { get; }
		ICell this[CubeCoordinate coord] { get; }
		bool HasCell( CubeCoordinate coord );
		void Swap( CubeCoordinate from, CubeCoordinate to );
		void Drop( CubeCoordinate from, CubeCoordinate to );
		void Spawn( CubeCoordinate coord, int count );
		void Destroy( IEnumerable<CubeCoordinate> coords );
		void Destroy( CubeCoordinate coord );

		Task WaitAnim();
	}

	[SelectionBase]
	public sealed class Board : MonoBehaviour, IBoard {

		private IDictionary<CubeCoordinate, Cell> cells = null;
		private CoordConverter converter = null;
		private Block template = null;
		private AnimScheduler anim = null;

		public void Init( IDictionary<CubeCoordinate, Cell> cells, CoordConverter converter ) {
			this.cells = cells;
			this.converter = converter;
			template = Resources.Load<Block>( "Block/Block" );
			anim = gameObject.GetOrAddComponent<AnimScheduler>();
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

		void IBoard.Spawn( CubeCoordinate coord, int count ) {
			Debug.Assert( count > 0 );

			var spawner = cells[coord].spawner;
			Debug.Assert( spawner != null, $"Tried to spawn a block from non-spawner cell, {coord}" );

			var offset = FlatTopDirection.N * count;
			var p = coord;
			for ( var i = 0; i < count; ++i ) {
				var cell = cells[p];
				cell.block = SpawnBlock( spawner.Draw(), p + offset );
				anim.Drop( cell );
				p -= FlatTopDirection.N;
			}
		}

		private Block SpawnBlock( IBlock archetype, CubeCoordinate coord ) {
			var block = Instantiate( template );
			block.Init( archetype );
			block.transform.position = converter.Hex2World( coord );
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