using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.Security.Cryptography;

public class CoreDataManager : MonoBehaviour
{
    [SerializeField]
    bool debugMode = false;

	[SerializeField]
	bool hashingEnabled = true;

    private static CoreDataManager coreDataManager;

    public static CoreDataManager instance
    {
        get { return coreDataManager; }
    }
       
    public string userID;

    string userIDFilePath
    {
        get { return Application.persistentDataPath + "/" + dataDirectoryName + "/" + userID + ".json"; }
    }

    public string dataDirectoryName;

    HashedPlayerCore hashedCoreData;
    public PlayerCore coreData; 

    public System.Action PlayerDataLoaded;

    string rootPath
    {
        get
        {
            return Application.persistentDataPath + "/" + dataDirectoryName;
        }
    }

    #region Monobehaviour Functions
    void Awake()
    {
        if (!coreDataManager)
        {
            coreDataManager = FindObjectOfType<CoreDataManager>();
            if (coreDataManager)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        if (instance == null)
        {
            coreDataManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }   

    // Update is called once per frame
    void Update()
    {
		
    }

	private void Start()
	{
		LoadCurrentUser();
	}

	private void OnDisable()
    {
		SaveCurrentUser();
    }

    #endregion

    #region CoreData (Player) Functions

    void CreateNewPlayerCoreData()
    {
		ensureDirectoryExists(rootPath);
		string path = rootPath + "/" + userID + ".json";
        
        PlayerCore newData = new PlayerCore();
        newData.userID = userID;

		string jsonSaveFile;

		if (hashingEnabled)
		{
			HashedPlayerCore hashedData = new HashedPlayerCore();
			hashedData.playerData = newData;
            
			string resultHash = DataUtility.Md5Sum(JsonUtility.ToJson(hashedData.playerData, true));
			hashedData.hash = resultHash;

			jsonSaveFile = JsonUtility.ToJson(hashedData, true);
		} else {
			jsonSaveFile = JsonUtility.ToJson(newData, true);
		}

		File.WriteAllText(path, jsonSaveFile);
    }

    // Load methods

	void LoadCurrentUser()
    {
        LoadPlayerCoreData(userID);
    }

    public void LoadPlayerCoreData(string userID)
    {
        string path = Application.persistentDataPath + "/" + dataDirectoryName + "/" + userID + ".json";
        string json;

        try
        {
            json = File.ReadAllText(path);

            // Checks if the hashes match
			if (hashingEnabled)
			{
				hashedCoreData = JsonUtility.FromJson<HashedPlayerCore>(json);

				if (isHashedDataValid(hashedCoreData)) 
				{
					coreData = hashedCoreData.playerData;
				} 
				else 
				{
					CreateNewPlayerCoreData();
					return;
				}
			} 
			else 
			{
				coreData = JsonUtility.FromJson<PlayerCore>(json);
			}

			if (debugMode)
			{
				Debug.Log("Player " + coreData.userID + " has been successfully loaded! Json printed below: ");
				Debug.Log("-------------------------------------------------------------");
				Debug.Log(json);
			}
            
            if (PlayerDataLoaded != null)
                PlayerDataLoaded();
        }
        catch (IOException e)
        {
            print(e);
            print("No save file located!");
        }
    }

    // hash related methods

	bool isHashedDataValid(HashedPlayerCore playerCore) {
		
		string resultHash = DataUtility.Md5Sum(JsonUtility.ToJson(hashedCoreData.playerData, true));

		if (debugMode) {
			Debug.Log("Resultant hash is: " + resultHash);
		}

		if (resultHash == hashedCoreData.hash) 
		{
			if (debugMode)
                Debug.Log("Hashes match! Data is valid");
			return true;
		} 

		if (debugMode) 
		{
			Debug.Log("Hashes do NOT match. Data is invalid.");
		}

		return false;
    }   

    // Save methods

	void SaveCurrentUser()
    {
        SavePlayerCoreData(userID, true);
    }

    public void SavePlayerCoreData(string userID, bool autoReload = true)
    {
		ensureDirectoryExists(rootPath);

		string path = rootPath + "/" + userID + ".json";
        string coreDatajson = JsonUtility.ToJson(coreData, true);

		string resultJson;

		if (hashingEnabled)
		{
			string hash = DataUtility.Md5Sum(coreDatajson);
			HashedPlayerCore hashedCore = new HashedPlayerCore();
			hashedCore.hash = hash;
			hashedCore.playerData = coreData;
			resultJson = JsonUtility.ToJson(hashedCore, true);
		} 
        else 
		{
			resultJson = JsonUtility.ToJson(coreData, true);
		}

        File.WriteAllText(path, resultJson);

		if (debugMode)
		{
			Debug.Log("Saved Core Player Data to: " + path);
			print("SavedPlayerCoreData called. Results: " + resultJson);
		}

        if (autoReload)
        {
			JsonUtility.FromJsonOverwrite(resultJson, coreData);
        }
    }

	private void ensureDirectoryExists(string directoryName)
    {
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
    }
    #endregion

    #region Debug Functions

    void printPlayerCoreData()
    {
		if (Directory.Exists(userIDFilePath))
            Debug.Log(File.ReadAllText(userIDFilePath));
    }

    #endregion
}
