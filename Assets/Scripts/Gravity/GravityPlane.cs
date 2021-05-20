//Code taken from catlikecoding

using UnityEngine;

namespace Gravity
{
    //todo restrict effect to size of the plane
    /// <summary>
    /// Gravity Source based on a plane shape, affecting all items between the y-position (up) of the plane and its range
    /// </summary>
    public class GravityPlane : GravitySource
    {
        [SerializeField] float gravity = 9.81f;
        [SerializeField, Min(0f)] float range = 1f;

        /// <summary>
        /// Returns the current gravity for a given position
        /// </summary>
        /// <param name="position">The position affected by gravity</param>
        /// <returns>The gravity</returns>
        public override Vector3 GetGravity(Vector3 position)
        {
            Vector3 up = transform.up;
            float upDistance = Vector3.Dot(up, position - transform.position);

            if (upDistance > range)
            {
                return Vector3.zero;
            }

            float g = -gravity;
            if (upDistance > 0f)
            {
                g *= 1f - upDistance / range;
            }

            return g * up;
        }

        private void OnDrawGizmos()
        {
            Vector3 scale = transform.localScale;
            scale.y = range;
            Gizmos.matrix =
                Matrix4x4.TRS(transform.position, transform.rotation, scale);
            Vector3 size = new Vector3(1f, 0f, 1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Vector3.zero, size);
            if (range > 0f)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(Vector3.up, size);
            }
        }
    }
}