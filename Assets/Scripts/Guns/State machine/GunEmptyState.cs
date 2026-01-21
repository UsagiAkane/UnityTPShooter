namespace Guns.State_machine
{
    public sealed class GunEmptyState : GunState
    {
        public GunEmptyState(GunStateMachine fsm, Gun gun) : base(fsm, gun) { }

        public override void Reload()
        {
            fsm.SwitchState(new GunReloadingState(fsm, gun));
        }
    }
}