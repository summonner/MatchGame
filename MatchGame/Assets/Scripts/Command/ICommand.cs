using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public interface ICommand {
		void Apply( IBoard board );
		void Undo( IBoard board );
	}
}