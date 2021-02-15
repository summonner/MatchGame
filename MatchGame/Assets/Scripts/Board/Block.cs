using UnityEngine;
using Summoner.Util;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public interface IBlock {
		byte color { get; }
		byte type { get; }
	}

	[SelectionBase]
	public sealed class Block : MonoBehaviour, IBlock {

		public byte color { get; private set; }
		public byte type { get; private set; }

		public void Init( IBlock archetype ) {
			color = archetype.color;
			type = archetype.type;

			SetColor( color );
		}

		private static readonly Color[] colors = { Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white };
		private void SetColor( byte color ) {
			GetComponentInChildren<Renderer>().material.color = colors[color];
		}
	}
}