using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Summoner.Util.Extension;

namespace Summoner.MatchGame {
	public class Board : MonoBehaviour {

		private IDictionary<CubeCoordinate, Cell> cells = null;
		private CoordConverter converter;
		private GameObject template;
		private AnimScheduler anim;
		private IList<Column> columns = null;

		public void Init( IDictionary<CubeCoordinate, Cell> cells, CoordConverter converter, IList<Column> columns ) {
			this.cells = cells;
			this.converter = converter;
			this.columns = columns;
			template = Resources.Load<GameObject>( "Block/Block" );
			anim = gameObject.GetOrAddComponent<AnimScheduler>();
		}

		public IEnumerator Fill() {
			foreach ( var column in columns ) {
				var numEmpties = 0;
				foreach ( var coord in column ) {
					Debug.Assert( cells.ContainsKey( coord ) );
					if ( cells[coord].block == null ) {
						numEmpties += 1;
					}
					else {
						var emptyCell = cells[coord - column.up * numEmpties];
						Debug.Assert( emptyCell.block == null );
						emptyCell.Swap( cells[coord] );
						anim.Drop( emptyCell );
					}
				}

				if ( column.hasSpawner == true ) {
					var coord = column.top + column.up;
					for ( var i = 0; i < numEmpties; ++i ) {
						var block = SpawnBlock( coord );
						var cell = cells[coord - column.up * numEmpties];
						cell.Put( block );
						coord += column.up;
						anim.Drop( cell );
					}
				}
			}

			yield return new WaitUntil( () => (anim.isPlaying <= 0) );
		}

		private Block SpawnBlock( CubeCoordinate coord ) {
			var block = Instantiate( template );
			block.transform.position = converter.Hex2World( coord );
			return block.GetComponent<Block>();
		}

	}
}