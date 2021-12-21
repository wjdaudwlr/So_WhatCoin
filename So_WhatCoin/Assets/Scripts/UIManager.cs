using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject titlePanel;

    [Header("InGame UI")]
    [SerializeField]
    private GameObject inGamePanel;
    [SerializeField]
    private Button[] menuButtons;
    [SerializeField]
    private GameObject[] menePanels;
    [SerializeField]
    Queue<GameObject> UI_queue = new Queue<GameObject>();
    
    int currentMenuNum;                 // 현재 메뉴 번호

    private void Start()
    {
        StartCoroutine(TapToStart());

        currentMenuNum = 0;

        menuButtons[currentMenuNum].Select();
        menePanels[currentMenuNum].SetActive(true);
    }

    IEnumerator TapToStart()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameStart();

                yield break;
            }
            yield return null;
        }
    }

    private void GameStart()
    {
        titlePanel.SetActive(false);
        inGamePanel.SetActive(true);
    }

    public void MenuButtonClick(int clickButtonNum)
    {
        menePanels[currentMenuNum].gameObject.SetActive(false);
        menePanels[clickButtonNum].gameObject.SetActive(true);
        

        currentMenuNum = clickButtonNum;
    }

    public void UI_appear(GameObject obj)
    {
        obj.SetActive(true);
        UI_queue.Enqueue(obj);
    }
    public void UI_disappear()
    {
        GameObject obj = UI_queue.Dequeue();
        obj.SetActive(false);
    }
}
