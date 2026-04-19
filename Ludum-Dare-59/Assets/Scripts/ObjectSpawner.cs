using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private bool destroySelfAfterSpawn = true;
    [SerializeField] private Transform[] objectsToSpawn;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnObjects();
        }
    }

    public void SpawnObjects()
    {
        foreach (var t in objectsToSpawn)
        {
            Instantiate(t, Vector3.zero, Quaternion.identity);
        }

        if (destroySelfAfterSpawn)
        {
            Destroy(gameObject);
        }
    }
}