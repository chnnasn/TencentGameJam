using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Uimanager : MonoBehaviour
{
    static bool CanLond = false;
    static bool hhh = false;
    // Start is called before the first frame update
    void Start()
    {

        if (transform.name == "11") {
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
                if (CanLond) {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    public void Done() {

        transform.parent.GetChild(1).gameObject.SetActive(true);

        transform.parent.GetChild(0).gameObject.SetActive(false);

    }

    static bool uuu = true;
    // Update is called once per frame
    void Update()
    {
        if (hhh) {

            if (uuu && Input.anyKeyDown) {
                if (transform.name == "11") {
                    gameObject.GetComponent<Animator>().Play("8888");

                }

            }
        
        }
    }

    public void uu() {
        uuu = false;


    }

    public void Lond() {

        CanLond = true;
    }

    public void Text() {

        transform.parent.GetChild(2).gameObject.SetActive(true);
    }

    public void h() {

        hhh = true;
    }
}
