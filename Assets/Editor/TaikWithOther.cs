using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum Talking
{
    StartTalking,
    Talking,
    NotTalking

}

[System.Serializable]
public class StringList
{
    public List<string> items;

    public StringList(List<string> items)
    {
        this.items = items;
    }
}

public class TaikWithOther : MonoBehaviour
{

    Talking newState;
    private List<string> ObjectWord = new List<string>();
    public Text Talk;
    public static TaikWithOther instance;
    public float WorlSpeed = 0.1f; // 默认的逐字显示速度
    bool WorkLodng = false; // 用于跟踪是否正在逐字显示
    int WordIndex = -1;

     void Awake()
    {

        StartCoroutine(ReadJsonFile());

        if (instance != null) {
            Destroy(gameObject);

        }
        else {
            instance = this;
        
        }
        newState = Talking.NotTalking;
    }

    void Start()
    {

    }

    public void Click() {
        if (newState == Talking.Talking) {
            if (WorkLodng)
            {
                StopAllCoroutines();

                WorkLodng = false;

                Talk.text = ObjectWord[WordIndex];

            }
            else
            {
                WordIndex++;

                StartCoroutine(WordByWord(WordIndex));

            }
        }
    
    }
    

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) {
            
        //}
    }

    public void StartTalk() {
        if (ObjectWord.Count > 0)
        {
            gameObject.SetActive(true);
            WordIndex++;
            transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(WordByWord(WordIndex)); // 从第一个单词开始逐字显示
        }
    }


    private IEnumerator ReadJsonFile()
    {
        string uri = Path.Combine(Application.streamingAssetsPath, "Words.json");

        UnityWebRequest request = UnityWebRequest.Get(uri);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            StringList dataList = JsonUtility.FromJson<StringList>(json);

            foreach (var table in dataList.items)
            {
                ObjectWord.Add(table);
            }
        }
        else
        {
            Debug.LogError("Failed to load JSON: " + request.error);
        }

        gameObject.SetActive(false);
    }

    IEnumerator WordByWord(int order)
    {
        newState = Talking.Talking;

        // 检查 order 是否在有效范围内
        if (order >= ObjectWord.Count)
        {
            newState = Talking.NotTalking;

            transform.GetChild(0).gameObject.SetActive(false);
            yield break; // 退出协程

        }

        WorkLodng = true; // 开始逐字显示
        string old = ObjectWord[order]; // 获取要显示的单词

        if (old == "。") {
            newState = Talking.NotTalking;
            gameObject.SetActive(false);
            yield break; // 退出协程
        }

        Talk.text = ""; // 清空文本框

        for (int i = 0; i < old.Length; i++)
        {
            Talk.text += old[i]; // 逐字添加字符
            yield return new WaitForSeconds(WorlSpeed); // 等待指定的时间
        }

        WorkLodng = false; // 结束逐字显示
    }
}