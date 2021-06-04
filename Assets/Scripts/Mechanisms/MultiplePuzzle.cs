using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class handling multiple interactions to solve a puzzle
/// </summary>
public class MultiplePuzzle : MonoBehaviour
{
    [SerializeField] private bool errorsReset = true;
    [SerializeField, Tooltip("Order matters, the key to unlock the puzzle")] private List<GameObject> puzzlePieces;
    [SerializeField] private UnityEvent onInteract = new UnityEvent();
    private int _currentNbr;

    /// <summary>
    /// Activates the puzzle, registering the transform as the last action, will validate the puzzle if a correct series of inputs is entered
    /// </summary>
    /// <param name="t">The piece of the puzzle being interacted with</param>
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