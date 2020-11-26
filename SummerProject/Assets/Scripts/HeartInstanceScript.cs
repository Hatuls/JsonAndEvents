using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartInstanceScript : MonoBehaviour
{
    // Start is called before the first frame update

    public void Init(bool isGoodHeart,Vector3 playerPos)
    {
        transform.position = playerPos ;
        if (!isGoodHeart)
            GetComponent<SpriteRenderer>().color = Color.black;

        StartCoroutine(AnimationTillDisapear());
    }

    IEnumerator AnimationTillDisapear()
    {
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
