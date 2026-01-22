using System;
using Guns.State_machine;
using Player.AimSystem;
using UnityEngine;

namespace Guns
{
    public abstract class Gun : MonoBehaviour, IAimProvider
    {
        public event Action<int, int> OnAmmoAmountChanged;
        public event Action Shot;
        public event Action ReloadStarted;
        public event Action ReloadFinished;
        public event Action DryFire;
     
        [SerializeField] protected Transform firePoint;
        [SerializeField] protected Transform visualRoot;
        public Quaternion VisualRotation => visualRoot != null ? visualRoot.rotation : transform.rotation;
        
        [Header("Runtime State")]
        protected int currentAmmo;

        //Runtime snapshot from config to Initialize
        protected int clipSize;
        protected float fireCooldown;
        protected float reloadDuration;
        protected int damage;
        
        protected IDamageInstigator instigator;
        
        private GunStateMachine stateMachine;
        private bool isInitialized;

        public int CurrentAmmo => currentAmmo;
        public int MaxAmmo => clipSize;
        public bool HasAmmo => currentAmmo > 0;
        
        protected AimResult? lastAim;
        protected bool HasAimDebug()
        {
            return lastAim.HasValue;
        }
        protected Vector3 GetLastAimPoint()
        {
            return lastAim.Value.AimPoint;
        }

        //FSM read only this
        public float FireCooldown => fireCooldown;
        public float ReloadDuration => reloadDuration;
        
        //IAimProvider
        public Transform AimOrigin => firePoint;
        public virtual void OnAimUpdated(AimResult aim)
        {
            lastAim = aim;
            //visual gun rotation
            Quaternion targetRotation = Quaternion.LookRotation(aim.Direction, Vector3.up);
            visualRoot.rotation = targetRotation;     
        }

        public virtual void Initialize(GunConfig cfg, int startAmmo)
        {
            if (isInitialized)
                return;
            isInitialized = true;
            
            clipSize = cfg.clipSize;
            fireCooldown = 60f / cfg.fireRate;
            reloadDuration = cfg.reloadTime;
            damage = cfg.damage;

            currentAmmo = Mathf.Clamp(startAmmo, 0, clipSize);

            stateMachine = new GunStateMachine(this);
            
            NotifyAmmo();
        }
        
        // ===== API =====
        public void HandleFirePressed(AimResult aim)
        {
            stateMachine?.FirePressed(aim);
        }

        public void HandleFireReleased()
        {
            stateMachine?.FireReleased();
        }

        public void HandleReload()
        {
            stateMachine?.Reload();
        }

        public void Tick(float dt)
        {
            stateMachine?.Tick(dt);
        }
        
        public void SetOwner(IDamageInstigator owner)
        {
            instigator = owner;
        }

        public void ConsumeAmmo(int amount)
        {
            currentAmmo = Mathf.Max(0, currentAmmo - amount);
            NotifyAmmo();
        }

        public void RefillAmmo()
        {
            currentAmmo = clipSize;
            NotifyAmmo();
        }

        //FSM
        public void ExecuteShot(AimResult aim)
        {
            ShootLogic(aim);
            Shot?.Invoke();
        }

        protected abstract void ShootLogic(AimResult aim);

        private void NotifyAmmo()
        {
            OnAmmoAmountChanged?.Invoke(currentAmmo, clipSize);
        }
        
        public void NotifyDryFire()
        {
            DryFire?.Invoke();
        }
        
        public void StartReload()
        {
            ReloadStarted?.Invoke();
        }

        public void FinishReload()
        {
            RefillAmmo();
            ReloadFinished?.Invoke();
        }
    }
}