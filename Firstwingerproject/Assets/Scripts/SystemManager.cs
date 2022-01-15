using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    // Start is called before the first frame update

    static SystemManager intance = null;

    public static SystemManager Instance
    {
        get
        {
            return intance;
        }
    }

    private void Awake()
    {
        if (intance != null)
        {
            Debug.LogError("SystemManager error! Singltone error");
            Destroy(gameObject);
            return;
        }
        intance = this;
    }

    [SerializeField]
    Player player;
    public Player Hero
    {
        get
        {
            return player;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
