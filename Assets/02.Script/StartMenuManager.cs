using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    [Header("튜토리얼 기본 UI")]
    public GameObject tutorialPanel; // 튜토리얼 전체 창

    [Header("튜토리얼 페이지들 (순서대로 넣어주세요)")]
    public GameObject[] tutorialPages; // 페이지를 담는 배열
    
    [Header("이전 / 다음 / 나가기 버튼 UI")]
    public GameObject prevButton; // 이전 버튼
    public GameObject nextButton; // 다음 버튼
    public GameObject exitButton; // 나가기 버튼

    private int PageIndex = 0; // 현재 몇 번째 페이지를 보고 있는지 저장하는 변수

    void Start()
    {
        SoundManagers.instance.PlaySFX(SoundManagers.instance.menuBGM);
        if (PlayerPrefs.GetInt("IsFirstTime", 0) == 0)
        {
            PageIndex = 0;
            if (tutorialPanel != null) tutorialPanel.SetActive(true);
            UpdatePage(); // 첫 페이지 세팅
        }
        else
        {
            PageIndex = 0;
            UpdatePage();
            if (tutorialPanel != null) tutorialPanel.SetActive(false);
        }
    }

    public void OpenTutorial()
    {
        PageIndex = 0;
        if (tutorialPanel != null) 
            tutorialPanel.SetActive(true);
        UpdatePage();
    }

    // 게임 시작
    public void StartGame()
    {
        SoundManagers.instance.PlaySFX(SoundManagers.instance.gameStartSFX);
        SceneManager.LoadScene("Stage1");
    }
    
    // 페이지 넘김 조작
    private void UpdatePage()
    {
        for (int i = 0; i < tutorialPages.Length; i++)
        {
            // 현재 보고 있는 번호(PageIndex)의 페이지면 활성화(true), 아니면 비활성화(false)
            tutorialPages[i].SetActive(i == PageIndex);
        }

        // 이전 버튼 : 0번째(첫 장)일 때는 이전 버튼을 숨김
        if (prevButton != null)
        {
            prevButton.SetActive(PageIndex > 0);
        }

        if (nextButton != null)
        {
            nextButton.SetActive(PageIndex < tutorialPages.Length - 1);
        }

        if (exitButton != null)
        {
            exitButton.SetActive(PageIndex == tutorialPages.Length - 1);
        }
    }
    
    // 다음 버튼을 눌렀을 때 실행될 함수
    public void NextPage()
    {
        SoundManagers.instance.PlaySFX(SoundManagers.instance.buttonClickSFX);
        
        // 아직 마지막 장이 아니라면 다음 장으로 이동
        if (PageIndex < tutorialPages.Length - 1)
        {
            PageIndex++;
            UpdatePage();
        }
    }
    
    // 이전 버튼을 눌렀을 때 실행될 함수
    public void PrevPage()
    {
        SoundManagers.instance.PlaySFX(SoundManagers.instance.buttonClickSFX);
        
        // 1번째 장보다 뒤에 있다면 이전 장으로 이동
        if (PageIndex > 0)
        {
            PageIndex--;
            UpdatePage();
        }
    }
    
    // 튜토리얼 닫기
    public void CloseTutorial()
    {
        SoundManagers.instance.PlaySFX(SoundManagers.instance.buttonClickSFX);
        
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        PlayerPrefs.SetInt("IsFirstTime", 1);
        PlayerPrefs.Save();
    }
}
