using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using PanicPinnacle.Match;
using TeamUtility.IO;

public class MockPregame : MonoBehaviour {

    public SuperTextMesh playerCountText;
    
	// Update is called once per frame
	void Update () {

        //check for players joining
        for(int i = 1; i < 4; i++)
        {
            if(!MatchManager.HasPlayer((PlayerID)i) && InputManager.GetButton("PrimaryAction", (PlayerID)i)){
                MatchManager.AddPlayer((PlayerID)i);
            }
        }

        //ui
        playerCountText.Text = "";
        foreach(PlayerID pid in MatchManager.ActivePlayers)
        {
            playerCountText.Text = playerCountText.text + "Player JOINED : " + pid + "\n";
        }
        

        //goto Round
        if(InputManager.GetButton("Start", PlayerID.One))
        {
            SceneManager.LoadScene("Debug Round 3");
        }

	}
}
