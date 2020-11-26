using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    //Components
    Rigidbody2D _enemyRB2D;
    CapsuleCollider2D _enemyCapsule2d;
    Animator _animator;
    internal EnemyMovement instance;

    // Config
    [SerializeField] float speed;
    [SerializeField] float leftX, rightX;
    [SerializeField] bool isFacingRight = true;
    bool isNotDead = true;


    //Action
     internal Action<int> DoDmg;

  
    void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyCapsule2d = GetComponent<CapsuleCollider2D>();
        _enemyRB2D = GetComponent<Rigidbody2D>();
        instance = this;
    }




    void Update()
    {
        if (isFacingRight)
        {
            _enemyRB2D.velocity = new Vector2(speed, 0);

            if (transform.position.x > rightX)
            {
                isFacingRight = false;
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            }
        }
        else
        {
            _enemyRB2D.velocity = new Vector2(-speed, 0);

            if (transform.position.x < leftX)
            {
                isFacingRight = true;
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }

        }

        if (isNotDead)
        {
            AttackPlayer();
            GotHitFromArrow();
        }

    }


    IEnumerator DestroyEnemyCorpse() {
        float timerTillDie=2f;
        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(timerTillDie);

        Destroy(gameObject);
    }

    internal void GotHitFromArrow()
    {
        float timeToRotate = 0.3f;
        float turnDegree = 90f;


        if (_enemyCapsule2d.IsTouchingLayers(LayerMask.GetMask("Arrow")))
        {
            _animator.SetTrigger("Dead");

            isNotDead = false;

            _enemyRB2D.constraints = RigidbodyConstraints2D.FreezePositionX;

            LeanTween.rotateZ(gameObject, transform.localScale.x * turnDegree, timeToRotate);

            StartCoroutine(DestroyEnemyCorpse());
        }
    }

    private void AttackPlayer() {
        if (_enemyCapsule2d.IsTouchingLayers(LayerMask.GetMask("Player")) )
        {
            Debug.Log("GotHit!");
            DoDmg(-1);
        }
    }
}
