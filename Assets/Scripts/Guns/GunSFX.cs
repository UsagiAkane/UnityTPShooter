using UnityEngine;

namespace Guns
{
    using UnityEngine;

    public class GunSFX : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource source;

        [Header("Fire")]
        [SerializeField] private AudioClip shotClip;
        [SerializeField] private Vector2 shotPitchRange = new Vector2(0.95f, 1.05f);

        [Header("Dry Fire")]
        [SerializeField] private AudioClip dryFireClip;

        [Header("Reload")]
        [SerializeField] private AudioClip reloadStartClip;
        [SerializeField] private AudioClip reloadEndClip;

        private Gun gun;

        private void Awake()
        {
            gun = GetComponentInParent<Gun>();
        }

        private void OnEnable()
        {
            gun.Shot += PlayShot;
            gun.DryFire += PlayDryFire;
            gun.ReloadStarted += PlayReloadStart;
            gun.ReloadFinished += PlayReloadEnd;
        }

        private void OnDisable()
        {
            gun.Shot -= PlayShot;
            gun.DryFire -= PlayDryFire;
            gun.ReloadStarted -= PlayReloadStart;
            gun.ReloadFinished -= PlayReloadEnd;
        }

        private void PlayShot()
        {
            if (shotClip == null) return;

            source.pitch = Random.Range(shotPitchRange.x, shotPitchRange.y);
            source.PlayOneShot(shotClip);
        }

        private void PlayDryFire()
        {
            if (dryFireClip == null) return;
            source.PlayOneShot(dryFireClip);
        }

        private void PlayReloadStart()
        {
            if (reloadStartClip == null) return;
            source.PlayOneShot(reloadStartClip);
        }

        private void PlayReloadEnd()
        {
            if (reloadEndClip == null) return;
            source.PlayOneShot(reloadEndClip);
        }
    }

}