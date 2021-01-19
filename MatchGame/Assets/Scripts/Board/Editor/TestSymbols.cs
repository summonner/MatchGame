using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame.Test {

	public interface ISymbol { }
	public class Spawner : ISymbol {
		public readonly CubeCoordinate down;
		public Spawner( CubeCoordinate down ) {
			this.down = down;
		}
	}

	public class Block : ISymbol {
		public readonly byte value;
		public Block( byte value ) {
			this.value = value;
		}

		public override string ToString() {
			return value.ToString();
		}

		public override bool Equals( object other ) {
			var cell = other as ICell;
			if ( cell != null ) {
				return (cell.block != null)
					&& (cell.block.color == value);
			}

			return base.Equals( other );
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}
	}

	public class Empty : ISymbol {
		public override bool Equals( object other ) {
			var cell = other as ICell;
			if ( cell != null ) {
				return cell.block == null;
			}

			return base.Equals( other );
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}
	}

}