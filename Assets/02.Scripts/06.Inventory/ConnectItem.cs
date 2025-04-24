using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectItem : MonoBehaviour
{
    public List<ConnectionBetweenItemObject> connectionBetweenItemObjects;
    [System.Serializable]
    public class ConnectionBetweenItemObject
    {
        public int ItemData_Num;
        public GameObject Object;
    }
    public GameObject FindObject(int num)
    {
        for (int i = 0; i < connectionBetweenItemObjects.Count; i++)
        {
            if (connectionBetweenItemObjects[i].ItemData_Num == num)
            {
                return connectionBetweenItemObjects[i].Object;
            }
        }
        return null;
    }
}
