using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    // Start is called before the first frame update

    static SystemManager instance = null;

    public SystemManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("SystemManager error! Singltone error");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
