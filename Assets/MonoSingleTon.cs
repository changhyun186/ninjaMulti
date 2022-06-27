using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    static bool ShuttingDown;
    static T instance;
    public static T Instance
    {
        get
        {
            if(ShuttingDown)
            {
                Debug.LogWarning("[Instance]instance"+typeof(T)+"is already existring.");
                return null; 

            }
            if (!instance)
            {
                instance = FindObjectOfType<T>(true);
                if(instance==null)
                {
                    Debug.LogError("인스턴스가 하이라키에 없습니다.");
                    return null;

                }
                    
                //instance = (T)FindObjectOfType(typeof(T));
                //if (!instance)
                //{
                //    GameObject temp = new GameObject(typeof(T).ToString());
                //    instance = temp.AddComponent<T>();
                //}
            }
            return instance;
        }


    }

    //private void OnApplicationQuit()
    //{
    //    ShuttingDown = true;
    //}
    //private void OnDestroy()
    //{
    //    ShuttingDown = true;
    //}
}

public class MonoSingleTonUndead<T> : MonoBehaviour where T : MonoBehaviour
{
    static bool ShuttingDown;
    static T instance;
    public static T Instance
    {
        get
        {
            if (ShuttingDown)
            {
                Debug.LogWarning("[Instance]instance" + typeof(T) + "is already existring.");
                return null;

            }
            if (!instance)
            {
                instance = FindObjectOfType<T>(true);
                DontDestroyOnLoad(instance);
                
                if (instance == null)
                {
                    Debug.LogError("인스턴스가 하이라키에 없습니다.");
                    return null;

                }

                //instance = (T)FindObjectOfType(typeof(T));
                //if (!instance)
                //{
                //    GameObject temp = new GameObject(typeof(T).ToString());
                //    instance = temp.AddComponent<T>();
                //}
            }

            T[] instances = FindObjectsOfType<T>(true);

            if (instances.Length > 1)
            {
                foreach(var element in instances)
                {
                    if(element!=instance)
                    {
                        Destroy(element);
                    }
                }
            }

            return instance;
        }


    }

    //private void OnApplicationQuit()
    //{
    //    ShuttingDown = true;
    //}
    //private void OnDestroy()
    //{
    //    ShuttingDown = true;
    //}
}