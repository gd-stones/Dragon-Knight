using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    private Vector3[] initialPositon;

    private void Awake()
    {
        // save the initial positions of the enemies
        initialPositon = new Vector3[enemies.Length];
        for (int  i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                initialPositon[i] = enemies[i].transform.position;
            }
        }
    }

    public void ActivateRoom(bool _status)
    {
        // activate/deactivate enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status);
                enemies[i].transform.position = initialPositon[i];
            }
        }
    }
}
