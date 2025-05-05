namespace Reflectrix.Assets.Scripts
{
    public struct TileState
    {
        public bool IsOccupied { get; private set; }

        public TileObjectType? TileObjectType { get; private set; }

        public void OccupyTile(TileObjectType tileObjectType)
        {
            IsOccupied = true;
            TileObjectType = tileObjectType;
        }
    }

    public enum TileObjectType
    {
        Obstacle,
        PredifinedObject,
        CustomObject,
    }
}