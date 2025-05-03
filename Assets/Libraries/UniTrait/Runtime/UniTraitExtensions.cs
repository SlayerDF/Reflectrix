using UnityEngine;

namespace UniTrait
{
    public static class UniTraitExtensions
    {
        public static bool TryGetTrait<T>(this GameObject go, out T trait) where T : class, IUniTrait
        {
            trait = null;

            if (!go.TryGetComponent(out UniTraitContainer container))
            {
                return false;
            }

            trait = container.GetTrait<T>();
            return trait != null;
        }

        public static bool TryGetTrait<T>(this Component comp, out T trait) where T : class, IUniTrait
        {
            return comp.gameObject.TryGetTrait(out trait);
        }

        public static bool TryGetTrait<T>(this Collider col, out T trait) where T : class, IUniTrait
        {
            return col.gameObject.TryGetTrait(out trait);
        }

        public static bool TryGetTrait<T>(this Collision col, out T trait) where T : class, IUniTrait
        {
            return col.gameObject.TryGetTrait(out trait);
        }

        public static bool TryGetTrait<T>(this Collider2D col, out T trait) where T : class, IUniTrait
        {
            return col.gameObject.TryGetTrait(out trait);
        }

        public static bool TryGetTrait<T>(this Collision2D col, out T trait) where T : class, IUniTrait
        {
            return col.gameObject.TryGetTrait(out trait);
        }
    }
}
