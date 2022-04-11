using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTouchController : MonoBehaviour
{
    [SerializeField] private Shooter _shooter;
    [SerializeField] private Camera _camera;
    private Vector2 _startTouchPosition = Vector2.zero;

    public void OnTouchDown(Vector2 touchPosition)
    {
        _startTouchPosition = touchPosition;
        if(GameManager.Instance.CurrentGameState != GameState.Playing)
            return;
        _shooter.SaveAnchorPosition();
    }

    public void OnTouchHold(Vector2 touchPosition)
    {
        if(GameManager.Instance.CurrentGameState != GameState.Playing)
            return;
        Vector2 screenRightEdgePosition = _camera.WorldToScreenPoint(_shooter.RightEdgePosition);
        Vector2 screenLeftEdgePosition = _camera.WorldToScreenPoint(_shooter.LeftEdgePosition);
        float screenDistanceBetweenEdges = Vector2.Distance(screenLeftEdgePosition, screenRightEdgePosition);
        float relativeScreenXOffset = (touchPosition.x - _startTouchPosition.x) / screenDistanceBetweenEdges;
        _shooter.MoveGun(_shooter.XRange * relativeScreenXOffset);
    }

    public void OnTouchUp(Vector2 touchPosition)
    {
        if(GameManager.Instance.CurrentGameState != GameState.Playing)
            return;
        _shooter.Shoot();
    }
}
