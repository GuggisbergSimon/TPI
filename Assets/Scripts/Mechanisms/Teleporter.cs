using System;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Class teleporting an object based on conditions
/// </summary>
public class Teleporter : MonoBehaviour
{
    private enum TeleportType
    {
        Global,
        Local
    }
    
    [SerializeField] private TeleportType type = TeleportType.Global;
    [SerializeField] private Vector3[] localTeleports = default;
    [SerializeField] private Transform[] globalTeleports = default;
    [SerializeField] private bool sameRotation;
    
    /// <summary>
    /// Teleports a transform based on the parameters setup
    /// </summary>
    /// <param name="t">The transform to teleport</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Teleport(Transform t)
    {
        switch (type)
        {
            case TeleportType.Global:
                int i = Random.Range(0, globalTeleports.Length);
                t.transform.position = globalTeleports[i].position;
                if (!sameRotation)
                {
                    t.transform.rotation = globalTeleports[i].rotation;
                }
                break;
            case TeleportType.Local:
                t.transform.position += localTeleports[Random.Range(0, localTeleports.Length)];
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}