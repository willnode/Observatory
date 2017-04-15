using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Text text;
    // Use this for initialization
    void Start ()
    {
        StartCoroutine(LoadNow());
    }

    IEnumerator LoadNow ()
    {
        var l = SceneManager.LoadSceneAsync (1);
        l.allowSceneActivation = true;
        while (!l.isDone) {
            text.text = string.Format ("Loading... ({0})", l.progress.ToString("P0"));
            yield return null;
        }
    }
}
