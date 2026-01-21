using Player.AimSystem;

namespace Guns.State_machine
{
    public class GunIdleState : GunState
    {
        public GunIdleState(GunStateMachine fsm, Gun gun)
            : base(fsm, gun) { }

        public override void FirePressed(AimResult aim)
        {
            if (!gun.HasAmmo)
            {
                fsm.SwitchState(new GunEmptyState(fsm, gun));
                return;
            }

            fsm.SwitchState(new GunFiringState(fsm, gun, aim));
        }

        public override void Reload()
        {
            fsm.SwitchState(new GunReloadingState(fsm, gun));
        }
    }
}