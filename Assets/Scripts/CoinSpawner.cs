using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
  [SerializeField] private GameObject coinPrefab;
    [SerializeField] private int numberOfCoins = 30;
    [SerializeField] private int randomSeed = 42; // Seed for random number generation to ensure consistent coin placement across runs

    [SerializeField] private float width = 20f;
    [SerializeField] private float depth = 20f;
    [SerializeField] private float spawnHeight = 15f;

    void Start()
    {
        Random.InitState(randomSeed); // Initialize the random number generator with the specified seed for consistent coin placement

        for (int i = 0; i < numberOfCoins; i++)
        {
            SpawnCoin();
        }
    }

    void SpawnCoin()
    {
        float randomX = Random.Range(-width / 2f, width / 2f);
        float randomZ = Random.Range(-depth / 2f, depth / 2f);

        Vector3 spawnPosition = transform.position + new Vector3(randomX, 0, randomZ);

        // Quaternion randomRotation = Random.rotation;

        GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

        Rigidbody rb = coin.GetComponent<Rigidbody>();

        Vector3 force = new Vector3(
            Random.Range(-.6f, .6f),
            Random.Range(0f, .2f),
            Random.Range(-.6f, .6f));

        rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);

        rb.AddForce(force, ForceMode.Impulse);
    }
}
