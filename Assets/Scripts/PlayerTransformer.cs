using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class PlayerTransformer : MonoBehaviour
{
    [SerializeField] float spiritStateDuration = 3f;
    [SerializeField] GameObject enemies;

    [SerializeField] GameSession gameSession;

    Animator anim;

    public PlayerState LifeState { get; private set; }

    //public event Action onStateChangeAction;
    private void Awake()
    {
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
            switch (LifeState)
            {
                case PlayerState.Spirit:
                    break;

                case PlayerState.Alive:
                    StartCoroutine(SpiritState());
                    break;

                case PlayerState.TemporaryAlive:
                    LifeState = PlayerState.Dead;
                    gameSession.ReloadLevel();
                    break;
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (LifeState == PlayerState.TemporaryAlive)
            {
                LifeState = PlayerState.Alive;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FlyingEnemyMover enemy = collision.gameObject.GetComponent<FlyingEnemyMover>();
        if(enemy != null)
        {
            LifeState = PlayerState.Dead;
            gameSession.ReloadLevel();
        }
    }

    IEnumerator SpiritState()
    {
        LifeState = PlayerState.Spirit;
        enemies.SetActive(true);
        anim.SetBool("isSpirit", true);
        
        yield return new WaitForSeconds(spiritStateDuration);
        
        LifeState = PlayerState.TemporaryAlive;
        enemies.SetActive(false);
        anim.SetBool("isSpirit", false);
    }

}

public enum PlayerState 
{ 
    Alive,
    Spirit,
    TemporaryAlive,
    Dead
}


