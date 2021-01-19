using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;

namespace Summoner.MatchGame.Test {
	public class FillBoardTest {

		[TestCase( "SimpleFill" )]
		[TestCase( "CenterFill" )]
		public void Test( string testCase ) {
			TestBoard board = new TestBoard( testCase );
			FillBoard fillBoard = new FillBoard();
			var task = fillBoard.Do( board );
			while ( task.IsCompleted == false ) { }
			if ( task.Exception != null ) {
				throw task.Exception;
			}
			board.Test();
		}
	}
}