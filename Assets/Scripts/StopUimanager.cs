using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopUimanager : MonoBehaviour
{
    public GameObject Stoppanel;
    public bool isstop;
    // Start is called before the first frame update
    void Start()
    {
        isstop=false;
        Stoppanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isstop)
        {
           
            Stopgame();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isstop)
        {
            Backgame();
        }
    }
    public void Stopgame()
    {
        Debug.Log("stop");
        Stoppanel.SetActive(true);
        Time.timeScale = 0;
        isstop = true;
    }
    public void Backgame()
    {
        Stoppanel.SetActive(false);
        Time.timeScale = 1;
        isstop = false;
    }
}
