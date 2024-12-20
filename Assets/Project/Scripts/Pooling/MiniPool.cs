using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPool<T> where T : Component
{
    private Queue<T> pools = new Queue<T>();
    private List<T> listActives = new List<T>();
    int index = 0;

    T prefab;
    Transform parent;

    public void OnInit(T prefab, int amount, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.index = 0;

        for (int i = 0; i < amount; i++)
        {
            Despawn(GameObject.Instantiate(prefab, parent));
        }
    }

    public T Spawn(Vector3 pos, Quaternion rot)
    {
        T go = pools.Count > 0 ? pools.Dequeue() : GameObject.Instantiate(prefab, parent);

        listActives.Add(go);

        go.transform.SetPositionAndRotation(pos, rot);
        go.gameObject.SetActive(true);

        return go;
    }
    public T Spawn()
    {
        //T go = pools.Count > 0 ? pools.Dequeue() : GameObject.Instantiate(prefab, parent);

        //listActives.Add(go);
        //go.gameObject.SetActive(true);

        //return go;

        T go = index < listActives.Count ? listActives[index] : null;

        if (go == null)
        {
            go = GameObject.Instantiate(prefab, parent);
            listActives.Add(go);
        }

        index++;
        go.gameObject.SetActive(true);

        return go;
    }

    public void Despawn(T obj)
    {
        if (obj.gameObject.activeSelf)
        {
            obj.gameObject.SetActive(false);
            //pools.Enqueue(obj);
            //listActives.Remove(obj);
        }
    }

    public void Collect()
    {
        //while (listActives.Count > 0)
        //{
        //    Despawn(listActives[0]);
        //}

        for (int i = 0; i < index; i++)
        {
            Despawn(listActives[i]);
        }
        index = 0;
    }

    public void Release()
    {
        //Collect();

        //while (pools.Count > 0)
        //{
        //    GameObject.Destroy(pools.Dequeue().gameObject);
        //}

        Collect();
        for (int i = 0; i < listActives.Count; i++)
        {
            GameObject.Destroy(listActives[i].gameObject);
        }
        listActives.Clear();
        index = 0;
    }

}
