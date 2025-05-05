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

        private bool[,] levelCells;

        private void Start()
        {
            var cellBounds = levelGrid.FloorTileMap.cellBounds;
            levelCells = new bool[cellBounds.size.x, cellBounds.size.y];

            for (int i = 0; i < predefinedGameObjects.Count; i++)
            {
                var tile = levelGrid.Grid.WorldToCell(predefinedGameObjects[i].transform.position);
                levelCells[tile.x, tile.y] = true;
            }
        }

        public bool IsCellFree(Vector3Int cell)
        {
            if (!ValidateCellCoordinates(cell))
            {
                return false;
            }

            return levelCells[cell.x, cell.y];
        }

        public void UpdateCellState(Vector3Int cell, bool isFree)
        {
            if (!ValidateCellCoordinates(cell))
            {
                return;
            }

            levelCells[cell.x, cell.y] = isFree;
        }

        private bool ValidateCellCoordinates(Vector3Int cell)
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