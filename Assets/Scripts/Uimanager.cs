using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Uimanager : MonoBehaviour
{
    static bool CanLond = false;
    // Start is called before the first frame update
    void Start()
    {

        if (transform.name == "Slider") {
            StartCoroutine(LondingNextScene());
        }
        
    }

    IEnumerator LondingNextScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                if (CanLond && Input.anyKeyDown) {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    public void Done() {

        transform.parent.GetChild(1).gameObject.SetActive(true);
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Lond() {

        CanLond = true;
    }
}
