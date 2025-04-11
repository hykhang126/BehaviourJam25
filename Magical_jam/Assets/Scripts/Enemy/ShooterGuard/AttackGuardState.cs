using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGuardState : GuardState
{
    // Start is called before the first frame update
    public AttackGuardState(ShootingGuard ctx) : base(ctx)
    {
    }

    public override void Start()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        ctx.Shoot();
    }

    public override void Exit()
    {
        
    }
}
