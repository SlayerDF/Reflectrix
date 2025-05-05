using UnityEngine;

namespace Reflectrix.Assets.Scripts
{
    public struct TileState
    {
        public bool IsOccupied { get; private set; }

        public TileObjectType TileObjectType { get; private set; }

        public GameObject GameObject { get; private set; }

        public void OccupyTile(TileObjectType tileObjectType, GameObject gameObject)
        {
            IsOccupied = true;
            TileObjectType = tileObjectType;
            GameObject = gameObject;
        }
    }

    public enum TileObjectType
    {
        None,
        Obstacle,
        PredifinedObject,
        Mirror,
        Splitter,
        Merger,
    }
}