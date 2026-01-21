namespace Guns.State_machine
{
    public sealed class GunReloadingState : GunState
    {
        private float timer;

        public GunReloadingState(GunStateMachine fsm, Gun gun)
            : base(fsm, gun)
        {
            timer = gun.ReloadDuration; //runtime snapshot
        }

        public override void Tick(float dt)
        {
            timer -= dt;
            if (timer <= 0f)
            {
                gun.RefillAmmo();
                fsm.SwitchState(new GunIdleState(fsm, gun));
            }
        }
    }

}