using System;
using UnityEngine;

public class FallingSpikePlat : MonoBehaviour
{
    // Start is called before the first frame update
    //Components
    [SerializeField] BoxCollider2D platform;
    [SerializeField] BoxCollider2D Spikes;
    private Rigidbody2D SpikeRB;

    internal FallingSpikePlat instance;
    internal Action CollideWithSpike;

    //Config Position and magnitude
    [SerializeField] float yMin, yMax;
    [SerializeField] float Magnitude = 20f;
    [SerializeField] float delayTime = 0f;

    void Awake()
    {
        SpikeRB = GetComponent<Rigidbody2D>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        SmashPlayerWithSpikes();
    }
    private void FixedUpdate()
    {
         MoveUpDown();
    }

    private void MoveUpDown()
    {
       
        Vector2 PlatformPos = new Vector2(transform.position.x, Mathf.Sin(delayTime + Time.time) * Magnitude );
        PlatformPos.y = Mathf.Clamp(PlatformPos.y, yMin, yMax);
      
       SpikeRB.MovePosition(PlatformPos);
    }

    private void SmashPlayerWithSpikes() {
        if (Spikes.IsTouchingLayers(LayerMask.GetMask("Player")))
            CollideWithSpike();
        
    }


}
