using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Class moving the transform based on different parameters
/// </summary>
public class MovingRoutine : MonoBehaviour
{
    [SerializeField, Tooltip("Wether the routine is active at start")] private bool isActive = true;
    [SerializeField, Tooltip("A negative value means there is no limit.")] private int numberMoves = -1;
    [SerializeField] private MovingMode movingMode = 0;
    [SerializeField, Tooltip("Global coordinates")] private Point[] points = null;
    private Vector3 _initPos;
    private Quaternion _initRot;
    private int _indexPoints;
    private Coroutine _movingCoroutine;
    private bool _isAscending;
    private bool CanMove => numberMoves < 0 || numberMoves > 0;

    [Serializable]
    private struct Point
    {
        public Vector3 position;
        public Vector3 angle;
        public float speedToReach;
        public float timeToStop;
    }
    
    private enum MovingMode
    {
        Cycle,
        PingPong,
        Random
    }

    private void Start()
    {
        _initPos = transform.position;
        _initRot = transform.rotation;
        if (isActive)
        {
            Activate();
        }
    }

    /// <summary>
    /// Starts the movingCoroutine if it wasn't turned on
    /// </summary>
    public void Activate()
    {
        isActive = true;
        if (_movingCoroutine != null)
        {
            StopCoroutine(_movingCoroutine);
        }

        _movingCoroutine = StartCoroutine(Moving());
    }

    private IEnumerator Moving()
    {
        while (CanMove)
        {
            Vector3 initPos = transform.position;
            Quaternion initRot = transform.rotation;
            float timer = 0.0f;
            Point currentPoint = points[_indexPoints];
            float timeToReach = Vector3.Distance(initPos, _initPos + currentPoint.position) / currentPoint.speedToReach;
            while (timer < timeToReach)
            {
                timer += Time.deltaTime;
                transform.position = Vector3.Lerp(initPos, _initPos + currentPoint.position, timer / timeToReach);
                transform.rotation = Quaternion.Lerp(initRot, _initRot * Quaternion.Euler(currentPoint.angle),
                    timer / timeToReach);
                yield return null;
            }

            switch (movingMode)
            {
                case MovingMode.Cycle:
                    _indexPoints = (_indexPoints + 1) % points.Length;
                    break;
                case MovingMode.PingPong:
                    _indexPoints += _isAscending ? 1 : -1;
                    if (_indexPoints < 0)
                    {
                        _isAscending = true;
                    }
                    else if (_indexPoints >= points.Length)
                    {
                        _isAscending = false;
                    }
                    int range = points.Length - 1;
                    _indexPoints = Mathf.Abs((_indexPoints + range) % (range * 2) - range);
                    break;
                case MovingMode.Random:
                    _indexPoints = Random.Range(0, points.Length);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return new WaitForSeconds(currentPoint.timeToStop);

            if (numberMoves > 0)
            {
                numberMoves--;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!isActiveAndEnabled)
        {
            return;
        }
        //todo update gizmos positions if modifying in runtime
        if (Application.isEditor && !Application.isPlaying)
        {
            _initPos = transform.position;
            _initRot = transform.rotation;
        }
        if (points == null) return;
        int i = 0;
        foreach (var point in points)
        {
            Vector3 pos = _initPos + point.position;
            Quaternion angle = _initRot * Quaternion.Euler(point.angle);
            Gizmos.color = i == _indexPoints ? Color.magenta : Color.white;
            Gizmos.DrawCube(pos, Vector3.one * 0.2f);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos, pos + angle * Vector3.right);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pos, pos + angle * Vector3.up);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(pos, pos + angle * Vector3.forward);
            i++;
        }
    }
}