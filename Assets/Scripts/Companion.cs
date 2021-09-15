using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{

    [SerializeField]
    private int damageValue = 10;

    [SerializeField]
    private GameObject player;

    private bool enemyNearby = false;

    private Collider enemyCollider;

    private Enemy enemy;

    [SerializeField]
    private float distancePlayer = 5f;

    [SerializeField]
    private float distanceEnemies = 2f;

    [SerializeField]
    private float maximumDistance = 10f;

    [SerializeField]
    private OnDestroySubscriber subscriber;

    [SerializeField]
    private NavMeshAgent agent;

    private Transform target;

    public enum State {
        Following,
        Attacking,
    }

    private State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Following;
        target = player.transform;
        //smoothFollow.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        // enemies in range
        if (enemyCollider != null){
            // already attacking
            if (state.Equals(State.Attacking))
            {
                // actually Attack (right now it attacks every frame. Use timer for less frequent attacks)
                enemy.Damage(damageValue);

                // out of player range, switch back to following, else keep attacking
                if (OutOfPlayerRange())
                {
                    Debug.Log("Companion follows player again because it reached max distance");
                    ChangeState(State.Following);
                }



            // Currently Following, but not out of player range and enemies nearby -> switch to attacking
            } else if(OutOfPlayerRange())
            {
                ChangeState(State.Following);
            } 

        } else
        {
            // no enemies in range
            if (!enemyNearby && !state.Equals(State.Following))
            {
                ChangeState(State.Following); 
            }
        }
        
        agent.destination = target.position;
    }

    private bool OutOfPlayerRange()
    {
        return maximumDistance <= Vector3.Distance(player.transform.position, this.transform.position);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Enemy"))
        {
            Debug.Log("Enemy in range: " + other.transform.position);
            if (!enemyNearby)
            {
                Debug.Log("Changing Enemy");
                enemyCollider = other;
                enemy = other.gameObject.GetComponent<Enemy>();
                other.gameObject.GetComponent<NotifyOnDestroy>().Subscribe(subscriber);
                ChangeState(State.Attacking);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.Equals(enemyCollider))
        {
            Debug.Log("enemy left");
            enemyNearby = false;
            enemyCollider = null;
        }
    }

    private void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.Following :
                target = player.transform;
                agent.stoppingDistance = distancePlayer;
                state = State.Following;
                break;

            case State.Attacking :
                enemyNearby = true;
                target = enemyCollider.gameObject.transform;
                agent.stoppingDistance = distanceEnemies;
                state = State.Attacking;
                break;
        }
    }

    internal void OnEnemyDestroyed()
    {
        Debug.Log("Companion notified");
        enemyNearby = false;
    }
}
