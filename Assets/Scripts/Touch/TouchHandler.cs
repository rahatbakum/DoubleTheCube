using UnityEngine;
using UnityEngine.Events;

public class TouchHandler : MonoBehaviour
{
    private const int LeftMouseButton = 0;
    private const int MiddleMouseButton = 2;
    private const int RightMouseButton = 1;

    [SerializeField] private UnityEvent<Vector2> _touchDown;
    public event UnityAction<Vector2> TouchDown
    {
        add => _touchDown.AddListener(value);
        remove => _touchDown.RemoveListener(value);
    }
    [SerializeField] private UnityEvent<Vector2> _touchHold;
    public event UnityAction<Vector2> TouchHold
    {
        add => _touchHold.AddListener(value);
        remove => _touchHold.RemoveListener(value);
    }
    [SerializeField] private UnityEvent<Vector2> _touchUp;
    public event UnityAction<Vector2> TouchUp 
    {
        add => _touchUp.AddListener(value);
        remove => _touchUp.RemoveListener(value);
    }
    private bool _isPreviousFrameTouchPressed = false;
    private Vector2 _previousFrameTouchPosition = Vector2.zero;

    private void Update()
    {
        #if UNITY_EDITOR_WIN
        
        CheckClick();

        #else

        CheckTouch();

        #endif
    }

    private void CheckTouch()
    {
        if (Input.touchCount > 0)
        {
            Vector2 currentFrameTouchPosition = Input.GetTouch(0).position;
            if(_isPreviousFrameTouchPressed)
                _touchHold.Invoke(currentFrameTouchPosition);
            else
                _touchDown.Invoke(currentFrameTouchPosition);
            _previousFrameTouchPosition = currentFrameTouchPosition;
            _isPreviousFrameTouchPressed = true;
        }
        else 
        {
            if(_isPreviousFrameTouchPressed)
                _touchUp.Invoke(_previousFrameTouchPosition);
            _isPreviousFrameTouchPressed = false;
        }
    }

    private void CheckClick()
    {
        if (Input.GetMouseButton(LeftMouseButton))
        {
            Vector2 currentFrameTouchPosition = Input.mousePosition;
            if(_isPreviousFrameTouchPressed)
                _touchHold.Invoke(currentFrameTouchPosition);
            else
                _touchDown.Invoke(currentFrameTouchPosition);
            _previousFrameTouchPosition = currentFrameTouchPosition;
            _isPreviousFrameTouchPressed = true;
        }
        else 
        {
            if(_isPreviousFrameTouchPressed)
                _touchUp.Invoke(_previousFrameTouchPosition);
            _isPreviousFrameTouchPressed = false;
        }
    }
}
