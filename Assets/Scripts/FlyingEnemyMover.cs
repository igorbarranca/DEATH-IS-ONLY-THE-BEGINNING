using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyMover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float distanceForChasing = 15f;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }

    void ChasePlayer()
    {
        if(Vector2.Distance(transform.position, player.position) > distanceForChasing) { return; }
        
        var step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.position, step);
    }
}
