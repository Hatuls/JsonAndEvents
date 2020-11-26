using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    internal CoinScript coinInstance;

    CircleCollider2D _MyCollider;

    internal Action GiveMoneyToThePlayerAct;
   void Awake()
    {
        coinInstance = this;
        _MyCollider = GetComponent<CircleCollider2D>();
     
    }


    private void Update()
    {
        if (_MyCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) { 

            GiveMoneyToThePlayerAct();

            StartCoroutine(TriggerCoinAqcuireAnim());

            PlayCoinAcuiredSound();

        }
    }

    private IEnumerator TriggerCoinAqcuireAnim()
    {

        float CoinAnimYValue = .8f; // POSITION
        float leanTweanTimer = .3f; // TIMER FOR POSITION
        float waitBetweenGoingUpAndDown = .5f; // TIMER BETWEEN THE LEANTWEENS
        float waitForTheAnimatorTofinishPlayTheAnim = 2f;  //TIMER FOR THE ANIMATION FROM THE ANIMATION CLIP

        _MyCollider.enabled = false;

        PlayCoinAcquiredAnim();

        LeanTween.moveY(gameObject, transform.position.y + CoinAnimYValue, leanTweanTimer);

        yield return new WaitForSeconds(waitBetweenGoingUpAndDown);

        LeanTween.moveY(gameObject, transform.position.y - CoinAnimYValue, leanTweanTimer);

        yield return new WaitForSeconds(waitForTheAnimatorTofinishPlayTheAnim);

        Destroy(gameObject);

    }

    private void PlayCoinAcuiredSound() => GetComponent<AudioSource>().Play(); // no need to store in a parameter if its only used once and no need for that component after that  

    private void PlayCoinAcquiredAnim () =>  GetComponent<Animator>().SetTrigger("Aqcuired");// no need to store in a parameter if its only used once and no need for that component after that  


}
