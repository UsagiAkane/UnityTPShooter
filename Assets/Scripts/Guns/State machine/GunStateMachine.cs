using Player.AimSystem;

namespace Guns.State_machine
{
    public class GunStateMachine
    {
        private GunState current;

        public GunStateMachine(Gun gun)
        {
            current = new GunIdleState(this, gun);
            current.Enter();
        }

        public void FirePressed(AimResult aim)
            => current.FirePressed(aim);

        public void FireReleased()
            => current.FireReleased();

        public void Reload()
            => current.Reload();

        public void Tick(float dt)
            => current.Tick(dt);

        public void SwitchState(GunState next)
        {
            current.Exit();
            current = next;
            current.Enter();
        }
    }
}