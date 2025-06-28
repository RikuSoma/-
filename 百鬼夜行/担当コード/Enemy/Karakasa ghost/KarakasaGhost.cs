using UnityEngine;

public class KarakasaGhost : TargetingEnemy
{
    private KarakasaGhostMovement karakasaMovement;

    [SerializeField] private KarakasaMovementConfig movementConfig;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        karakasaMovement = GetComponent<KarakasaGhostMovement>();
        if (karakasaMovement == null)
        {
            karakasaMovement = gameObject.AddComponent<KarakasaGhostMovement>();
        }

        karakasaMovement.Initialize(this);
        karakasaMovement.SetConfig(movementConfig);

        if (healthManager != null)
        {
            healthManager.OnDeath += OnDeath;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDeath()
    {
        GameObject gomi = Instantiate(piecesPrefab, transform.position, Quaternion.identity);
        Destroy(gomi, GomiLifeTime);
        Destroy(gameObject);
    }
}