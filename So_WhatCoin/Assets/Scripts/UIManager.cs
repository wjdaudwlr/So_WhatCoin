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
    [SerializeField]
    private Text myInformationText;

    int currentMenuNum;                 // ���� �޴� ��ȣ

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
        if (UI_queue.Count != 0) 
            UI_disappear();
        InitProfile();
        obj.SetActive(true);
        UI_queue.Enqueue(obj);
    }
    public void UI_disappear()
    {
        GameObject obj = UI_queue.Dequeue();
        obj.SetActive(false);
    }

    private void InitProfile()
    {
        myInformationText.text = "<size=50> �̸� : " + 
            string.Format("{0:n0}", GameManager.Instance.player.playerData.name) + "</size>\n\n<size=45> �� : " + 
            string.Format("{0:n0}", GameManager.Instance.player.playerData.playerMoney) + "</size>\n\n��ġ �� ��� : "+ 
            string.Format("{0:n0}", GameManager.Instance.player.playerData.clickMoney) + "\n�ʴ� ���      : " + 
            string.Format("{0:n0}", GameManager.Instance.player.playerData.automatcIncome); 
    }

}
