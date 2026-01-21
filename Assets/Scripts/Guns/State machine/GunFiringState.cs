using Player.AimSystem;

namespace Guns.State_machine
{
    public sealed class GunFiringState : GunState
    {
        private readonly AimResult aim;
        private float cooldown;

        public GunFiringState(GunStateMachine fsm, Gun gun, AimResult aim)
            : base(fsm, gun)
        {
            this.aim = aim;
        }

        public override void Enter()
        {
            gun.ExecuteShot(aim);
            gun.ConsumeAmmo(1);
            cooldown = gun.FireCooldown; //runtime snapshot
        }

        public override void Tick(float dt)
        {
            cooldown -= dt;
            if (cooldown <= 0f)
                fsm.SwitchState(new GunIdleState(fsm, gun));
        }
    }
}