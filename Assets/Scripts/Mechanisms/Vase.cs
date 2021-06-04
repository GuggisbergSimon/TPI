using UnityEngine;

public class Vase : MonoBehaviour
{
    [SerializeField] private int score = 10;

    public void IncrementStatistics()
    {
        IncrementStatistics(score);
    }
    
    public void IncrementStatistics(int value)
    {
        GameManager.Instance.StatisticsManager.VasesPicked++;
        GameManager.Instance.StatisticsManager.Score += value;
    }
}
