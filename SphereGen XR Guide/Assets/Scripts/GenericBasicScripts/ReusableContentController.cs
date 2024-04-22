using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReusableContentController : MonoBehaviour
{
    private static ReusableContentController _instance;
    public static ReusableContentController Instance
    {
        get;
        private set;
    }

    //this is the list of prefabs you expect to be generated at runtime that you want to keep track of.
    public List<GameObject> ExpectedContent;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckContent()
    {
        
    }
}
