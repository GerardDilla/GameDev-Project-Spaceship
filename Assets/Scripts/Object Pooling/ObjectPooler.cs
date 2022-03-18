using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private GameObject parent;
    public GameObject GetPooledObject(GameObject objectReferred, GameObject customParent = null, string objectName = "")
    {

        // Sets default object name
        if (objectName == "") objectName = objectReferred.name;

        // Sets parent where object is/will be assigned
        parent = checkCustomParent(objectName, customParent);

        if (parent != null)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (!child.gameObject.activeInHierarchy)
                {

                    if (objectName == child.gameObject.name)
                    {
                        return child.gameObject;
                    }
                }
            }
        }
        else
        {
            parent = new GameObject(objectName + "_Parent");
        }
        GameObject obj = (GameObject)Instantiate(objectReferred);
        obj.name = objectName;
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
                GameObject heirarchyCategory = new GameObject(heirarchyCategoryName);
            }
        }
        for (int i = 0; i < pooledAmount; i++)
        {
            if (customParent != null)
            {
                GameObject obj = (GameObject)Instantiate(objectPool);
                obj.name = objectPool.name;
                obj.transform.parent = customParent.transform;
                obj.SetActive(false);
            }
            else
            {
                GameObject obj = (GameObject)Instantiate(objectPool);
                obj.transform.parent = GameObject.Find(heirarchyCategoryName).transform;
                obj.name = objectPool.name;
                obj.SetActive(false);
            }
        }
    }
    private GameObject checkCustomParent(string objectName, GameObject customParent)
    {
        GameObject parent;
        if (customParent != null)
        {
            parent = customParent;
        }
        else
        {
            parent = GameObject.Find(objectName + "_Parent");
        }
        return parent;
    }


}
