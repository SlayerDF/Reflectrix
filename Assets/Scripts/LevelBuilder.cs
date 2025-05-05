using Reflectrix.Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Reflectrix
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField]
        private TilemapCollider2D tilemapCollider;

        [SerializeField]
        private LevelGrid levelGrid;

        [SerializeField]
        private List<GameObject> predefinedGameObjects = new();

        private TileState[,] levelCells;

        private void Start()
        {
            var cellBounds = levelGrid.FloorTileMap.cellBounds;
            levelCells = new TileState[cellBounds.size.x, cellBounds.size.y];

            for (int i = 0; i < levelGrid.ObstaclesTileMap.cellBounds.size.x; i++)
            {
                for (int j = 0; j < levelGrid.ObstaclesTileMap.cellBounds.size.y; j++)
                {
                    var hasTile = levelGrid.ObstaclesTileMap.HasTile(ConvertCoordinatesFromInternalFormat(i, j, cellBounds.min));
                    if (hasTile)
                    {
                        levelCells[i, j].OccupyTile(TileObjectType.Obstacle);
                    }
                }
            }

            for (int i = 0; i < predefinedGameObjects.Count; i++)
            {
                var tile = levelGrid.Grid.WorldToCell(predefinedGameObjects[i].transform.position);
                var localCoordinates = ConvertCoordinatesToInternalFormat(tile, cellBounds.min);
                levelCells[localCoordinates.x, localCoordinates.y].OccupyTile(TileObjectType.PredifinedObject);
            }
        }

        private Vector2Int ConvertCoordinatesToInternalFormat(Vector3Int tile, Vector3Int boundsMin)
        {
            var x = tile.x - boundsMin.x;
            var y = tile.y - boundsMin.y;
            return new Vector2Int(x, y);
        }

        private Vector3Int ConvertCoordinatesFromInternalFormat(int x, int y, Vector3Int boundsMin)
        {
            var tileX = x + boundsMin.x;
            var tileY = y + boundsMin.y;
            return new Vector3Int(tileX, tileY);
        }

        public bool IsCellFree(Vector3Int cell)
        {
            var internalCell = ConvertCoordinatesToInternalFormat(cell, levelGrid.FloorTileMap.cellBounds.min);
            if (!ValidateCellCoordinates(internalCell))
            {
                return false;
            }

            return !levelCells[internalCell.x, internalCell.y].IsOccupied;
        }

        public void UpdateCellState(Vector3Int cell, bool isFree)
        {
            var internalCell = ConvertCoordinatesToInternalFormat(cell, levelGrid.FloorTileMap.cellBounds.min);
            if (!ValidateCellCoordinates(internalCell))
            {
                return;
            }

            if (!isFree)
            {
                // TODO: Add the logic to desoccupy the tile.
                return;
            }

            levelCells[internalCell.x, internalCell.y].OccupyTile(TileObjectType.CustomObject);
        }

        private bool ValidateCellCoordinates(Vector2Int cell)
        {
            if (cell.x >= levelCells.GetLength(0) || cell.x < 0 ||
                cell.y >= levelCells.GetLength(1) || cell.y < 0)
            {
                Debug.LogError("Cell is out of bounds: " + cell);
                return false;
            }

            return true;
        }
    }
}