using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiEvent : MonoBehaviour, IPointerClickHandler
{

    static bool Can = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (Can) {
            if(transform.name == "TalkText")
            {
                int x = transform.GetSiblingIndex();

                if (x < transform.parent.childCount - 1)
                {
                    transform.parent.GetChild(transform.GetSiblingIndex() + 1).gameObject.SetActive(true);
                }

            }

            if (transform.name == "End")
            {

                Uimanager uimanager = GetComponent<Uimanager>();

                uimanager.Lond();

            }
        }
      
    }

    public void hhh() {

        Can = true;

    }
}
