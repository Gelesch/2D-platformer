using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float walkDistance = 6f;
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float timeToWait = 5f;
    [SerializeField] private float timeToChase = 3f;
    [SerializeField] private float minDistanceToPlayer = 1.5f;
    [SerializeField] private float chasingSpeed = 3f;

    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private Vector2 _leftBoundaryPosition;
    private Vector2 _rightBoundfryPosotion;
    private Vector2 _nexPoint;

    private bool _isFacingRight = true;
    private bool _isWait = false; 
    public bool IsFacingRight => _isFacingRight;
    private bool _isCasingPlayer=false ;
    
    
    private float _walkSpeed;
    private float _waitTime;
    private float _chaseTime;
    
    public void StartChasingPlayer()
    {
        _isCasingPlayer = true;
        _chaseTime = timeToChase;
        _walkSpeed = chasingSpeed;
    }
    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rb = GetComponent<Rigidbody2D>();
        _leftBoundaryPosition = transform.position;
        _rightBoundfryPosotion = _leftBoundaryPosition + Vector2.right * walkDistance;
        _waitTime = timeToWait;
        _chaseTime = timeToChase;
        _walkSpeed = patrolSpeed;

    }

    private void Update()
    { 
        //когда враг переходит в режим приследовния вкл таймер,когда таймер истекает,враг переходит в режим потруля
        if (_isCasingPlayer)
        {
            StartChasingTimer();
        }
    // таймер разворачивания врага
    if (_isWait && !_isCasingPlayer)
    {
        StartWaitTimer();
    }
    if (ShouldWait())
    {
        _isWait = true;
    }
    }

    public bool ShouldWait()
    {
        bool _isOutOfRightBounadry = _isFacingRight && transform.position.x > _rightBoundfryPosotion.x;
        bool _isOutOfLeftBounadary = !_isFacingRight && transform.position.x < _leftBoundaryPosition.x;
        return _isOutOfRightBounadry || _isOutOfLeftBounadary;
    }

    private void FixedUpdate()
    {
        _nexPoint= Vector2.right * _walkSpeed * Time.fixedDeltaTime;


        if ( _isCasingPlayer &&  Math.Abs(DistanceToPlayer())<minDistanceToPlayer)
        {
            return;
        }
        
        //находится ли наш игрок в режиме прислеования
        if (_isCasingPlayer)
        {
           ChasePlayer();
        }
        //игнорирование потруля
        if (!_isWait && !_isCasingPlayer)
        {
           Patrol();
        }
     
      
    }

    private void Patrol()
    {
        if (!_isFacingRight)
        {
            _nexPoint.x *= -1;
        }
        _rb.MovePosition((Vector2)transform.position+_nexPoint);
        
    }
  // проверяем с какой стороны находися наш игрок и идем в ту сторону
  private void ChasePlayer()
  {
      float distance = DistanceToPlayer();
        if (distance<0)
        {
            _nexPoint.x *= -1;
        }

        if (distance>0.2f && !_isFacingRight)
        {
            Flip();
        }
        else if (distance<0.2f && _isFacingRight)
        {
            Flip();
        }
        
       _rb.MovePosition((Vector2)transform.position+_nexPoint);
    }

  private float DistanceToPlayer()
  {

      return _playerTransform.position.x - transform.position.x;
  }
    //Таймер
    private void StartWaitTimer()
    {
        if (_isWait)
        {
            _waitTime -= Time.deltaTime;
            if (_waitTime<0f)
            {
                _waitTime = timeToWait; 
                _isWait = false;
                Flip();
            }

        }
    }

    private void StartChasingTimer()
    {
        _chaseTime -= Time.deltaTime;

        if (_chaseTime<0f)
        {
            _isCasingPlayer = false;
            _chaseTime = timeToChase;
            _walkSpeed = patrolSpeed;
        }
    }
    private  void OnDrawGizmos()
    {   Gizmos.color=Color.red;
        Gizmos.DrawLine(_leftBoundaryPosition,_rightBoundfryPosotion);
    }
    void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }
}
    
