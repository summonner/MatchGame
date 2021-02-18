using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame {

	public interface ICell {
		IBlock block { get; }
		bool isSpawner { get; }
	}

	[SelectionBase]
	public sealed class Cell : MonoBehaviour, ICell {
		[HideInInspector] public Block block = null;
		public bool isSpawner => GetComponent<Spawner>() != null;

		IBlock ICell.block {
			get {
				return block;
			}
		}
	}
}