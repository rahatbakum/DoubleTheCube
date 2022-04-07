using UnityEngine;
using TMPro;

public class ResizingCubeText : CubeText
{
    [Min (0f)]
    [SerializeField] private float _maxFontSizeWithFitWidth = 13f;
    [Min (0f)]
    [SerializeField] private float _maxFontSizeWithFitHeight = 7f;
    public override string Text
    {
        get => base.Text;
        set 
        {
            SetFontSizeToTMPs(StringLengthToFontSize(value.Length));
            base.Text = value;
        }
    }

    private void SetFontSizeToTMPs(float fontSize)
    {
        foreach(var item in _textMeshes)
            item.fontSize = fontSize;
    }

    private float StringLengthToFontSize(int stringLength)
    {
        if(stringLength <= 0)
            return Mathf.Min(_maxFontSizeWithFitWidth, _maxFontSizeWithFitHeight);
        float fontAspectRatio = _maxFontSizeWithFitHeight / _maxFontSizeWithFitWidth;
        if(fontAspectRatio * stringLength < 1f) // if text height bigger than text width
            return _maxFontSizeWithFitHeight;
        return _maxFontSizeWithFitWidth / stringLength; 
    }
}
