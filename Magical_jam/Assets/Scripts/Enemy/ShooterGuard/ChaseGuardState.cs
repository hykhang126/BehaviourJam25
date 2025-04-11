using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseGuardState : GuardState
{
    public ChaseGuardState(ShootingGuard ctx) : base(ctx) { }

    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        ctx.moveToPlayer();
        
        if (ctx.isNextToPlayer())
        {
            ctx.SwitchState(new AttackGuardState(ctx));
        }   
    }

    public override void Exit()
    {
        
    }
}
