using UnityEngine;

// CoinSpawner
// Used in Level 1 on the CoinSpawnPoint object.
// Creates a set number of coins scattered randomly inside a rectangle
// centered on this object, then gives each one a little push and spin
// so they tumble onto the floor instead of all landing flat.
// (Levels 2 and 3 place their coins by hand instead.)
public class CoinSpawner : MonoBehaviour
{
  [SerializeField] private GameObject coinPrefab;    // The coin to copy (GoldCoinBlank prefab). Must have a Rigidbody.
    [SerializeField] private int numberOfCoins = 30;
    [SerializeField] private int randomSeed = 42; // Seed for random number generation to ensure consistent coin placement across runs

    // Size of the spawn area in world units. The area is centered on this
    // object, so it reaches width/2 to the left and width/2 to the right.
    [SerializeField] private float width = 20f;
    [SerializeField] private float depth = 20f;

    void Start()
    {
        // Note: this seeds Unity's shared random generator, so every other
        // script that uses Random also gets the same "random" numbers each run.
        Random.InitState(randomSeed); // Initialize the random number generator with the specified seed for consistent coin placement

        for (int i = 0; i < numberOfCoins; i++)  // Run SpawnCoin once per coin
        {
            SpawnCoin();
        }
    }

    // Creates one coin at a random spot in the spawn area.
    void SpawnCoin()
    {
        // Pick a random X and Z inside the rectangle. Y stays at the spawner's height.
        float randomX = Random.Range(-width / 2f, width / 2f);
        float randomZ = Random.Range(-depth / 2f, depth / 2f);

        // Offset from the spawner's position, so moving the spawner moves the whole area
        Vector3 spawnPosition = transform.position + new Vector3(randomX, 0, randomZ);

        // Quaternion randomRotation = Random.rotation;

        // Instantiate makes a copy of the prefab in the scene.
        // Quaternion.identity means "no rotation".
        GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

        // Get the coin's Rigidbody so we can push it with physics
        Rigidbody rb = coin.GetComponent<Rigidbody>();

        // A small random push: sideways in X and Z, slightly upward in Y
        Vector3 force = new Vector3(
            Random.Range(-.6f, .6f),
            Random.Range(0f, .2f),
            Random.Range(-.6f, .6f));

        // Torque is a rotational push. insideUnitSphere gives a random direction.
        // ForceMode.Impulse applies it all at once instead of over time.
        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);

        rb.AddForce(force, ForceMode.Impulse);
    }
}
