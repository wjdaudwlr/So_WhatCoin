using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private GameObject[] profileImages;

    

    int currentMenuNum;                 // 현재 메뉴 번호

    

    private void Start()
    {
       currentMenuNum = 0;

       menuButtons[currentMenuNum].Select();
       menePanels[currentMenuNum].SetActive(true);
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
        myInformationText.text = "<size=50> 이름 : " +
            string.Format("{0:n0}", GameManager.Instance.player.playerData.name) + "</size>\n\n<size=45> 돈 : " +
            string.Format("{0:n0}", GameManager.Instance.player.playerData.playerMoney) + "</size>\n\n터치 당 골드 : " +
            string.Format("{0:n0}", GameManager.Instance.player.playerData.clickMoney) + "\n초당 골드      : " +
            string.Format("{0:n0}", GameManager.Instance.player.playerData.automatcIncome);

        foreach (string key in GameManager.Instance.player.playerData.itemDict.Keys)
        {
            if (GameManager.Instance.player.playerData.itemDict[key])
            {
                Debug.Log(key);
                Item item = GameManager.Instance.itemMap[key];
                
                profileImages[item.number].SetActive(true);
            }
        }
    }
}
