using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    private LayerMask layerDetection;
    private float currentIdleTime, elapsedIdleTime, LOS, enemyRayDistance = 10000f;
    public enum STATE{Idle, Patroling, ChasingSound, ChasingPlayer}
    [SerializeField] private bool canSeePlayer = false;
    private STATE currentState;
    public Transform heardNoise;
    private Transform playerTransform, eyes;
    private Transform[] patrolPointsArray = new Transform[6];
    public Animator anim;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        eyes = GameObject.Find("Eyes").GetComponent<Transform>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        for (int i = 0; i < (patrolPointsArray.Length - 1); i++)
        {
            string point = $"Patrol Point {i + 1}";
            if (point != null)
            {
                patrolPointsArray[i] = GameObject.Find(point).GetComponent<Transform>();
            }
        }
        
    }

    void Start()
    {
        layerDetection = LayerMask.GetMask("Player");
        ChangeState(STATE.Idle);
    }

    void Update() // Que hace dependiendo de cada state.
    {
        LineOfSight();
        switch (currentState)
        {
            case STATE.Idle:
                elapsedIdleTime += Time.deltaTime;
                if (elapsedIdleTime >= currentIdleTime)
                {
                    elapsedIdleTime = 0;
                    ChangeState(STATE.Patroling);
                }
                break;
            case STATE.Patroling:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    ChangeState(STATE.Idle);
                    elapsedIdleTime = 0;
                }
                break;
            case STATE.ChasingSound: //Entra en este estado SOLAMENTE si choca con una de los peos que deja el jugador.
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    ChangeState(STATE.Idle);
                    elapsedIdleTime = 0;
                }
                break;
            case STATE.ChasingPlayer:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (canSeePlayer == true) // Si sigue viendo al jugador despues de llegar a donde perseguia, sigue persiguiendo.
                    {
                        ChangeState(STATE.ChasingPlayer);
                    }
                    else // Si no, vuelve a idle.
                    {
                        ChangeState(STATE.Idle);
                        elapsedIdleTime = 0;
                    }
                }
                break;
        }
    }

    public void ChangeState(STATE enemyStateUpdate) // Funcion que actualizado el state.
    {
        currentState = enemyStateUpdate;
        switch (currentState)
        {
            case STATE.Idle:
                anim.SetBool("Walking", false);
                anim.SetBool("Running", false);
                break;
            case STATE.Patroling:
                anim.SetBool("Walking", true);
                anim.SetBool("Running", false);
                agent.SetDestination(patrolPointsArray[Random.Range(0, patrolPointsArray.Length - 1)].position);
                Debug.Log("Enemy Changes Direction");
                break;
            case STATE.ChasingSound:
                anim.SetBool("Walking", true);
                anim.SetBool("Running", false);
                agent.SetDestination(heardNoise.position);
                Debug.Log("I heard something...");
                break;
            case STATE.ChasingPlayer:
                anim.SetBool("Running", true);
                agent.SetDestination(playerTransform.position);
                Debug.Log("I see you!!!!!!");
                break;

        }
    }
    void LineOfSight() // Raycast que basicamente revisa si puede ver al player desde lejos.
    {
        LOS = eyes.localEulerAngles.y;
        LOS = Mathf.Clamp(LOS, -90, 90);
        if (Physics.Raycast(eyes.position, eyes.forward, out RaycastHit hit, enemyRayDistance, layerDetection))
        {
            Debug.DrawRay(eyes.position, eyes.forward, Color.green);
            if (hit.transform.gameObject.name == "Player")
            {
                ChangeState(STATE.ChasingPlayer);
                canSeePlayer = true;
            }
        }
        else
        {
            Debug.DrawRay(eyes.position, eyes.forward, Color.red);
            canSeePlayer = false;
        }
    }
}
