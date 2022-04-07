using UnityEngine;


public class CubeColorController : MonoBehaviour
{
    private const int MaxCubeLevel = 11;
    private const float DefaultColorS = 1f;
    private const float DefaultColorB = 0.75f;

    [SerializeField] protected MeshRenderer _meshRenderer;

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

    private Color ColorByNumber(int number)
    {
        int level = (int) Mathf.Log(number, Cube.NumberBase);
        float colorH = (float) ((level - Cube.StartLevel) % MaxCubeLevel) / MaxCubeLevel;
        return Color.HSVToRGB(colorH, DefaultColorS, DefaultColorB);
    }
}
