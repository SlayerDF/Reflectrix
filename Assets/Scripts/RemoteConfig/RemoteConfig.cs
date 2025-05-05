using System;

namespace Reflectrix.RemoteConfig
{
    [Serializable]
    public struct RemoteConfig
    {
        public int deviceQuantity;
        public bool rayIntersectionEnabled;
        public float zoomSpeed;
        public float panSpeed;
        public string winTitle;
        public string loseTitle;
    }
}
