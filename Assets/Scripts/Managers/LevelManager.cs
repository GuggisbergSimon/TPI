/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Manager handling various objects in the scene, saving and loading
 */

using UnityEngine;

/// <summary>
/// Manager handling various objects in the scene, saving and loading
/// </summary>
public class LevelManager : MonoBehaviour
{
    private PlayerController _player;
    public PlayerController Player => _player;
    public int NbrVases { get; private set; }

    private void Awake()
    {
        //Alternative way to find the player, based on tags
        //_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _player = FindObjectOfType<PlayerController>();
        NbrVases = FindObjectsOfType<PickUp>().Length;
    }

    /// <summary>
    /// Saves the position of all the items registered in an external file
    /// </summary>
    public void Save()
    {
        //todo implement
    }

    /// <summary>
    /// Loads the save located in an external file
    /// </summary>
    public void Load()
    {
        //todo implement
    }
}