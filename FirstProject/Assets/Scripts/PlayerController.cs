using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]  private float speedX = 1f;
    [SerializeField] private Animator animator;
    
    private const float speedXMultiplayer = 50f;
    private float _horizontal=0f;
    private bool _isGround = false;
    private bool _isJump = false;
    private bool _isFacingRight = true;
    private bool _isFinish = false;
    private Rigidbody2D _rg;
    private bool _isLevelArm;
    private LeverArm _leverArm;
   
    private Finish _finish;
    
    private  
    void Start()
    {
        _rg = GetComponent<Rigidbody2D>();
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
        _leverArm = FindObjectOfType<LeverArm>();
    }

    private void Update()
    {
         _horizontal=Input.GetAxis("Horizontal"); //-1 :1
         animator.SetFloat("speedX",Math.Abs(_horizontal ) );
         if (Input.GetKey(KeyCode.W) && _isGround)
         {
             _isJump = true;
         }

         if (Input.GetKey(KeyCode.F) )
         {
             if (_isFinish)
             {
                 _finish.FinishLevel();
             }

             if (_leverArm)
             {
                 _leverArm.ActivateLeverArm();
             }
         }
    }

    private void FixedUpdate()
    {
        _rg.velocity = new Vector2(_horizontal*speedX*speedXMultiplayer*Time.fixedDeltaTime, _rg.velocity.y);

        //когда можем прыгать
        if (_isJump)
        {
            _rg.AddForce(new Vector2(0f,500f));
            _isGround = false;
            _isJump = false;
        }

        if (_horizontal>0f&& !_isFacingRight)
        {
           Flip();
        }
        else if(_horizontal <0f && _isFacingRight)
        {
            Flip();
        }
        
        
    }

    
    
    //разворот игрока в разные стороны
    void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }
    
    
//Когда игрок касается любого Collider то срабатывает функция
    private void OnCollisionEnter2D(Collision2D other)
    {    
        //когда соприкосаемся с коладером земли
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
        
    }

    
    //используем Trigger чтобы геро мог проходить на сквозь  финиша 
    private void OnTriggerEnter2D(Collider2D other)
    {
        //являетя ли этот коллайдер levelArm,является ли рычагом
        LeverArm leverArm = other.GetComponent<LeverArm>();
        //когда соприкосаемся с коладером финиша
        if (other.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Worked");
            _isFinish = true;
        }

        if (leverArm!=null)
        {
            _isLevelArm = true;
        }
    }
    
    
    
    // метод срабатывает когда мы уходим из зоны финиша
    //используем Trigger чтобы геро мог проходить на сквозь  финиша 
    private void OnTriggerExit2D (Collider2D other)
    {
        LeverArm leverArmTemp = other.GetComponent<LeverArm>();
        
        if (other.CompareTag("Finish"))
        {
            Debug.Log("Not worked");
            _isFinish = false;
        }

        if (leverArmTemp!=null)
        {
            _isLevelArm = false;
        }
    }

    
   
}

   

