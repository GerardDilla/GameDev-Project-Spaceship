using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCollector : MonoBehaviour
{
    public GameObject CollectEffect;
    private ObjectPooler objectPooler;
    private int currentBalance;

    private void OnEnable()
    {
        // PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("Coin"))
        {
            currentBalance = PlayerPrefs.GetInt("Coin");
        }
        else
        {
            currentBalance = 0;
        }

        objectPooler = GameObject.Find("ObjectPooler").GetComponent<ObjectPooler>();
    }

    private void FixedUpdate()
    {
        CoinCollector();
    }
    private void CoinCollector()
    {
        Collider2D[] magnet = Physics2D.OverlapCircleAll(transform.position, 3f);
        foreach (Collider2D objectnear in magnet)
        {
            if (objectnear.gameObject.tag == "Collectible")
            {
                var distance = objectnear.transform.position - transform.position;
                var difference = -distance.normalized;
                objectnear.GetComponent<Rigidbody2D>().velocity = difference * 200;
            }
        }
        Collider2D[] collector = Physics2D.OverlapCircleAll(transform.position, 1.5f);
        foreach (Collider2D loot in collector)
        {
            if (loot.gameObject.tag == "Collectible")
            {
                var lootscript = loot.GetComponent<Collectible>();
                if (lootscript == null) return;

                RegisterCollectible(lootscript.Amount);
                loot.gameObject.SetActive(false);
            }
        }
    }
    public void RegisterCollectible(int Amount)
    {
        currentBalance = currentBalance + Amount;
        PlayerPrefs.SetInt("Coin", currentBalance);
        Debug.Log(PlayerPrefs.GetInt("Coin"));
        CollectionEffect();
    }
    private void CollectionEffect()
    {
        if (CollectEffect == null) return;

        GameObject collecteffect = objectPooler.GetPooledObject(CollectEffect);
        collecteffect.SetActive(true);
        collecteffect.transform.position = transform.position;
    }
}
