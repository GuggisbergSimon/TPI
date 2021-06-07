/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Manager handling various stats related to the game
 */

using System;
using UnityEngine;

/// <summary>
/// Manager handling various stats related to the game
/// </summary>
public class StatisticsManager : MonoBehaviour
{
    private int _vasesPicked;
    private AudioSource _audioSource;

    public int VasesPicked
    {
        get => _vasesPicked;
        set
        {
            _vasesPicked = value;
            if (value >= GameManager.Instance.LevelManager.NbrVases)
            {
                _audioSource.Play();
            }
        }
    }
    public float DistanceWalked { get; set; }
    public float TimeSpent { get; set; }
    public int Score { get; set; }
    public int NbrJumps { get; set; }


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Gets the score, can be calculated with a custom formula
    /// </summary>
    /// <returns>The score</returns>
    public int GetScore()
    {
        return Score;
    }

    /// <summary>
    /// Saves the current statistics and score in an external file
    /// </summary>
    public void Save()
    {
        //todo implement
    }

    /// <summary>
    /// Loads the current statistics and score in an external file
    /// </summary>
    public void Load()
    {
        //todo implement
    }
}