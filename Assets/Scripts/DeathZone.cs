using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private float _maxCubeStayingTime = 1f;
    private List<StayingCube> _stayingCubes = new List<StayingCube>();

    private void OnTriggerStay(Collider other)
    {

        Cube cube = other.GetComponentInParent<Cube>();
        if(cube != null)
        {
            StayingCube stayingCube = _stayingCubes.Find((StayingCube stayingCube) => stayingCube.ThisCube == cube);
            if(stayingCube == null)
            {
                if(cube.IsActivated)
                {
                    _stayingCubes.Add(new StayingCube(cube, Time.time, OnCubeMerging));
                }
            }
            else if(Time.time >= stayingCube.StartStayingTime + _maxCubeStayingTime)
            {
                Lose();
            }
        }
    }    

    private void OnTriggerExit(Collider other)
    {

        Cube cube = other.GetComponentInParent<Cube>();
        if(cube != null)
        {
            StayingCube stayingCube = _stayingCubes.Find((StayingCube stayingCube) => stayingCube.ThisCube == cube);
            if(stayingCube != null)
            {
                _stayingCubes.Remove(stayingCube);
            }
        }
    }  

    private void OnCubeMerging(Cube cube)
    {
        for(int i = 0; i < _stayingCubes.Count; i++)
            if(_stayingCubes[i].ThisCube == null)
            {
                _stayingCubes.Add(new StayingCube(cube, _stayingCubes[i].StartStayingTime, OnCubeMerging));
                _stayingCubes.RemoveAt(i);
                break;
            }
    }

    private void Lose()
    {
        GameManager.Instance.Lose();
    }

    private class StayingCube
    {
        private Cube _thisCube;
        public Cube ThisCube => _thisCube;
        private float _startStayingTime;
        public float StartStayingTime => _startStayingTime;

        public StayingCube(Cube cube, float startStayingTime, UnityAction<Cube> onCubeMerging)
        {
            _thisCube = cube;
            _startStayingTime = startStayingTime;
            cube.Merging += onCubeMerging;
        }
    }
}