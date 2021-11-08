using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentControllerEnemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private NavMeshAgentController player;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        _agent.SetDestination(player.transform.position);
    }
}