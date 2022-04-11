using UnityEngine;


public class CubeColorController : MonoBehaviour
{

    [SerializeField] protected MeshRenderer _meshRenderer;
    [SerializeField] protected Color[] _colors;

    public void OnCubeInitialized(int number)
    {
        SetColor(ColorByNumber(number));
    }

    protected virtual void SetColor(Color color)
    {
        SetColorToMeshRenderer(color);
    }

    private void SetColorToMeshRenderer(Color color)
    {
        _meshRenderer.material.color = color;
    }

    private Color ColorByNumber(int number) => _colors[(Cube.NumberToLevel(number) - Cube.StartLevel) % _colors.Length];
    
}
