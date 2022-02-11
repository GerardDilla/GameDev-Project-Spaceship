using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public GameObject GetPooledObject(GameObject objectReferred)
    {
        // Debug.Log(objectIndex);
        // List<GameObject> childList = new List<GameObject>();
        // GameObject objectReferred = objects[objectIndex];
        GameObject parent = GameObject.Find(objectReferred.name + "_Parent");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (!child.gameObject.activeInHierarchy)
            {
                return child.gameObject;
            }
        }
        GameObject obj = (GameObject)Instantiate(objectReferred);
        var parentName = GameObject.Find(objectReferred.name + "_Parent");
        obj.transform.parent = parentName.transform;
        obj.SetActive(false);
        return obj;

    }

    public void InstantiateObjects(GameObject objectPool, int pooledAmount)
    {
        var heirarchyCategory = new GameObject(objectPool.name + "_Parent");
        if (GameObject.Find(heirarchyCategory.name) == null)
        {
            Instantiate(heirarchyCategory);
        }
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectPool);
            obj.transform.parent = heirarchyCategory.transform;
            obj.SetActive(false);
        }
    }

}
