using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame {

	public interface ICell {
		IBlock block { get; }
		bool isSpawner { get; }
	}

	[SelectionBase]
	public sealed class Cell : MonoBehaviour, ICell {

		public CubeCoordinate coord { get; private set; }
		public Block block;
		public bool isSpawner { get; private set; }

		void Awake() {
			isSpawner = GetComponent<Spawner>() != null;
		}

		IBlock ICell.block {
			get {
				return block;
			}
		}
	}
}