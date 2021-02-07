using UnityEngine;
using System.Collections.Generic;

namespace Summoner.Random {
	public interface IDrawer {
		public delegate float RandomFunc();

		int Draw();
	}
}