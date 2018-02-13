using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TeamUtility.IO;


public class MockMainMenu : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
	    if(InputManager.GetButton("Start"))
        {
            SceneManager.LoadScene("MOCK Pregame");
        }
	}
}
