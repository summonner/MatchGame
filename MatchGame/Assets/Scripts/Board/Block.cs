using UnityEngine;
using Summoner.Util;
using System.Collections.Generic;

namespace Summoner.MatchGame {

	public interface IBlock {
		enum Elemental {
			Water,
			Wind,
			Earth,
			Fire,
			Light,
			Dark,
			Gold,
		};

		Elemental elemental { get; }
	}

	public class Block : MonoBehaviour, IBlock {

		public IBlock.Elemental elemental { get; private set; }
	}
}