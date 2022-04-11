using UnityEngine;

public class AdPageShower : MonoBehaviour
{
    [SerializeField] private AdPage _adPage;
    [SerializeField] private Shooter _shooter;
    [Min (1)]
    [SerializeField] private int _minShootCount = 10;
    [Min (1)]
    [SerializeField] private int _maxShootCount = 20;

    private int _shootCount = 0;
    private int _targetShootCount = 0;

    private void Awake()
    {
        UpdateTargetShootCount();
    }

    private void OnEnable()
    {
        _shooter.Shot += OnShot;
    } 
     
    private void OnDisable()
    {
        _shooter.Shot -= OnShot;
    }

    private void UpdateTargetShootCount()
    {
        _targetShootCount = _shootCount + Random.Range(_minShootCount, _maxShootCount);
    }

    private void OnShot(Cube cube)
    {
        _shootCount++;
        if(_shootCount >= _targetShootCount)
        {
            _adPage.Show();
            UpdateTargetShootCount();
        }
    }
}
