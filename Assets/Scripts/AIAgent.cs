public class AIAgent : Agent
{
    private PlayerAgent _player;

    protected override void Awake()
    {
        base.Awake();
        _player = FindObjectOfType<PlayerAgent>();
    }

    private void Update()
    {
        agent.SetDestination(_player.transform.position);
    }
}