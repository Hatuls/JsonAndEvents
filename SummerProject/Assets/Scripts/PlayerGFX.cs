using System;
using System.Collections;
using UnityEngine;

public class PlayerGFX : MonoBehaviour
{
    Animator _playerAnimator;
    SpriteRenderer _sr;


    internal Action DeployArrow;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
        
    }


    // coroutines
    internal IEnumerator GotHitSemiAnimation()
    {
        float totalTime = 2f;
        float time = Time.time;
        while (Time.time - time < totalTime)
        {
            _sr.enabled = !_sr.enabled;

            yield return new WaitForSeconds(0.2f);
        }
    }



    //animations

    internal void ClimbingVelocity(float _climbingVelocity) => _playerAnimator.SetFloat("ClimbingVelocity", _climbingVelocity);
    internal void PlayRunAnimation(bool _isRunning) => _playerAnimator.SetBool("isRunning", _isRunning);
    internal void PlayClimbAnimation(bool _isClimbing) => _playerAnimator.SetBool("IsClimbing", _isClimbing);
    internal void PlayDeadAnimation() => _playerAnimator.SetTrigger("Dead");

    internal void PlayShootAnimation() { 
       _playerAnimator.SetTrigger("Shoot");
     
    } 


    internal void ReleaseArrow() => DeployArrow(); 


}
