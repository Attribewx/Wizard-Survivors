using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InfiniteScroller : MonoBehaviour
{
    [SerializeField] private bool InfiniteHoriz;
    [SerializeField] private bool InfiniteVert;
    [SerializeField] private float Choke = 16f;

    private Transform cameraTransform;
    private Vector3 lastCameraPos;
    private Tilemap tilemap;
    private List<TileInfo> tiles;
    private List<TileInfo> maxHorizontalTiles;
    private List<TileInfo> minHorizontalTiles;
    private List<TileInfo> maxVerticalTiles;
    private List<TileInfo> minVerticalTiles;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private int minCoordX;
    private int maxCoordX;
    private int minCoordY;
    private int maxCoordY;
    private int _width;
    private int _height;
    private Vector2 screenBounds;

    private void Awake()
    {
        Camera mainCamera = FindObjectOfType<Camera>();
        cameraTransform = mainCamera.transform;
        lastCameraPos = cameraTransform.position;

        tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
        tiles = tilemap.GetTiles();

        if(tiles.Count == 0)
        {
            Debug.Log("Tile count is 0");
            return;
        }

        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        maxHorizontalTiles = new List<TileInfo>();
        minHorizontalTiles = new List<TileInfo>();
        maxVerticalTiles = new List<TileInfo>();
        minVerticalTiles = new List<TileInfo>();

        CalculateSize();

        _width = maxCoordX - minCoordX;
        _height = maxCoordY - minCoordY;
    }

    private void CalculateSize()
    {
        maxHorizontalTiles.Clear();
        minHorizontalTiles.Clear();
        maxVerticalTiles.Clear();
        minVerticalTiles.Clear();

        minX = tiles[0].worldPoint.x;
        maxX = tiles[tiles.Count - 1].worldPoint.x;
        minY = tiles[0].worldPoint.y;
        maxY = tiles[tiles.Count - 1].worldPoint.y;

        foreach(TileInfo tile in tiles)
        {
            if(tile.worldPoint.x <= minX)
            {
                minX = tile.worldPoint.x;
                minCoordX = tile.coordinates.x;
            }
            if (tile.worldPoint.x >= maxX)
            {
                maxX = tile.worldPoint.x;
                maxCoordX = tile.coordinates.x;
            }

            if (tile.worldPoint.y <= minY)
            {
                minY = tile.worldPoint.y;
                minCoordY = tile.coordinates.y;
            }

            if (tile.worldPoint.y >= maxY)
            {
                maxY = tile.worldPoint.y;
                maxCoordY = tile.coordinates.y;
            }

        }

        foreach (TileInfo tile in tiles)
        {
            if (tile.worldPoint.x <= minX)
            {
                minHorizontalTiles.Add(tile);
            }
            if (tile.worldPoint.x >= maxX)
            {
                maxHorizontalTiles.Add(tile);
            }

            if (tile.worldPoint.y <= minY)
            {
                minVerticalTiles.Add(tile);
            }

            if (tile.worldPoint.y >= maxY)
            {
                maxVerticalTiles.Add(tile);
            }

        }
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPos;
        if(deltaMovement.Equals(Vector3.zero))
        {
            return;
        }

        if(InfiniteHoriz)
        {
            if(cameraTransform.position.x + screenBounds.x + Choke > maxX)
            {
                MoveHorizontalTiles(minHorizontalTiles, _width);
            }
            else if(cameraTransform.position.x - screenBounds.x -Choke < minX)
            {
                MoveHorizontalTiles(maxHorizontalTiles, -_width);
            }
        }

        if(InfiniteVert)
        {
            if (cameraTransform.position.y + screenBounds.y + Choke > maxY)
            {
                MoveVerticalTiles(minVerticalTiles, _height);
            }
            else if (cameraTransform.position.y - screenBounds.y - Choke < minY)
            {
                MoveVerticalTiles(maxVerticalTiles, -_height);
            }
        }

        lastCameraPos = cameraTransform.position;
    }

    private void MoveVerticalTiles(List<TileInfo> tiles, int amount)
    {
        foreach (TileInfo tile in tiles)
        {
            SetTile(tile, new Vector3Int(tile.coordinates.x , tile.coordinates.y + amount, tile.coordinates.z));
        }
        CalculateSize();
    }

    private void MoveHorizontalTiles(List<TileInfo> tiles, int amount)
    {
        foreach(TileInfo tile in tiles)
        {
            SetTile(tile, new Vector3Int(tile.coordinates.x + amount, tile.coordinates.y, tile.coordinates.z));
        }
        CalculateSize();
    }

    private void SetTile(TileInfo tile, Vector3Int newCoordinates)
    {
        tilemap.SetTile(tile.coordinates, null);
        tile.coordinates = newCoordinates;
        tilemap.SetTile(tile.coordinates, tile.tile);
        tile.worldPoint = tilemap.CellToWorld(tile.coordinates);
    }
}
