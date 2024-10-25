using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    static bool can = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void end() {

        if (can) {


            Application.Quit();
        }
    
    }

    public void Con() {
        if (can) {
            SceneManager.LoadScene(1);

        }
      
    }

    public void h() {
        can = true;
    }

}
