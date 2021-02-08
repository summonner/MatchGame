using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame {

	public interface ICell {
		IBlock block { get; }
		bool isSpawner { get; }
	}

	public class Cell : MonoBehaviour, ICell {

		public CubeCoordinate coord { get; private set; }
		public Block block;
		public bool isSpawner { get; private set; }

		public void Init( CubeCoordinate coord, Vector3 position, bool isSpawner ) {
			this.coord = coord;
			transform.localPosition = position;
			name = coord.ToString();
			this.isSpawner = isSpawner;
		}

		IBlock ICell.block { 
			get {
				return block;
			}
		}
	}
}