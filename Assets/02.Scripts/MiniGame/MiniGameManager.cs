using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    private static MiniGameManager instance;
    public static MiniGameManager Instance {  get { return instance; } }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        else
            instance = this;
    }
}
