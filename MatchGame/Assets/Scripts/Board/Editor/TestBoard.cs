using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Summoner.MatchGame.Test {
	public sealed class TestBoard : IBoard {
		private IDictionary<CubeCoordinate, TestCell> cells = new SortedList<CubeCoordinate, TestCell>( 8 * 8, BottomLeftToTopRight.Instance );
		private IEnumerator<IDictionary<CubeCoordinate, ISymbol>> source = null;
		public TestBoard( string fileName ) {
			source = TestBoardParser.Parse( fileName ).GetEnumerator();
			source.MoveNext();
			foreach ( var symbol in source.Current ) {
				cells.Add( symbol.Key, new TestCell { coord = symbol.Key, isSpawner = symbol.Value is Spawner } );
			}
		}

		public void Test() {
			Assert.IsTrue( source.MoveNext() );
			foreach ( var symbol in source.Current ) {
				if ( symbol.Value is Spawner ) {
					continue;
				}

				Assert.IsTrue( cells.ContainsKey( symbol.Key ), "cannot find a cell {0}", symbol.Key );
				Assert.AreEqual( symbol.Value, cells[symbol.Key] );
			}
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

		IEnumerable<KeyValuePair<CubeCoordinate, ICell>> IBoard.cells {
			get {
				foreach ( var cell in cells ) {
					yield return new KeyValuePair<CubeCoordinate, ICell>( cell.Key, cell.Value );
				}
			}
		}

		void IBoard.Drop( CubeCoordinate from, CubeCoordinate to ) {
			((IBoard)this).Swap( from, to );
		}

		bool IBoard.HasCell( CubeCoordinate coord ) {
			return cells.ContainsKey( coord );
		}

		void IBoard.Spawn( CubeCoordinate coord, int count ) {
			for ( int i = 0; i < count; ++i ) {
				var p = coord + i * FlatTopDirection.S;
				cells[p].block = new TestBlock();
			}
		}

		void IBoard.Swap( CubeCoordinate from, CubeCoordinate to ) {
			var temp = cells[from].block;
			cells[from].block = cells[to].block;
			cells[to].block = temp;
		}

		void IBoard.MoveTimeline() { 
			// do nothing
		}

		Task IBoard.WaitAnim() {
			return Task.FromResult( true );
		}

		public void Destroy( CubeCoordinate coord ) { 
			if ( cells.TryGetValue( coord, out var cell ) == false ) {
				return;
			}

			cell.block = null;
		}

		public void Destroy( IEnumerable<CubeCoordinate> coords ) {
			foreach ( var coord in coords ) {
				Destroy( coord );
			}
		}

		private class TestCell : ICell {
			public CubeCoordinate coord { get; set; }
			public IBlock block { get; set; }
			public bool isSpawner { get; set; }

			public override string ToString() {
				if ( block == null ) {
					return coord + ", empty";
				}

				return coord + ", " + block.ToString();
			}
		}

		private class TestBlock : IBlock {
			public byte color { get; set; }
			public byte type { get; set; }

			public override string ToString() {
				return $"{color}, {type}";
			}
		}
	}
}