using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    private static MiniGameManager instance;
    public static MiniGameManager Instance {  get { return instance; } }

    private GameObject miniGameCanvas;
    private MiniGameController[] miniGames = new MiniGameController[2];

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;

        Init();
    }

    public void Init()
    {
        miniGameCanvas = GameObject.Find("MiniGameCanvas").gameObject;
        miniGames[0] = miniGameCanvas.transform.GetChild(0).GetComponent<MiniGameController>();
        miniGames[1] = miniGameCanvas.transform.GetChild(1).GetComponent<MiniGameController>();
    }

    public void OnMiniGameStart()
    {
        miniGames[Random.Range(0, miniGames.Length)].MiniGameStart();
    }
}
