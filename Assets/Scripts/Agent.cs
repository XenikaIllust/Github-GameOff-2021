using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
}