using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform playerTransform;

    public enum STATE
    {
        Idle,
        Patroling,
        ChasingSound,
        ChasingPlayer
    }
    public STATE currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    private void ChangeState(STATE state)
    {
        currentState = state;
    }
}
