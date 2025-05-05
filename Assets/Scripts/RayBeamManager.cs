using System.Collections.Generic;
using Reflectrix.Entities.Devices;
using Reflectrix.LaserBeam;
using UniTrait;
using UnityEngine;

namespace Reflectrix
{
    public class RayBeamManager : MonoBehaviour
    {
        private const float rayWidthMultiplier = 0.05f;
        private const bool canIntersect = false;

        #region Serialized Fields

        [SerializeField]
        private List<Emitter> emitters = new();

        [SerializeField]
        private LevelGrid levelGrid;

        [SerializeField]
        private LineRenderer linePrefab;

        [SerializeField]
        private LayerMask obstaclesLayerMask;

        [SerializeField]
        private LayerMask devicesLayerMask;

        #endregion Serialized Fields

        private readonly List<LineRenderer> activeLines = new();

        private BoundsInt tileBounds;

        private readonly Dictionary<Vector3Int, Color> occupiedTiles = new();

        private readonly HashSet<IBeamReceiverAndEmitter> visitedDevices = new();

        private int tileMapSize;

        #region Event Functions

        private void Start()
        {
            tileBounds = levelGrid.FloorTileMap.cellBounds;
            tileMapSize = tileBounds.size.x * tileBounds.size.y;

            foreach (var emitter in emitters)
            {
                var points = emitter.Emit();
                for (var i = 0; i < points.Length; i++)
                {
                    DrawRay(points[i]);
                }
            }
        }

        private void OnDestroy()
        {
            Clear();
        }

        #endregion Event Functions

        private bool IsPointInBounds(Vector3Int point, BoundsInt bounds)
        {
            return point.x >= bounds.min.x &&
                   point.y >= bounds.min.y &&
                   point.x <= bounds.max.x &&
                   point.y <= bounds.max.y;
        }

        private void Update()
        {
            Clear();
            foreach (var emitter in emitters)
            {
                var points = emitter.Emit();
                for (var i = 0; i < points.Length; i++)
                {
                    DrawRay(points[i]);
                }
            }
        }

        private void DrawRay(ILaserBeamPoint laserBeam)
        {
            var path = new List<Vector3>();
            var currentTile = levelGrid.Grid.WorldToCell(laserBeam.Origin);
            var currentWorldPos = levelGrid.Grid.GetCellCenterWorld(currentTile);
            var step = laserBeam.Direction.normalized * levelGrid.Grid.cellSize.x;
            var stepSize = levelGrid.Grid.cellSize.x * 0.5f;
            var drawLineEnd = true;

            for (var i = 0; i < tileMapSize; i++)
            {
                if (!IsPointInBounds(currentTile, tileBounds))
                {
                    break;
                }

                if (levelGrid.FloorTileMap.GetTile(currentTile) == null)
                {
                    break;
                }

                if (occupiedTiles.ContainsKey(currentTile))
                {
                    break;
                }

                // Check if the tile is already occupied by another laser beam.
                if (!canIntersect && occupiedTiles.ContainsKey(currentTile))
                {
                    if (occupiedTiles[currentTile] != laserBeam.Color)
                    {
                        path.Add(currentWorldPos);
                        drawLineEnd = false;
                        SpawnIntersectionFX(currentWorldPos);
                        break;
                    }
                }

                occupiedTiles[currentTile] = laserBeam.Color;

                // Check if there is an obstacle in the way.
                var hit = Physics2D.Raycast(currentWorldPos, step, stepSize, obstaclesLayerMask);
                if (hit.collider != null)
                {
                    path.Add(hit.point);
                    drawLineEnd = false;
                    break;
                }

                // Check if there is a device in the way.
                var hitDevice = Physics2D.Raycast(currentWorldPos, step, stepSize, devicesLayerMask);
                if (hitDevice.collider != null)
                {
                    var receiverAndEmitter = hitDevice.collider.gameObject.GetComponentInParent<IBeamReceiverAndEmitter>();
                    if (receiverAndEmitter != null && !visitedDevices.Contains(receiverAndEmitter))
                    {
                        var emittedBeams = receiverAndEmitter.ReceiveAndEmit(new ILaserBeamPoint[]
                        {
                            new LaserBeamPoint(currentWorldPos, laserBeam.Direction, laserBeam.Intensity, laserBeam.Color)
                        });

                        foreach (var emitted in emittedBeams)
                        {
                            DrawRay(emitted);// Recurse for each new beam
                        }
                    }
                }

                path.Add(currentWorldPos);
                currentWorldPos += step;
                currentTile = levelGrid.Grid.WorldToCell(currentWorldPos);
            }

            // Add one more point to finish the line at the end of the tile
            // and not on the center of the tile.
            if (path.Count > 0 && drawLineEnd)
            {
                var finalPos = path[^1] + step * 0.5f;
                path.Add(finalPos);
            }

            // Create line renderer.
            if (path.Count >= 2)
            {
                var line = Instantiate(linePrefab);
                var baseWidth = laserBeam.Intensity * rayWidthMultiplier;
                line.startWidth = baseWidth;
                line.endWidth = baseWidth;
                line.startColor = laserBeam.Color;
                line.endColor = laserBeam.Color;

                line.positionCount = path.Count;
                line.SetPositions(path.ToArray());
                activeLines.Add(line);
            }
        }

        private void SpawnIntersectionFX(Vector3 currentWorldPos)
        {
            // TODO: Spawn intersection FX
        }

        private void Clear()
        {
            for (var i = 0; i < activeLines.Count; i++)
            {
                if (activeLines[i] != null)
                {
                    Destroy(activeLines[i].gameObject);
                }
            }

            activeLines.Clear();

            occupiedTiles.Clear();
        }
    }
}