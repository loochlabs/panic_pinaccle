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
            if(!MatchManager.HasPlayer((PlayerInputID)i) && InputManager.GetButton("PrimaryAction", (PlayerInputID)i)){
                MatchManager.AddPlayer((PlayerInputID)i);
            }
        }

        //ui
        playerCountText.Text = "";
        foreach(PlayerInputID pid in MatchManager.ActivePlayers)
        {
            playerCountText.Text = playerCountText.text + "Player JOINED : " + pid + "\n";
        }
        

        //goto Round
        if(InputManager.GetButton("Start", PlayerInputID.One))
        {
            SceneManager.LoadScene("Debug Round 3");
        }

	}
}
