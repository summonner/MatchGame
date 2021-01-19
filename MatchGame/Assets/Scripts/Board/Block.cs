using UnityEngine;
using Summoner.Util;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public interface IBlock {
		byte color { get; }
	}

	public class Block : MonoBehaviour, IBlock {

		public byte color { get; private set; }
	}
}