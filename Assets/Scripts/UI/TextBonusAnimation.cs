using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class TextBonusAnimation : MonoBehaviour
{
    private const float AngleRange = 90f;
    private const float MinAlpha = 0f;
    private const float MaxAlpha = 1f;

    [SerializeField] private Text _text;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private AnimationCurve _moveAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve _alphaAnimationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private Transform _mainTransform;
    [Min(MathHelper.MinNotZeroNumber)]
    [SerializeField] private float _animationTime = 1.25f;
    [Min(MathHelper.MinNotZeroNumber)]
    [SerializeField] private float _animationDistance = 1f;
    

    public void InitializeAndPlay(int bonus)
    {
        Initialize(bonus);
        Play();
    }

    public void Initialize(int bonus)
    {
        _text.text = "+" +bonus.ToString();
    }

    public void Play()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector3 startPosition = transform.position;
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(-MathHelper.Half(AngleRange), MathHelper.Half(AngleRange)));
        Vector3 finishPosition = startPosition + rotation * Vector3.up * _animationDistance;
        float startTime = Time.time;
        while(Time.time < startTime + _animationTime)
        {
            float timeCoef = (Time.time - startTime) / _animationTime;
            _mainTransform.position = Vector3.Lerp(startPosition, finishPosition, _moveAnimationCurve.Evaluate(timeCoef));
            _canvasGroup.alpha = Mathf.Lerp(MaxAlpha, MinAlpha, _alphaAnimationCurve.Evaluate(timeCoef));
            yield return null;
        }

        Destroy(_mainTransform.gameObject);
    }
}
