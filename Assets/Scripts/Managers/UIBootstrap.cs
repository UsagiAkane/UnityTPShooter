using UnityEngine;

namespace Managers
{
    public class UIBootstrap : MonoBehaviour
    {
        [Header("Managers")]
        [SerializeField] private WeaponInventory weaponInventory;
        [SerializeField] private ScoreManager scoreManager;

        [Header("UI")]
        [SerializeField] private AmmoUI ammoUI;
        [SerializeField] private ScoreUI scoreUI;

        private void Awake()
        {
            // Score
            scoreUI.Bind(scoreManager);

            // Ammo
            ammoUI.Bind(weaponInventory);
        }
    }
}