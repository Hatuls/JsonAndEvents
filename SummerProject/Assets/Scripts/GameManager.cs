
using System;
using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    float timeTillAnimationIsFinished = 3f;

    int numGameManager;
    [SerializeField] int playerCoins;
    [SerializeField] int playerArrows;
    [SerializeField] int playerLives;

    public bool isDMGProof = false;
  
    //Cached components
    PlayerController _playerInstance;
  [SerializeField]  InGameUi _playerDisplayUI;

    SaveProgress save1 = new SaveProgress();

    UnityEvent unityEvent;

    // Getters Setters
    int SetPlayerHealth
    {
        get { return playerLives; }
        set
        {
            
            playerLives = value;

            if (playerLives <= 0)
            {
                _playerInstance.SetIsAlive = false;
                StartCoroutine(WaitForResetGameSessionToRestartGame());
            }
            if (playerLives > 5)
            {
                playerLives = 5;
            }
          
            UpdateUI();
        }


    }
    int SetPlayerCoins
    {
        get { return playerCoins; }
        set
        {
            playerCoins = value;

            if (playerCoins >= 10)
            {
                playerCoins -= 10;
                SetPlayerHealth++;
                _playerInstance.CreatHeart(true);
            }

            UpdateUI();
        }
    }

    int SetPlayerAmmo
    {
        get
        {
            return playerArrows;
        }
        set
        {
            playerArrows = value;
            _playerInstance.PlayerIsOutOfAmmo(playerArrows > 0);
        }
    }


    //Awake
    private void Awake()
    {
     
        numGameManager = FindObjectsOfType<GameManager>().Length;
        if (numGameManager > 1)
        {
            Destroy(gameObject);
            print("ManagerDestroyed");
            return;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
 
    }

    //Start
    private void Start()
    {
        InitLevelOnLoad();
        SetPlayerAmmo = 0;
        if (unityEvent == null)
        {
            unityEvent = new UnityEvent();
        }

    }
    
     //Load every scene
    private void OnLevelWasLoaded(int level)
    {
InitLevelOnLoad();
        
    }

    // Init level
    private void InitLevelOnLoad()
    {
        // caches containers
        Transform EnemyContainer = null;
        Transform SpikesContainer = null;
        CoinScript[] coins = null;


        EnemyMovement enemyScript=null;


        
        // reseting the value's
        _playerInstance = null;
        EnemyContainer = null;
        EnemyContainer = null;
        coins = null;

        //Finding the right components and asign them with the right pointer
        // then asign action events



        //collisions with normal spikes kill the player
        _playerInstance = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _playerInstance.KillPlayerAct += InstaKillPlayer;
        _playerInstance.KillPlayerAct += _playerInstance.PlayDeathSound;
        _playerInstance.GainArrowPickUpAct = PlayerArrow;
        _playerInstance.PlayerIsOutOfAmmo(playerArrows > 0);

        //Enemies Actions
        EnemyContainer = GameObject.FindGameObjectWithTag("EnemyContainer").transform;

        for (int i = 0; i < EnemyContainer.transform.childCount; i++)
        {
            enemyScript = EnemyContainer.GetChild(i).GetComponent<EnemyMovement>();
            enemyScript.instance.DoDmg += ApplyLives;
           

        }

        FallingSpikePlat fallingSpikePlatScript = null ;
        // Moving platform with spikes Actions
        SpikesContainer = GameObject.FindGameObjectWithTag("SpikesContainer").transform;
        for (int i = 0; i < SpikesContainer.childCount; i++)
        {
            fallingSpikePlatScript = SpikesContainer.GetChild(i).GetComponent<FallingSpikePlat>();
            fallingSpikePlatScript.instance.CollideWithSpike += InstaKillPlayer;
            fallingSpikePlatScript.instance.CollideWithSpike += _playerInstance.PlayDeathSound;
        }

        // asign coin action
        coins = FindObjectsOfType<CoinScript>();
        foreach (CoinScript cs in coins)
        {
            Debug.Log(cs.name);
            cs.coinInstance.GiveMoneyToThePlayerAct = AddCoinToBank;

        }


      

        UpdateUI();
    }


    //Player's lives
    internal void InstaKillPlayer() { SetPlayerHealth -= playerLives;
        _playerInstance.CreatHeart(false);
    }
    internal void ApplyLives(int oneHealth)
    {
        //when called check if the player is alive and didnt recieve multiple dmg from different sources
        // then check if the player has more than 1 health left and that the health he recieved is minus and not gained health
        // if it true start corutine for the visability on/off and adjust isdmgproof for that duration to prevent multiple dmgs
        //add the the health he recieved to the player health

        if (SetPlayerHealth >= 0 && !_playerInstance.SetIsAbleToMove)
        {
            _playerInstance.CreatHeart(oneHealth>0);
            if (oneHealth < 0 && playerLives > 1)
                StartCoroutine(WaitForResetCurrentGameSessionLevel());

            if (oneHealth <0)
            {
                _playerInstance.PlayDeathSound();
            }

            SetPlayerHealth += oneHealth;
        }


    }



    // Player's PickUp System
    private void AddCoinToBank()
    {
        SetPlayerCoins++;
    }

    private void PlayerArrow(int _amountOfArrows)
    {
        print("I Recieved Ammo");
        SetPlayerAmmo += _amountOfArrows;
        UpdateUI();
    }



    // UI in Game
    private void UpdateUI() => _playerDisplayUI.UpdateUI(SetPlayerHealth, SetPlayerCoins, SetPlayerAmmo);





    //Coroutines

    IEnumerator WaitForResetGameSessionToRestartGame()
    {
        // reset game to the first level


        yield return new WaitForSeconds(timeTillAnimationIsFinished);
        MainMenuManager.HeDied = true;
        SceneManager.LoadScene(0);



        Destroy(gameObject);

    }
    IEnumerator WaitForResetCurrentGameSessionLevel()
    {
        // reset game to the current level

        _playerInstance.SetIsAbleToMove = true; // apply to not recieve any further dmg

        _playerInstance.PlayImmuneAnim(); // play immune anim 

        _playerInstance.PlayDeadAnim(); // play death anim

        yield return new WaitForSeconds(timeTillAnimationIsFinished); // wait for the animation to finish

        var currentScene = SceneManager.GetActiveScene().buildIndex;  // get the current scene the player at

        _playerInstance.SetIsAbleToMove = false; // set the immune to false

        SceneManager.LoadScene(currentScene); // load the current scene
        
    
        

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(SaveGame);
            unityEvent.Invoke();
            Debug.Log("Action Accured: Game Saved");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            unityEvent.RemoveAllListeners();
            unityEvent.AddListener(LoadGame);
            unityEvent.AddListener(UpdateUI);
            unityEvent.Invoke();
            Debug.Log("Action Accured: Game Loaded");
        }
    }


    public void SaveGame() {

        save1.arrowSaved = playerArrows;
        save1.coinSaved = playerCoins;
        save1.lifeSaved = playerLives;
        save1.currentSavePos = _playerInstance.transform.position;
        save1.scene = SceneManager.GetActiveScene();

        string json = JsonUtility.ToJson(save1);
        File.WriteAllText("Save.txt", json);
    }

    public void LoadGame() {
       
        string readfile = File.ReadAllText("Save.txt");
         save1   = JsonUtility.FromJson<SaveProgress>(readfile);
        Debug.Log(save1.arrowSaved);
        playerArrows= save1.arrowSaved  ;
       playerCoins = save1.coinSaved  ;
         playerLives= save1.lifeSaved;
       _playerInstance.transform.position  =   save1.currentSavePos;
        SceneManager.SetActiveScene( save1.scene);
    }
}

[Serializable]
public class SaveProgress {


    public Vector2 currentSavePos;
    public int coinSaved;
    public int arrowSaved;
    public int lifeSaved;
    public Scene scene;

}