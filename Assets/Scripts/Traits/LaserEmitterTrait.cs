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

        private UniTraitEventBus eventBus;

        #region IUniTrait Members

        public void InjectEventBus(UniTraitEventBus bus)
        {
            eventBus = bus;
        }

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
            eventBus.Publish(new EmittedEvent { Points = emissionPoints });

            return emissionPoints;
        }

        #region Nested type: ${0}

        public struct EmittedEvent
        {
            public LaserBeamPoint[] Points;
        }

        #endregion
    }
}
