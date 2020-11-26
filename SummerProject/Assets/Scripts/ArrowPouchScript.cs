using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPouchScript : MonoBehaviour
{
    
    ArrowPouchScript instance;
    BoxCollider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (_collider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            _collider.enabled = false;
            StartCoroutine(DisapearAnimation());


        }
    }


    IEnumerator DisapearAnimation()
    {
        GetComponent<AudioSource>().Play();
        float distanceUpDown = .8f;
        float timeForAnim = .3f; 
        LeanTween.alpha(gameObject, 0, distanceUpDown);
        LeanTween.moveY(gameObject, transform.position.y + distanceUpDown, timeForAnim);
        yield return new WaitForSeconds(timeForAnim);
        LeanTween.moveY(gameObject, transform.position.y - distanceUpDown, timeForAnim);
        yield return new WaitForSeconds(timeForAnim);
        Destroy(gameObject);

    }
}
