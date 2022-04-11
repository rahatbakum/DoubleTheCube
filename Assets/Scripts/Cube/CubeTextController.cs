using UnityEngine;
using TMPro;

public class CubeTextController : MonoBehaviour
{
    [SerializeField] protected TextMeshPro[] _textMeshes;

    public void OnCubeInitialized(int number)
    {
        SetText(number.ToString());
    }

    protected virtual void SetText(string text)
    {
        SetTextToTMPs(text);
    }

    private void SetTextToTMPs(string text)
    {
        foreach(var item in _textMeshes)
        {
            item.text = text;
        }
    }
}
