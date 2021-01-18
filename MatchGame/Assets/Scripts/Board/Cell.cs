using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame {

	public interface ICell { 
		CubeCoordinate coord { get; }
		IBlock block { get; }
	}

	public class Cell : MonoBehaviour, ICell {

		public CubeCoordinate coord { get; private set; }
		public Block block;

		public void Init( CubeCoordinate coord, Vector3 position ) {
			this.coord = coord;
			transform.localPosition = position;
			name = coord.ToString();
		}

		IBlock ICell.block { 
			get {
				return block;
			}
		}
	}
}