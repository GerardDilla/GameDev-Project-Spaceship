using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    public GameObject Loot;
    public Vector2 Amount = new Vector2(0, 3);
    private ObjectPooler objectPooler;

    private void OnEnable()
    {
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
    }

    public void DropLoot()
    {
        Vector3 direction = transform.position;
        float numberSpawn = Random.Range(Amount.x, Amount.y);
        for (int i = 0; i <= numberSpawn; i++)
        {
            var loot = objectPooler.GetPooledObject(Loot);
            loot.transform.position = direction;
            loot.SetActive(true);
            direction.x = direction.x + ((Random.Range(0, 2) * 2 - 1) * 0.7f);
            direction.y = direction.y + ((Random.Range(0, 2) * 2 - 1) * 0.7f);
        }
    }

}
