using System;
using System.Linq;
using Reflectrix.LaserBeam;
using UniTrait;
using UnityEngine;

namespace Reflectrix.Traits
{
    [Serializable]
    public class LaserEmitterTrait : IUniTrait
    {
        #region Serialized Fields

        [SerializeField]
        private LaserBeamPointInspector[] emissionPoints;

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

        public ILaserBeamPoint[] Emit()
        {
            var points = emissionPoints.Cast<ILaserBeamPoint>().ToArray();

            eventBus.Publish(new EmittedEvent { Points = points });

            return points;
        }

        #region Nested type: ${0}

        public struct EmittedEvent
        {
            public ILaserBeamPoint[] Points;
        }

        #endregion
    }
}
