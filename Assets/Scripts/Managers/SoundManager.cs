using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Manager handling the audio, and means to interact with it
/// </summary>
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer = default;

    /// <summary>
    /// Changes the exposed parameter to the given float value
    /// </summary>
    /// <param name="parameter">the exposed parameter, as a string</param>
    /// <param name="value">value the parameter will be changed to</param>
    public void SetFloat(string parameter, float value)
    {
        audioMixer.SetFloat(parameter, value);
    }

    public float GetFloat(string parameter)
    {
        bool result =  audioMixer.GetFloat(parameter, out float value);
        return result ? value : 0f;
    }
}