using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Image loadingImg;
    [SerializeField] private Transform cursorAnchor;
    
    /// <summary>
    /// Updates the loading circle near the visor to the given percentage
    /// </summary>
    /// <param name="percent">The percent</param>
    public void LoadingFill(float percent)
    {
        loadingImg.fillAmount = percent;
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
