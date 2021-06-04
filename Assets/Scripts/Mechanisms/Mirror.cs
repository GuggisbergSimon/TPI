using UnityEngine;

/// <summary>
/// Class projecting what a camera sees to a texture, on a quad ideally to serve as a mirror, does not respect perspective
/// </summary>
public class Mirror : MonoBehaviour
{
    [SerializeField] private Vector2Int textureSize = new Vector2Int(512, 512);
    private MeshRenderer _renderer;
    private Camera _cam;
    private RenderTexture _renderTexture;

    private void Reset()
    {
        _renderer = GetComponent<MeshRenderer>();
        _cam = GetComponentInChildren<Camera>();
    }

    private void Awake()
    {
        Reset();
    }

    private void Start()
    {
        int depth = 16;
        if (!_renderTexture)
        {
            _renderTexture = new RenderTexture(textureSize.x, textureSize.y, depth);
            _renderTexture.Create();
        }

        _cam.targetTexture = _renderTexture;
        _renderer.material.mainTexture = _renderTexture;
    }
    
    private void OnDisable()
    {
        if (_renderTexture)
        {
            _renderTexture.DiscardContents();
            _renderTexture.Release();
        }
    }
}