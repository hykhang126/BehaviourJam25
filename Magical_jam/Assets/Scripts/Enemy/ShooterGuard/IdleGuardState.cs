using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleGuardState : GuardState
{
    public IdleGuardState(ShootingGuard ctx) : base(ctx) { }

    public override void Start() { }


    public override void Update()
    {
        
        if (ctx.isPlayerNearby())
        {
            ctx.SwitchState(new ChaseGuardState(ctx));
        }
    }

    public override void Exit()
    {
        
    }
}
