using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    [Header("Ui패널")] 
    public GameObject clearUI;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Finish"))
        {
            StageClear();
        }
    }

    void StageClear()
    {
        Debug.Log("클리어");
        
        SoundManagers.instance.StopBGM();
        SoundManagers.instance.PlaySFX(SoundManagers.instance.clearSFX);
        
        if (clearUI != null)
            clearUI.SetActive(true);
        
        Time.timeScale = 0;
    }
}
