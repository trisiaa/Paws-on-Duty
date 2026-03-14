using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalCutsceneTrigger : MonoBehaviour
{
    public string cutsceneScene = "FinalCutscene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(cutsceneScene);
        }
    }
}