using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public GameObject GetPooledObject(GameObject objectReferred, GameObject customParent = null)
    {
        // Debug.Log(objectIndex);
        // List<GameObject> childList = new List<GameObject>();
        // GameObject objectReferred = objects[objectIndex];
        GameObject parent = null;

        if (customParent != null)
        {
            parent = customParent;
        }
        else
        {
            parent = GameObject.Find(objectReferred.name + "_Parent");
        }

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (!child.gameObject.activeInHierarchy)
            {
                return child.gameObject;
            }
        }
        GameObject obj = (GameObject)Instantiate(objectReferred);
        obj.transform.parent = parent.transform;
        obj.SetActive(false);
        return obj;


    }

    public void InstantiateObjects(GameObject objectPool, int pooledAmount, GameObject customParent = null)
    {
        string heirarchyCategoryName = objectPool.name + "_Parent";
        if (customParent == null)
        {
            if (GameObject.Find(heirarchyCategoryName) == null)
            {
                // Debug.Log(GameObject.Find(heirarchyCategoryName));
                GameObject heirarchyCategory = new GameObject(heirarchyCategoryName);

                // Instantiate(new GameObject(heirarchyCategoryName));

            }
        }

        for (int i = 0; i < pooledAmount; i++)
        {

            if (customParent != null)
            {
                GameObject obj = (GameObject)Instantiate(objectPool);
                obj.transform.parent = customParent.transform;
                obj.SetActive(false);
            }
            else
            {
                GameObject obj = (GameObject)Instantiate(objectPool);
                obj.transform.parent = GameObject.Find(heirarchyCategoryName).transform;
                obj.SetActive(false);
            }

        }



    }

}
