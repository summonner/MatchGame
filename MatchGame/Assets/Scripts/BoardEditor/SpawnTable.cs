using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Summoner.MatchGame {
	public class SpawnTable : MonoBehaviour {
		[SerializeField] private List<Record> colors;
		[SerializeField] private List<Record> types;
		[SerializeField] private List<Cell> targets;

		[System.Serializable]
		public class Record {
			public byte value = 0;
			public float probability = 1.0f;
		}

		void Awake() {
			var colorDrawer = new Drawer( colors );
			var typeDrawer = new Drawer( types );
			var spawner = new Spawner( colorDrawer, typeDrawer );

			foreach ( var target in targets ) {
				target.spawner = spawner;
			}
		}

		public class Drawer {
			private readonly Random.IDrawer drawer = null;
			private readonly IList<byte> values = null;

			public Drawer( List<Record> table ) {
				if ( table.Count <= 1 ) {
					drawer = null;
					values = new byte[1] { table.FirstOrDefault().value };
				}
				else {
					drawer = new Random.WeightedDraw( table.Select( e => e.probability ) );
					values = table.Select( e => e.value ).ToArray();
				}
			}

			public byte Draw() {
				if ( drawer == null ) {
					return values[0];
				}

				return values[drawer.Draw()];
			}
		}
	}
}