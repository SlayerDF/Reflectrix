using System;
using LaserBeam;
using UniTrait;
using UnityEngine;

namespace Reflectrix.Traits
{
    [Serializable]
    public class LaserEmitterTrait : IUniTrait
    {
        #region Serialized Fields

        [SerializeField]
        private LaserBeamPoint[] emissionPoints;

# if UNITY_EDITOR
        [SerializeField]
        private bool debugMode;
#endif

        #endregion

        #region IUniTrait Members

# if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            if (!debugMode)
            {
                return;
            }

            for (var i = 0; i < emissionPoints.Length; i++)
            {
                if (!emissionPoints[i].Initialized)
                {
                    continue;
                }

                Gizmos.DrawRay(emissionPoints[i].Origin, emissionPoints[i].Direction);
            }
        }
#endif

        #endregion

        public LaserBeamPoint[] Emit()
        {
            return emissionPoints;
        }
    }
}
