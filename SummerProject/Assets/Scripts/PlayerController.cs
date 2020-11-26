using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //configs
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] const float climbSpeed = 5f;
    float myGravityAtStart;



    //states
    bool isAlive = true;
    bool isRunning = false;
    bool isClimbing = false;
    bool unAbleToMove = false;
    public bool HaveAmmo = false;

    //Cached components
    AudioSource _playerAudioSource;
    PlayerGFX playerGFX;
    Rigidbody2D _rb2d;
    CapsuleCollider2D _myBodyCol;
    BoxCollider2D _myFeetCol;

 

    // instance
    internal static PlayerController playerInstance;

    //Actions 
    internal Action KillPlayerAct;
    internal Action GainCoinPickUpAct;
    internal Action<int> GainArrowPickUpAct;
    internal Action KillEnemy;


    // Prefabs
    [SerializeField] ArrowScript ArrowPreFab;
    [SerializeField] HeartInstanceScript HeartPrefab;
    // getter setter:

    internal bool SetIsAlive
    {
        get { return isAlive; }
        set {
            isAlive = value;
            if (!isAlive)
            {
                _rb2d.velocity = Vector2.zero;
                PlayDeadAnim();

            }


        }

    }
    private bool SetisRunning
    {

        set
        {
            isRunning = value;
            playerGFX.PlayRunAnimation(isRunning);

            if (_rb2d.velocity.x > 0)
                transform.localScale = Vector2.one;

            if (_rb2d.velocity.x < 0)
                transform.localScale = new Vector2(-1, 1);

        }

        get { return isRunning; }
    }

    private bool SetIsClimbing
    {

        set
        {
            isClimbing = value;


            if (!isClimbing)
                _rb2d.gravityScale = myGravityAtStart;// return gravity to normal

            else
                _rb2d.gravityScale = 0f; // no gravity


            playerGFX.PlayClimbAnimation(isClimbing);
        }
    }

    internal bool SetIsAbleToMove
    {
        get { return unAbleToMove; }
        set
        {
            unAbleToMove = value;


        }

    }


    // Start is called before the first frame update
    void Awake()
    {
        Init();

    }

    public void Init()
    {
        playerInstance = this;
        playerGFX = GetComponentInChildren<PlayerGFX>();
        _myFeetCol = GetComponent<BoxCollider2D>();
        _myBodyCol = GetComponent<CapsuleCollider2D>();
        _rb2d = GetComponent<Rigidbody2D>();
        _playerAudioSource = GetComponent<AudioSource>();
        myGravityAtStart = _rb2d.gravityScale;
        playerGFX.DeployArrow = DeployArrow;
    }


    // Update is called once per frame
    void Update()
    {
        if (isAlive && !SetIsAbleToMove)
        {
            OnPressedControls();
            CollideWithHazzards();
            OnPlayerShootArrow();
            OnPlayerPickUpMyOwnArrow();
            OnPlayerPickUpArrowAmmo();
        }
    }


    //Controls
    private void OnPressedControls()
    {
        OnPlayerClimb(); // on pressing up/down/w/s on ladder
        OnPlayerJump();// on pressing space
        OnPlayerMoveHorizontal();// pressing left/right/a/d keys
    }
    private void OnPlayerMoveHorizontal()
    {

        float horizontalMovement = Input.GetAxis("Horizontal");

        Vector2 playerVelocity = new Vector2(horizontalMovement * movementSpeed, _rb2d.velocity.y);

        _rb2d.velocity = playerVelocity;

        SetisRunning = Mathf.Abs(_rb2d.velocity.x) > 0;
    }
    private void OnPlayerJump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && (IsOnGround() || IsOnLadder()))
        {
            SetIsClimbing = false;

            Debug.Log("Jump");

            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpForce);

            _rb2d.velocity += jumpVelocityToAdd;

        }
    }
    private void OnPlayerShootArrow()
    {
        if ((Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)) && IsOnGround() && HaveAmmo)
        {
            GainArrowPickUpAct(-1);
            StartCoroutine(ArrowCoolDowncoroutine());
            print("READY!");
        }
    }



    // functtions bow
    internal void PlayerIsOutOfAmmo(bool _hasAmmo) => HaveAmmo = _hasAmmo;
    private void DeployArrow()
    {
        var arrowcache = Instantiate(ArrowPreFab);
        arrowcache.Init(transform.position, transform.localScale.x);
     
    }
    private void OnPlayerPickUpMyOwnArrow() {
        if (_myBodyCol.IsTouchingLayers(LayerMask.GetMask("Arrow")))
        {
            GainArrowPickUpAct(1);
        }

    }
    private void OnPlayerPickUpArrowAmmo()
    {
        if (_myBodyCol.IsTouchingLayers(LayerMask.GetMask("PickUps")))
        {
            GainArrowPickUpAct(3);
            Debug.Log("HAHA GOT 3 arrows");
        }

    }

    //instanciate heartabove player

    public void CreatHeart(bool DidHeGainlife)
    {
        var heart = Instantiate(HeartPrefab, gameObject.transform);

        heart.Init(DidHeGainlife,transform.position);
    }


    // Collisions with objects and platforms
    private bool IsOnGround() => _myFeetCol.IsTouchingLayers(LayerMask.GetMask("ForeGround"));
    private bool IsOnLadder() => _myBodyCol.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    private bool IsCollideWithHazzard() => _myBodyCol.IsTouchingLayers(LayerMask.GetMask("Hazzards"));
    private void CollideWithHazzards()
    {
        if (IsCollideWithHazzard())
            
        KillPlayerAct();
    }
    private void OnPlayerClimb()
    {

        if (!IsOnLadder())  // if he's not on the ladder
        {
            SetIsClimbing = false; // stop the climbing animation and set isclimbing to false
            return;
        }
        else if (!IsOnGround()) // if he is colliding with the ladder but also not with the ground
            SetIsClimbing = true; // set climbing boolean and animation to true



        Vector2 climbVelocity = new Vector2(_rb2d.velocity.x, Input.GetAxis("Vertical") * climbSpeed);// calculate y velocity
        _rb2d.velocity = climbVelocity; // apply y velocity
        playerGFX.ClimbingVelocity(Mathf.Abs(_rb2d.velocity.y));
    }


    // play Animation && Coroutine
    internal void PlayImmuneAnim() => StartCoroutine(playerGFX.GotHitSemiAnimation());
    internal void PlayDeadAnim() { 

        //stop the player moving from previous velocity's
        _rb2d.velocity = Vector2.zero;

        //play the dead animation
        playerGFX.PlayDeadAnimation();
    }

     IEnumerator ArrowCoolDowncoroutine()
    {
        //play the animation
        playerGFX.PlayShootAnimation();

        // make the player un Able To Move
        SetIsAbleToMove = true;
        
        //stop previous movement
        _rb2d.velocity = Vector2.zero;


        //wait one second
        yield return new WaitForSeconds(1f);


        // restore the ability to move
        SetIsAbleToMove = false;
    }


    // sound
    internal void PlayDeathSound() => _playerAudioSource.Play();




}

