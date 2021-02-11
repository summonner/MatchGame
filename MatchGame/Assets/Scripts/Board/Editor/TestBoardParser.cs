using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame.Test {
	public class TestBoardParser {
		public static IEnumerable<IDictionary<CubeCoordinate, ISymbol>> Parse( string filename ) {
			var symbols = new Dictionary<CubeCoordinate, ISymbol>( 8 * 8 );
			foreach ( var layout in ReadLayouts( filename ) ) {
				symbols.Clear();
				foreach ( var token in TraverseTokens( layout ) ) {
					var symbol = ParseToken( token );
					if ( symbol == null ) {
						continue;
					}

					symbols.Add( token.coord, symbol );
				}
				yield return symbols;
			}

		}

		private static IEnumerable<IList<string>> ReadLayouts( string filename ) {
			var path = $"{Application.dataPath}/TestCases/{filename}.txt";
			var lines = System.IO.File.ReadAllLines( path );
			var layout = new List<string>( 8 );
			for ( var y = 0; y < lines.Length; ++y ) {
				var line = lines[y];
				if ( line.IsNullOrEmpty() ) {
					yield return layout.AsReadOnly();
					layout.Clear();
				}
				else {
					line = line.Replace( " ", string.Empty );
					layout.Add( line );
				}
			}
			yield return layout.AsReadOnly();
		}

		private static IEnumerable<Token> TraverseTokens( IList<string> layout ) {
			for ( var i = 0; i < layout.Count; ++i ) {
				var row = layout[i];
				var y = layout.Count - 1 - i;
				for ( var x = 0; x < row.Length; ++x ) {
					yield return new Token( x, y, row[x] );
				}
			}
		}

		private static ISymbol ParseToken( Token token ) {
			switch ( token.value ) {
				case '.':
					return null;
				case '-':
					return new Empty();
				case '¡é':
					return new Spawner( FlatTopDirection.S );
				case '¢Ù':
					return new Spawner( FlatTopDirection.SE );
				case '¢×':
					return new Spawner( FlatTopDirection.SW );
				case '*':
					return new Block( 0 );
				default:
					throw new System.FormatException( $"Unknown Symbol : {token}" );
			}
		}

		private struct Token {
			public readonly int x;
			public readonly int y;
			public readonly char value;

			public Token( int x, int y, char value ) {
				this.x = x;
				this.y = y;
				this.value = value;
			}

			public CubeCoordinate coord {
				get {
					return new CubeCoordinate( x, y - x / 2 );
				}
			}

			public override string ToString() {
				return $"( {x}, {y} ) {value}";
			}
		}

	}
}