using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerTransformer : MonoBehaviour
{
    [SerializeField] Sprite aliveSprite;
    [SerializeField] Sprite spiritSprite;
    SpriteRenderer spriteRenderer;

    [SerializeField] float spiritStateDuration = 3f;

    Animator anim;

    public PlayerState LifeState { get; private set; }

    bool isLastChance = false;

    //public event Action onStateChangeAction;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        LifeState = PlayerState.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(LifeState);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Hazard"))
        {
            //se alive -> spirit : coroutine di 3 secondi dove poi torna in alive state.
            if(LifeState == PlayerState.Spirit) { return; }
            if (isLastChance)
            {
                LifeState = PlayerState.Dead;
            }
            if(LifeState == PlayerState.Alive)
            {
                isLastChance = true;
                StartCoroutine(SpiritStateActivator());
            }
        }
        else if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isLastChance = false;
        }
    }

    IEnumerator SpiritStateActivator()
    {
        
        LifeState = PlayerState.Spirit;
        anim.SetBool("isSpirit", true);
        
        yield return new WaitForSeconds(spiritStateDuration);
        
        LifeState = PlayerState.Alive;
        anim.SetBool("isSpirit", false);
    }

}

public enum PlayerState 
{ 
    Alive,
    Spirit,
    Dead
}


