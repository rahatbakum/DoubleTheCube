using UnityEngine;
using UnityEngine.Events;

public class TouchHandler : MonoBehaviour
{
    [SerializeField] private UnityEvent _touchDown;
    public event UnityAction TouchDown
    {
        add => _touchDown.AddListener(value);
        remove => _touchDown.RemoveListener(value);
    }
    [SerializeField] private UnityEvent _touchHold;
    public event UnityAction TouchHold
    {
        add => _touchHold.AddListener(value);
        remove => _touchHold.RemoveListener(value);
    }
    [SerializeField] private UnityEvent _touchUp;
    public event UnityAction TouchUp 
    {
        add => _touchUp.AddListener(value);
        remove => _touchUp.RemoveListener(value);
    }

    private void Update()
    {
        
    }
}
