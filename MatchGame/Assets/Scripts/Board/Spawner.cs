using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public sealed class Spawner {
		private readonly SpawnTable.Drawer colors;
		private readonly SpawnTable.Drawer types;

		public Spawner( SpawnTable.Drawer colors, SpawnTable.Drawer types ) {
			this.colors = colors;
			this.types = types;
		}

		public IBlock Draw() {
			var color = colors.Draw();
			var type = types.Draw();
			return new Archetype( color, type );
		}

		private class Archetype : IBlock { 
			public byte color { get; private set; }
			public byte type { get; private set; }

			public Archetype( byte color, byte type ) {
				this.color = color;
				this.type = type;
			}
		}
	}
}