using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class Cell : MonoBehaviour {

		public CubeCoordinate coord { get; private set; }
		public Block block { get; private set; }

		public void Init( CubeCoordinate coord, Vector3 position ) {
			this.coord = coord;
			transform.localPosition = position;
			name = coord.ToString();
		}

		public void Put( Block newBlock ) {
			Debug.Assert( block == null );
			block = newBlock;
		}

		public void Swap( Cell other ) {
			var t = other.block;
			other.block = block;
			block = t;
		}
	}
}