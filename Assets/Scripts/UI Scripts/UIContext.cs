using UnityEngine;

public class UIContext : MonoBehaviour
{
    public static UIContext Instance { get; private set; }

    public WeaponInventory WeaponInventory { get; private set; }
    public ScoreManager ScoreManager { get; private set; }
    public LevelManager LevelManager { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        WeaponInventory = FindAnyObjectByType<WeaponInventory>();
        ScoreManager = FindAnyObjectByType<ScoreManager>();
        LevelManager = FindAnyObjectByType<LevelManager>();
    }
}