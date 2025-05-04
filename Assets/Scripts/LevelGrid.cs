using UnityEngine;
using UnityEngine.Tilemaps;

namespace Reflectrix
{
    [RequireComponent(typeof(Tilemap))]
    public class LevelGrid : MonoBehaviour
    {
        [SerializeField]
        private Grid grid;

        /// <summary>
        /// Current level grid.
        /// </summary>
        public Grid Grid => grid;

        /// <summary>
        /// Calculate map bounds based on all tilemaps in the level grid.
        /// </summary>
        public BoundsInt CalculateBounds()
        {
            var bounds = new BoundsInt();

            foreach (var tilemap in GetComponentsInChildren<Tilemap>())
            {
                tilemap.CompressBounds();

                if (tilemap.cellBounds.xMin < bounds.xMin) bounds.xMin = tilemap.cellBounds.xMin;
                if (tilemap.cellBounds.xMax > bounds.xMax) bounds.xMax = tilemap.cellBounds.xMax;
                if (tilemap.cellBounds.yMin < bounds.yMin) bounds.yMin = tilemap.cellBounds.yMin;
                if (tilemap.cellBounds.yMax > bounds.yMax) bounds.yMax = tilemap.cellBounds.yMax;
            }

            return bounds;
        }
    }
}