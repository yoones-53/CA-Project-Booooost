using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class GameManager : MonoBehaviour
{
    /*
    * 점수 계산, 최고 점수 저장, 게임 난이도
    * 적생성, 게임 Pause, UI 활성화를 담당한다
    */

     // 게임 매니저 싱글톤 객체
    public static GameManager Instance { get; private set; }

    int highScore = 0;
    int score = 0;
    public int Score => score;

    public void RocketScore()
    {
        if (isGameOver) return;

        int newScore = (int)player.position.x;

        if (newScore < 0)
            newScore = 0;

        if (score >= newScore) return;

        score = newScore;
        scoreText.text = $"{score}km";
    }

    void Awake()
    {
        Instance = this;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    [SerializeField]
    Transform player;

    [SerializeField]
    Vector2 alienSpawnYRange = new Vector2(-6f, 304f); // y값 적 생성 범위

    [Header("Panel")]
    public GameObject gameOverPanel;
    public GameObject pausePanel;


    [Header("Text")]
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI highScoreText;
    

    [Header("Spawn")]
    [SerializeField]
    float unitsPerSpawn = 10f;     // 이동거리마다 스폰

    [SerializeField]
    float unitsPerLevel = 100f;     // 이동거리마다 난이도 증가
    
    [SerializeField]
    int spawnCount = 1;


    // 초깃값들
    float nextSpawnX = 10f;         // 다음 스폰 초깃값
    float nextLevelX = 100f;        // 다음 난이도증가 초깃값
    bool isGameOver = false;
    bool isPaused = false;
    
    void Start()
    {
        highScoreText.text = $"{highScore}km";
    }

    void Update()
    {
        if (isGameOver)
        {
            RestartInput();
            return;
        }

        RocketScore();
        LevelUp();
        SpawnAlien();
        
        // ESC 게임 일시정지
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    /* 
    * 게임오버시 게임오버 패널이 켜지며
    * 최고 기록보다 기존 기록이 높으면 최고기록 변경
    */ 
    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive (true);

        // 높은 점수 저장
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        highScoreText.text = $"{highScore}km";
    }

    /* 
    * 플레이어가 nextSpawnX (x축 10)거리 이동하면
    * 플레이어기준 랜덤 x축(20~30)앞, 지정된 y축 범위에서
    * 6가지 적들중 한 마리 랜덤 적 스폰
    */ 
    void SpawnAlien()
    {
        if (player.position.x < nextSpawnX) return;
        for (int i = 0; i < spawnCount; i++)
        {
            float spawnX = player.position.x + Random.Range(20f, 30f);
            float spawnY = Random.Range(alienSpawnYRange.x, alienSpawnYRange.y);
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
            
            GameObject alien = PoolManager.Instance.GetAlien(Random.Range(0, 6));
            alien.transform.position = spawnPosition;
        }   
        nextSpawnX += unitsPerSpawn;
    }

    /* 
    * x축 100마다 난이도 증가
    * x축 10마다 스폰시 적 1마리 더 증가
    * 최대 스폰 마리수 30으로 제한
    */ 
    void LevelUp()
    {
        if (player.position.x >= nextLevelX && spawnCount < 30)
        {
            spawnCount++;
            nextLevelX += unitsPerLevel;
        }
    }

    /* 
    * ESC게임 일시정지 패널 온오프
    * 일시정지때 게임 시간 정지
    */ 
    void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    /* 
    * 키보드 아무키 인풋 or 마우스 좌클릭
    * 게임오버 상태이면 리스타트
    */ 
    void RestartInput()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            } 
    }
}