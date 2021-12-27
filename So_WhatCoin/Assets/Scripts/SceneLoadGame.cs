using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneLoadGame : MonoBehaviour
{
    public void GameStart()
    {
        LoadingSceneController.LoadScene("GameScene");
    }
}
