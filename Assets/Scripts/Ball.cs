using Fusion;

public class Ball : NetworkBehaviour
{
    [Networked]
    private TickTimer life { get; set; }
    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
        else
        {
            transform.position += transform.position * Runner.DeltaTime * 5;    
        }
    }

    public void Init()
    {
        life = TickTimer.CreateFromSeconds(Runner, 5);
    }
}
