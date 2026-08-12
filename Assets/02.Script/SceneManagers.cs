using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    [Header("모든 스테이지 씬 이름을 진행 순서대로 넣어주세요")]
    public string[] stageScenes; // 예: Stage1, Stage2, Stage3...

    [Header("메인 메뉴(로비) 씬 이름")]
    public string lobbySceneName = "MainMenu";

    // 🔗 [다음 스테이지] 버튼의 On Click()에 연결할 함수
    public void NextStage()
    {
        Time.timeScale = 1f; // 멈췄던 시간을 다시 흐르게 만듭니다!

        // 1. 현재 켜져 있는 씬의 이름을 가져옵니다.
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentIdx = -1;

        // 2. 배열에서 현재 내가 몇 번째 스테이지에 있는지 번호(인덱스)를 찾습니다.
        for (int i = 0; i < stageScenes.Length; i++)
        {
            if (stageScenes[i] == currentSceneName)
            {
                currentIdx = i;
                break;
            }
        }

        // 3. 다음 스테이지가 배열 안에 존재한다면 해당 씬을 로드합니다.
        if (currentIdx != -1 && currentIdx + 1 < stageScenes.Length)
        {
            SceneManager.LoadScene(stageScenes[currentIdx + 1]);
        }
        else
        {
            // 만약 마지막 스테이지를 깬 거라면 자동으로 로비로 보냅니다!
            Debug.Log("마지막 스테이지입니다! 메인 로비로 이동합니다.");
            SceneManager.LoadScene(lobbySceneName);
        }
    }

    // 🔗 [로비로 가기] 버튼의 On Click()에 연결할 함수
    public void GoToLobby()
    {
        Time.timeScale = 1f; // 멈췄던 시간을 다시 흐르게 만듭니다!
        SceneManager.LoadScene(lobbySceneName);
    }
}