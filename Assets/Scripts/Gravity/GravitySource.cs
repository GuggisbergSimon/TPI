//Code taken from catlikecoding

using UnityEngine;

namespace Gravity
{
    /// <summary>
    /// Gravity Source based on Unity's default behaviour
    /// </summary>
    public class GravitySource : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual Vector3 GetGravity(Vector3 position)
        {
            return Physics.gravity;
        }

        private void OnEnable()
        {
            CustomGravity.Register(this);
        }

        private void OnDisable()
        {
            CustomGravity.Unregister(this);
        }
    }
}