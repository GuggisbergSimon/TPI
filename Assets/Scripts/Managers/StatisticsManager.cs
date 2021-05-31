using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public int VasesPicked { get; set; }
    public float DistanceWalked { get; set; }
    public float TimeSpent { get; set; }
    
    public int Score { get; set; }

    public void Reset()
    {
        VasesPicked = 0;
        DistanceWalked = 0f;
        TimeSpent = 0f;
    }

    public void Save()
    {
        //todo implement
    }

    public void Load()
    {
        //todo implement
    }
}
