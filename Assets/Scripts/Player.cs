using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype _characterController;
    [SerializeField] private Ball _ballPrefab;
    private Vector3 _forward;
    [Networked] private TickTimer delay { get; set; }
    private void Awake()
    {
        _characterController = GetComponent<NetworkCharacterControllerPrototype>();
        _forward = transform.forward;
    }
    
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            _characterController.Move(data.Direction*Runner.DeltaTime * 5);
            
            if (data.Direction.sqrMagnitude > 0)
            {
                _forward = data.Direction;
            }

            if (delay.ExpiredOrNotRunning(Runner))
            {
                if ((data.Buttons & NetworkInputData.MOUSEBUTTON1) != 0)
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
                    Runner.Spawn(_ballPrefab, transform.position + _forward,
                        Quaternion.LookRotation(_forward),
                        Object.InputAuthority, (runner, o) =>
                        {
                            o.GetComponent<Ball>().Init();
                        });
                }    
            }
        }
    }
}
