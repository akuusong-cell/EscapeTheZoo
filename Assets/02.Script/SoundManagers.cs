using UnityEngine;

public class SoundManagers : MonoBehaviour
{
    // 어디서나 SoundManager.instance로 접근할 수 있게 만듭니다 (싱글톤)
    public static SoundManagers instance;

    [Header("오디오 소스 컴포넌트")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("배경 음악 (BGM)")]
    public AudioClip menuBGM;
    public AudioClip stageBGM;

    [Header("효과음 (SFX)")]
    public AudioClip gameStartSFX;
    public AudioClip buttonClickSFX;
    public AudioClip jumpSFX;
    public AudioClip dashSFX;
    public AudioClip ramaSpitSFX;
    public AudioClip switchOnSFX;
    public AudioClip deathSFX;
    public AudioClip clearSFX;

    void Awake()
    {
        // 싱글톤 세팅 (파괴되지 않고 유지)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 🎵 BGM 재생 함수
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
    
        // 만약 같은 음악이 이미 돌고 있다면 무시
        if (bgmSource.clip == clip && bgmSource.isPlaying) return; 

        bgmSource.Stop();             // 1. 혹시 모를 오디오 버퍼 꼬임 방지를 위해 완벽히 정지
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.time = 0f;          // 2. 오디오 재생 타임라인을 0초로 강제 초기화 (핵심!)
    
        bgmSource.Play();             // 4. 새 출발 재생!
    }

    // 🎵 BGM 정지 함수
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void StopDashSound()
    {
        sfxSource.Stop();
    }

    // 🔊 효과음 단발성 재생 함수 (여러 소리가 겹쳐도 다 뚫고 나옴)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}

