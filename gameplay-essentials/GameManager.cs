using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {

    static object _lock = new object();

    private static GameManager _gameManager;

    public static GameManager instance
    {
        get
        {
            if (_gameManager == null)
            {
                lock (_lock)
                    if (_gameManager == null)
                        _gameManager = new GameManager();
            }
            return _gameManager;
        }
    }

    #region Monobehavior Functions

    private void Awake()
    {
        if (!_gameManager)
        {
            _gameManager = FindObjectOfType<GameManager>();
            if (_gameManager)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        if (instance == null)
        {
            _gameManager = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            if (instance != this)
                Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

}
