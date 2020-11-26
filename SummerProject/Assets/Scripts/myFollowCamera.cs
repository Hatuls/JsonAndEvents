using UnityEngine;

public class myFollowCamera : MonoBehaviour
{
    // adjusting the camera to follow the player and not exit the game itself
    [SerializeField] Transform Player;
    public Vector2 maxPosition;
    public Vector2 minPosition ;


    void Start()
    {
      
    }

 
    void FixedUpdate()
    {
        Vector3 PlayerPos = new Vector3(Player.position.x, Player.position.y,transform.position.z);
        if (transform.position != Player.position) { 

        

        PlayerPos.x = Mathf.Clamp(PlayerPos.x, minPosition.x, maxPosition.x);
        PlayerPos.y = Mathf.Clamp(PlayerPos.y, minPosition.y, maxPosition.y);


        transform.position = Vector3.Lerp(transform.position,PlayerPos , 2f);
        }
    }
}
