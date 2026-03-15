using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject goodBoyImage;
    public float cutsceneTime = 5f;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(PlayCutscene());
        }
    }

    IEnumerator PlayCutscene()
    {
        goodBoyImage.SetActive(true);

        yield return new WaitForSeconds(cutsceneTime);

        SceneManager.LoadScene("Menu");
    }
}