using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Image loadingImg;
    [SerializeField] private Transform cursorAnchor;
    [SerializeField] private TextMeshProUGUI helpGrab, helpInteract;
    
    /// <summary>
    /// Updates the loading circle near the visor to the given percentage
    /// </summary>
    /// <param name="percent">The percent</param>
    public void LoadingFill(float percent)
    {
        loadingImg.fillAmount = percent;
    }

    public void HelpGrab(bool value)
    {
        helpGrab.gameObject.SetActive(value);
    }

    public void HelpInteract(bool value)
    {
        HelpInteract(value, helpInteract.text);
    }
    
    public void HelpInteract(bool value, string text)
    {
        helpInteract.gameObject.SetActive(value);
        helpInteract.text = text;
    }

    /// <summary>
    /// Adjusts the cursor pointing towards the item held
    /// </summary>
    /// <param name="isVisible">Wether it is visible</param>
    /// <param name="angle">The angle, starting from top</param>
    public void AdjustCursor(bool isVisible, float angle)
    {
        cursorAnchor.gameObject.SetActive(isVisible);
        cursorAnchor.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
