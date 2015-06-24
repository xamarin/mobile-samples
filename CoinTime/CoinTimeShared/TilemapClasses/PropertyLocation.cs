using System;
using System.Collections.Generic;
using CocosSharp;

namespace CoinTimeGame.TilemapClasses
{
	public struct PropertyLocation
	{
		public CCTileMapLayer Layer;
		public CCTileMapCoordinates TileCoordinates;

		public float WorldX;
		public float WorldY;
		public Dictionary<string, string> Properties;
	}
}

