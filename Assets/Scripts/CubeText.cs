using UnityEngine;
using TMPro;

public class CubeText : MonoBehaviour
{
    [SerializeField] protected TextMeshPro[] _textMeshes;
    [SerializeField] protected string _text;
    public virtual string Text
    {
        get => _text;
        set 
        {
            _text = value;
            SetTextToTMPs(value);
        }
    }

    protected void SetTextToTMPs(string text)
    {
        foreach(var item in _textMeshes)
        {
            item.text = text;
        }
    }

    #if UNITY_EDITOR
    [ContextMenu("Apply Text")]
    private void ApplyText()
    {
        Text = Text;
    }
    #endif
}
