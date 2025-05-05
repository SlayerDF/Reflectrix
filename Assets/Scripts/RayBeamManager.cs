using LaserBeam;
using Reflectrix.Entities.Devices;
using System.Collections.Generic;
using UnityEngine;

namespace Reflectrix
{
    public class RayBeamManager : MonoBehaviour
    {
        private const float rayWidthMultiplier = 0.05f;

        [SerializeField]
        private List<Emitter> emitters = new();

        [SerializeField]
        private LevelGrid levelGrid;

        [SerializeField]
        private LineRenderer linePrefab;

        [SerializeField]
        private LayerMask obstaclesLayerMask;

        private readonly List<LineRenderer> activeLines = new();

        private int tileMapSize;

        private BoundsInt tileBounds;

        private void Start()
        {
            tileBounds = levelGrid.FloorTileMap.cellBounds;
            tileMapSize = tileBounds.size.x * tileBounds.size.y;

            foreach (var emitter in emitters)
            {
                var points = emitter.LaserEmitterTrait.Emit();
                for (int i = 0; i < points.Length; i++)
                {
                    DrawRay(points[i]);
                }
            }
        }

        private bool IsPointInBounds(Vector3Int point, BoundsInt bounds)
        {
            return point.x >= bounds.min.x &&
                   point.y >= bounds.min.y &&
                   point.x <= bounds.max.x &&
                   point.y <= bounds.max.y;
        }

        private void DrawRay(LaserBeamPoint laserBeam)
        {
            var path = new List<Vector3>();
            var currentTile = levelGrid.Grid.WorldToCell(laserBeam.Origin);
            var currentWorldPos = levelGrid.Grid.GetCellCenterWorld(currentTile);
            var step = laserBeam.Direction.normalized * levelGrid.Grid.cellSize.x;
            var stepSize = levelGrid.Grid.cellSize.x * 0.5f;
            var drawLineEnd = true;

            for (int i = 0; i < tileMapSize; i++)
            {
                if (!IsPointInBounds(currentTile, tileBounds))
                    break;

                if (levelGrid.FloorTileMap.GetTile(currentTile) == null)
                    break;

                var hit = Physics2D.Raycast(currentWorldPos, step, stepSize, obstaclesLayerMask);
                if (hit.collider != null)
                {
                    path.Add(hit.point);
                    drawLineEnd = false;
                    break;
                }

                path.Add(currentWorldPos);
                currentWorldPos += step;
                currentTile = levelGrid.Grid.WorldToCell(currentWorldPos);
            }

            // Add one more point to finish the line at the end of the tile
            // and not on the center of the tile.
            if (path.Count > 0 && drawLineEnd)
            {
                Vector3 finalPos = path[^1] + step * 0.5f;
                path.Add(finalPos);
            }

            // Create line renderer.
            if (path.Count >= 2)
            {
                var line = Instantiate(linePrefab);
                line.startWidth = laserBeam.Intensity * rayWidthMultiplier;
                line.endWidth = laserBeam.Intensity * rayWidthMultiplier;
                line.startColor = laserBeam.Color;
                line.endColor = laserBeam.Color;

                line.positionCount = path.Count;
                line.SetPositions(path.ToArray());
                activeLines.Add(line);
            }
        }

        private void ClearLines()
        {
            for (int i = 0; i < activeLines.Count; i++)
            {
                if (activeLines[i] != null)
                {
                    Destroy(activeLines[i].gameObject);
                }
            }
            activeLines.Clear();
        }

        private void OnDestroy()
        {
            ClearLines();
        }
    }
}