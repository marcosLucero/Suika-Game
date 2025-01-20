using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private BoxCollider2D _boundaries;
    [SerializeField] private Transform _fruitThrowTransform;

    private Bounds _bounds;

    private float _leftBound;
    private float _rightBound;

    private float _startingLeftBound;
    private float _startingRightBound;

    private float _offset;

    private void Awake()
    {
        _bounds = _boundaries.bounds;

        _offset = transform.position.x - _fruitThrowTransform.position.x;

        _leftBound = _bounds.min.x + _offset;
        _rightBound = _bounds.max.x + _offset;

        _startingLeftBound = _leftBound;
        _startingRightBound = _rightBound;
    }
}
