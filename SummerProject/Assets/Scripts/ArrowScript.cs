using System.Collections;
using UnityEngine;


public class ArrowScript : MonoBehaviour
{

    // the direction the arrow going to
    Vector2 arrowDir;

    // raycast
    RaycastHit2D RayHit;
    
    // components
    Rigidbody2D _RB;
    [SerializeField] BoxCollider2D _ArrowBodyCollider;
    
    internal void Init(Vector2 _StartingPos, float direction)
    {
        float arrowSpawnPosAdjustment = 0.2f;
        float angleUp = 2.5f;
        float ThrowForce = 50f;
       

        transform.localScale =new Vector3( direction,1,1);
        transform.position = new Vector2(_StartingPos.x , _StartingPos.y -arrowSpawnPosAdjustment);
        transform.rotation = Quaternion.Euler(0, 0, direction * angleUp);
        
        _RB = GetComponent<Rigidbody2D>();

        _RB.AddForce(new Vector2(ThrowForce  * direction , angleUp), ForceMode2D.Impulse);

        StartCoroutine(TimeBeforeDisapear());
    }
    private void Update()
    {
        

       arrowDir = (Vector2.right * transform.localScale.x).normalized;
        RayHit = Physics2D.Raycast(transform.position, arrowDir,0.2f);
        
        if (RayHit.collider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            Debug.Log("Enemy Hit");
            _RB.velocity = Vector2.zero;
        
        }
    }


    

    IEnumerator TimeBeforeDisapear() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
