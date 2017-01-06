using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Main : Singleton<Main> {

    static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;

    [SerializeField]
    private GameObject[] enemyPrefabs;
    // [SerializeField]
    public WeaponDefinition[] weaponDefinitions;
    private WeaponType[] activeWeaponTypes;
    private float enemySpawnPerSecond = 0.5f;
    private float enemySpawnPadding = 1.5f;
    private float enemySpawnRate;
    

    protected override void Awake ()
    {
        base.Awake ();
        Utilities.SetCameraBounds (GetComponent<Camera> ());
        enemySpawnRate = 1f / enemySpawnPerSecond;
        Invoke ("SpawnEnemy", enemySpawnRate);

        W_DEFS = new Dictionary<WeaponType, WeaponDefinition> ();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            W_DEFS[def.type] = def;
        }
    }

    private void Start ()
    {
        activeWeaponTypes = new WeaponType[weaponDefinitions.Length];
        for (int i = 0; i < weaponDefinitions.Length; i++)
        {
            activeWeaponTypes[i] = weaponDefinitions[i].type;
        }
    }

    static public WeaponDefinition GetWeaponDefinition (WeaponType wt)
    {
        if (W_DEFS.ContainsKey (wt))
        {
            return W_DEFS[wt];
        }
        return new WeaponDefinition ();
    }

    public void DelayedRestart (float delay)
    {
        Invoke ("Restart", delay);
    }

    public void Restart ()
    {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name, LoadSceneMode.Single);
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
