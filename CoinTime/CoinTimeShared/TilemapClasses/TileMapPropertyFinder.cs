using System;
using CocosSharp;
using System.Collections.Generic;

namespace CoinTimeGame.TilemapClasses
{
	public class TileMapPropertyFinder
	{
		CCTileMap tileMap;
		public TileMapPropertyFinder (CCTileMap tileMap)
		{
			this.tileMap = tileMap;
		}


		public IEnumerable<PropertyLocation> GetPropertyLocations()
		{
			// Width and Height are equal so we can use either
			int tileDimension = (int)tileMap.TileTexelSize.Width;

			// Find out how many rows and columns are in our tile map
			int numberOfColumns = (int)tileMap.MapDimensions.Size.Width;
			int numberOfRows = (int)tileMap.MapDimensions.Size.Height;

			// Tile maps can have multiple layers, so let's loop through all of them:
			foreach (CCTileMapLayer layer in tileMap.TileLayersContainer.Children)
			{
				// Loop through the columns and rows to find all tiles
				for (int column = 0; column < numberOfColumns; column++)
				{
					// We're going to add tileDimension / 2 to get the position
					// of the center of the tile - this will help us in 
					// positioning entities, and will eliminate the possibility
					// of floating point error when calculating the nearest tile:
					int worldX = tileDimension * column + tileDimension / 2;
					for (int row = 0; row < numberOfRows; row++)
					{
						// See above on why we add tileDimension / 2
						int worldY = tileDimension * row + tileDimension / 2;

						CCTileMapCoordinates tileAtXy = layer.ClosestTileCoordAtNodePosition (new CCPoint (worldX, worldY));

						CCTileGidAndFlags info = layer.TileGIDAndFlags (tileAtXy.Column, tileAtXy.Row);

						if (info != null)
						{
							Dictionary<string, string> properties = null;

							try
							{
								properties = tileMap.TilePropertiesForGID (info.Gid);
							}
							catch
							{
								// CocosSharp crashed here...but this may be fixed in the current version
							}

							if (properties != null)
							{
								yield return new PropertyLocation {
									WorldX = worldX,
									WorldY = worldY,
									Properties = properties,
									Layer = layer,
									TileCoordinates = tileAtXy
								};
							}
						}
					}
				}
			}


		}
	}
}

