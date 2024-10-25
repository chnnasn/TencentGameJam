using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopUimanager : MonoBehaviour
{
    public GameObject Stoppanel;
     bool isstop;
    // Start is called before the first frame update
    void Start()
    {
        isstop=false;
        Stoppanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isstop && Input.GetKeyDown(KeyCode.Escape))
        //{

        //    Stopgame();
        //}

        //if (isstop && Input.GetKeyDown(KeyCode.Escape) )
        //{
        //    Backgame();
        //}

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isstop)
            {
                Stopgame();

            }
            else {
                Backgame();
            }
        }
    }
     void Stopgame()
    {
        Stoppanel.SetActive(true);
        isstop = true;
        Time.timeScale = 0;

    }
     void Backgame()
    {
        Stoppanel.SetActive(false);
        isstop = false;
        Time.timeScale = 1;

    }

    public void QuitGame() {

        Application.Quit();
    }

    public void Continue() {

        Backgame();
    }
}
