using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPooler : MonoBehaviour
{

    public GameObject loot;
    public Vector2 Amount;
    private ObjectPooler objectPooler;
    private void OnEnable()
    {
        GenerateLoot();
    }
    public void GenerateLoot()
    {
        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
        Vector3 direction = transform.localPosition;
        float numberSpawn = Random.Range(Amount.x, Amount.y);
        Debug.Log(direction);
        for (int i = 0; i <= 5; i++)
        {
            Debug.Log(direction.normalized);
            var effect = objectPooler.GetPooledObject(loot);
            effect.transform.position = direction;
            // effect.transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, 1 * Time.deltaTime);
            effect.SetActive(true);

            direction.x = direction.x + Random.Range(0, 2) * 2 - 1;
            direction.y = direction.y + Random.Range(0, 2) * 2 - 1;
        }
        Destroy(gameObject);

    }
}
