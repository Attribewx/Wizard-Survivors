using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class TileInfo
{
    public Vector3Int coordinates;

    public Vector3 worldPoint;

    public Tile tile;
}

public static class TileMapEX
{
    public static List<TileInfo> GetTiles(this Tilemap tilemap)
    {
        List<TileInfo> tileDataList = new List<TileInfo>();
        for (int y = tilemap.origin.y; y <  (tilemap.origin.y + tilemap.size.y); y++)
        {
            for(int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
            {
                Vector3Int coordinates = new Vector3Int(x, y, 0);
                Tile tile = tilemap.GetTile<Tile>(coordinates);
                if (tile != null)
                {
                    TileInfo tileinfo = new TileInfo()
                    {
                        tile = tile,
                        coordinates = coordinates,
                        worldPoint = tilemap.CellToWorld(coordinates)
                    };

                    tileDataList.Add(tileinfo);
                }
            }
        }
        return tileDataList;
    }
}
