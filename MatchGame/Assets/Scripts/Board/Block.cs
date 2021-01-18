using UnityEngine;
using Summoner.Util;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class Block : MonoBehaviour {

		public enum Elemental { 
			Water,
			Wind,
			Earth,
			Fire,
			Light,
			Dark,
			Gold,
		}

		public Elemental elemental { get; private set; }
	}
}