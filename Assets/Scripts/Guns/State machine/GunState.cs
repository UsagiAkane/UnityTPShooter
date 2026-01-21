using Player.AimSystem;

namespace Guns.State_machine
{
    public abstract class GunState
    {
        protected readonly GunStateMachine fsm;
        protected readonly Gun gun;

        protected GunState(GunStateMachine fsm, Gun gun)
        {
            this.fsm = fsm;
            this.gun = gun;
        }

        public virtual void Enter() {}
        public virtual void Exit() {}

        public virtual void FirePressed(AimResult aim) {}
        public virtual void FireReleased() {}
        public virtual void Reload() {}

        public virtual void Tick(float dt) {}
    }
}