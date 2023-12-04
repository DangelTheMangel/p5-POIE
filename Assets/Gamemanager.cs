using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Gamemanager : MonoBehaviour
{
    [SerializeField]
    GameObject loadningScreen;
    [SerializeField]
    Slider loadningbar;
    public void startMiniGame(int index) {
        loadningScreen.SetActive(true);
        StartCoroutine(loadLevel(index));
    }

    IEnumerator loadLevel(int i) {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(i);
        while (!loadOperation.isDone)
        {
            float progressV = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadningbar.value = progressV;
            yield return null;
        }

    }
}
