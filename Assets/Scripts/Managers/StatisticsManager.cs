using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    public int VasesPicked { get; set; }
    public float DistanceWalked { get; set; }
    public float TimeSpent { get; set; }
    public int Score { get; set; }
    
    public int NbrJumps { get; set; }

    public void Save()
    {
        //todo implement
    }

    public int GetScore()
    {
        //return Mathf.CeilToInt(Score / (NbrJumps + 1));
        return Score;
    }

    public void Load()
    {
        //todo implement
    }
}
