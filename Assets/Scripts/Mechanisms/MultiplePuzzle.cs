using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiplePuzzle : MonoBehaviour
{
    [SerializeField] private bool errorsReset = true;
    [SerializeField] private List<GameObject> puzzlePieces;
    [SerializeField] private UnityEvent onInteract = new UnityEvent();
    private int _currentNbr;

    public void Activate(Transform t)
    {
        if (_currentNbr < 0)
        {
            return;
        }
        if (puzzlePieces[_currentNbr].GetInstanceID() == t.gameObject.GetInstanceID())
        {
            _currentNbr++;
        }
        else if (errorsReset && puzzlePieces[0].GetInstanceID() == t.gameObject.GetInstanceID())
        {
            _currentNbr = 1;
        }
        else if (errorsReset)
        {
            _currentNbr = 0;
        }

        if (_currentNbr >= puzzlePieces.Count)
        {
            _currentNbr = -1;
            onInteract.Invoke();
        }
    }
}