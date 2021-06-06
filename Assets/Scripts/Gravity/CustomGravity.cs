/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Class handling various gravity sources present in the scene
 */

//Code taken from catlikecoding
using System.Collections.Generic;
using UnityEngine;

namespace Gravity
{
    /// <summary>
    /// Class handling various gravity sources present in the scene
    /// </summary>
    public static class CustomGravity
    {
        private static List<GravitySource> _sources = new List<GravitySource>();

        /// <summary>
        /// Registers a new gravity source
        /// </summary>
        /// <param name="source">A gravity source</param>
        public static void Register(GravitySource source)
        {
            Debug.Assert(
                !_sources.Contains(source),
                "Duplicate registration of gravity source!", source
            );
            _sources.Add(source);
        }

        /// <summary>
        /// Unregisters a gravity source, if it exists
        /// </summary>
        /// <param name="source">A gravity source</param>
        public static void Unregister(GravitySource source)
        {
            Debug.Assert(
                _sources.Contains(source),
                "Unregistration of unknown gravity source!", source
            );
            _sources.Remove(source);
        }

        /// <summary>
        /// Returns the current gravity for a given position, based on the multiple gravity sources registered
        /// </summary>
        /// <param name="position">The position affected by gravity</param>
        /// <returns>The gravity</returns>
        public static Vector3 GetGravity(Vector3 position)
        {
            Vector3 g = Vector3.zero;
            for (int i = 0; i < _sources.Count; i++)
            {
                g += _sources[i].GetGravity(position);
            }

            return g;
        }

        /// <summary>
        /// Returns the current gravity for a given position, assigns an upAxis
        /// </summary>
        /// <param name="position">The position affected by gravity</param>
        /// <param name="upAxis">The y-axis (up) affected by the gravity</param>
        /// <returns>The gravity</returns>
        public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis)
        {
            Vector3 g = Vector3.zero;
            for (int i = 0; i < _sources.Count; i++)
            {
                g += _sources[i].GetGravity(position);
            }

            upAxis = -g.normalized;
            return g;
        }

        /// <summary>
        /// Returns the y-axis (up) for a given position affected by gravity
        /// </summary>
        /// <param name="position">The position affected by gravity</param>
        /// <returns>The y-axis (up)</returns>
        public static Vector3 GetUpAxis(Vector3 position)
        {
            Vector3 g = Vector3.zero;
            for (int i = 0; i < _sources.Count; i++)
            {
                g += _sources[i].GetGravity(position);
            }

            return -g.normalized;
        }
    }
}