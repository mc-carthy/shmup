using UnityEngine;

public class Main : Singleton<Main> {

    [SerializeField]
    private GameObject[] enemyPrefabs;
    private float enemySpawnPerSecond = 0.5f;
    private float enemySpawnPadding = 1.5f;
    private float enemySpawnRate;

    protected override void Awake ()
    {
        base.Awake ();
        Utilities.SetCameraBounds (GetComponent<Camera> ());
        enemySpawnRate = 1f / enemySpawnPerSecond;
        Invoke ("SpawnEnemy", enemySpawnRate);
    }

    private void SpawnEnemy ()
    {
        int index = Random.Range (0, enemyPrefabs.Length);
        GameObject go = Instantiate (enemyPrefabs[index]) as GameObject;
        Vector3 pos = Vector3.zero;
        float xMin = Utilities.CamBounds.min.x + enemySpawnPadding;
        float xMax = Utilities.CamBounds.max.x - enemySpawnPadding;
        pos.x = Random.Range (xMin, xMax);
        pos.y = Utilities.CamBounds.max.y + enemySpawnPadding;
        go.transform.position = pos;

        Invoke("SpawnEnemy", enemySpawnRate);   
    }

}
