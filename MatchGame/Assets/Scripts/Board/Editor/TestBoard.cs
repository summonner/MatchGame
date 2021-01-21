using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Summoner.MatchGame.Test {
	public class TestBoard : IBoard {
		private IDictionary<CubeCoordinate, TestCell> cells = new Dictionary<CubeCoordinate, TestCell>( 8 * 8 );
		private IList<Column> columns = new List<Column>( 8 );
		private IEnumerator<IDictionary<CubeCoordinate, ISymbol>> source = null;
		public TestBoard( string fileName ) {
			source = TestBoardParser.Parse( fileName ).GetEnumerator();
			source.MoveNext();
			foreach ( var symbol in source.Current ) {
				cells.Add( symbol.Key, new TestCell() );
			}

			var size = 8;
			for ( var x = 0; x < size; ++x ) {
				var offset = x / 2;
				var bottom = new CubeCoordinate( x, -offset );
				var top = new CubeCoordinate( x, size - 1 - offset );
				source.Current.TryGetValue( top, out var cell );
				columns.Add( new Column( bottom, FlatTopDirection.N, top, cell is Spawner ) );
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

		IList<Column> IBoard.columns {
			get {
				return columns;
			}
		}

		void IBoard.Drop( CubeCoordinate from, CubeCoordinate to ) {
			((IBoard)this).Swap( from, to );
		}

		bool IBoard.HasCell( CubeCoordinate coord ) {
			return cells.ContainsKey( coord );
		}

		IBlock IBoard.Spawn( CubeCoordinate coord, CubeCoordinate offset ) {
			var block = new TestBlock();
			cells[coord].block = block;
			return block;
		}

		void IBoard.Swap( CubeCoordinate from, CubeCoordinate to ) {
			var temp = cells[from].block;
			cells[from].block = cells[to].block;
			cells[to].block = temp;
		}

		Task IBoard.WaitAnim() {
			return Task.FromResult( true );
		}


		private class TestCell : ICell {
			public CubeCoordinate coord { get; set; }
			public IBlock block { get; set; }

			public override string ToString() {
				if ( block == null ) {
					return coord + ", empty";
				}

				return coord + ", " + block.ToString();
			}
		}

		private class TestBlock : IBlock {
			public byte color { get; set; }

			public override string ToString() {
				return color.ToString();
			}
		}
	}
}