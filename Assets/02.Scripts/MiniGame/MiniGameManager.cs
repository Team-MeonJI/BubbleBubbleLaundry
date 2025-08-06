using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    private static MiniGameManager instance;
    public static MiniGameManager Instance {  get { return instance; } }

    public GameObject currentMiniGameCustomer;
    private GameObject miniGameCanvas;
    private MiniGameController[] miniGames = new MiniGameController[2];

    public bool isMiniGameOver = false;

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

    public void OnMiniGameStart(GameObject _customer)
    {
        miniGames[Random.Range(0, miniGames.Length)].MiniGameStart();
        currentMiniGameCustomer = _customer;
        isMiniGameOver = true;
    }

    public void OnMiniGameEnd(bool isGameSuccess)
    {
        CustomerManager.Instance.CoroutineHandler(currentMiniGameCustomer.GetComponent<CustomerBehaviour>().lineIndex, null, null, isGameSuccess ? 1 : 2);
        currentMiniGameCustomer = null;
        isMiniGameOver = false;
    }
}
