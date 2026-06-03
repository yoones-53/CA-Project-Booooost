using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /*
    * 게임 리스타트 방식이 현재 씬 다시 불러오기 방식이기에
    * BGM도 끊기게 않도록 싱글톤 객체방식으로 구현하였다.
    * 또한 DontDestroyOnload로 씬 전환되어도 유지되도록 했으며,
    * 중복 생성된 음악은 Destroy로 제거하였다.
    */
    public static SoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            // 파괴X
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // 중복 생성 X
            Destroy(this.gameObject);
        }
    }
}