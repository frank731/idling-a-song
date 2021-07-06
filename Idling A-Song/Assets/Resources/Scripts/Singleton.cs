using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    public static T Instance { 
        get
        {
            if (_instance == null)
            { // if the singleton instance has not yet been initialized
                _instance = FindObjectOfType<T>();
            }
            return _instance;
        }
    } 
    protected virtual void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            _instance = null;
        }
    }

}