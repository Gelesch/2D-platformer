using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private GameObject currentHitObject;
   //радиус
    [SerializeField] private float circleRdius;
    //придел видимости противника
    [SerializeField] private float maxDistance;
    //слой LayerMasl
    [SerializeField] private LayerMask _layerMask;

    private EnemyController _enemyController;
    
    //точка,где будет распаложен наш противник
    private Vector2 _origin;
    
    //направление от точки origin до создания окружности
    private Vector2 _direction;

    //растояние от противника до объекта 
    private float _currentHitDistance;

 

 
    public void Start()
    {
        _enemyController = GetComponent<EnemyController>();
    }

    private void Update()
    {
        _origin = transform.position;

        if (_enemyController.IsFacingRight)
        {
            _direction=Vector2.right;
        }
        else
        {
            _direction=Vector2.left;
        }
        
        //создаем окружность от origin
        RaycastHit2D hit = Physics2D.CircleCast(_origin, circleRdius, _direction, maxDistance, _layerMask);

        if (hit)
        {
            currentHitObject = hit.transform.gameObject;
            _currentHitDistance = hit.distance;
            if (currentHitObject.CompareTag("Player"))
            {
                 _enemyController.StartChasingPlayer();
            }
            
        }
        else
        {
            currentHitObject = null;
            _currentHitDistance = maxDistance;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color=Color.cyan;
        Gizmos.DrawLine(_origin, _origin + _direction *_currentHitDistance);
        Gizmos.DrawWireSphere(_origin+_direction*_currentHitDistance,circleRdius);
        
    }
}