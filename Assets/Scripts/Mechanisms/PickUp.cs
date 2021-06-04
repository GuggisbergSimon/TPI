using UnityEngine;

/// <summary>
/// Class for pickable items that gives some score
/// </summary>
public class PickUp : MonoBehaviour
{
    [SerializeField] private int score = 10;

    /// <summary>
    /// Picks the object up, adding some score to the statistics
    /// </summary>
    public void Pick()
    {
        Pick(score);
    }
    
    /// <summary>
    /// Picks the object up, adding some score to the statistics
    /// </summary>
    /// <param name="value">The score to be added</param>
    public void Pick(int value)
    {
        GameManager.Instance.StatisticsManager.VasesPicked++;
        GameManager.Instance.StatisticsManager.Score += value;
    }
}
