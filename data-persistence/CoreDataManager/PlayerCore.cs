using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerCore {

    /// Some example fields that PlayerCore can have
    /// ensure that the fields are serializable so that
    /// they show up in the Editor.
    public int playerID;

    public string userID;

    public string displayName;


    public PlayerCore()
    {
        playerID = 0;
        userID = "";
        displayName = "DISPLAY_NAME";
    }

}
