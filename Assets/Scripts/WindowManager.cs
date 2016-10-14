using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindowManager : MonoBehaviour {
    Rect errWin;
    bool err = false;

    List<int> errCodes = new List<int>();

    public void addCode(int errCode) {
        errCodes.Add(errCode);
    }


    void OnGUI() {
        if (err) {
            errWin = GUI.Window(0, new Rect((Screen.width - 375) / 2, (Screen.height - 300) / 2, 375, 100), ErrorWindow, "The Following Errors Occurred");
        }
    }

    void ErrorWindow(int windowID) {
        int y = 25;

        foreach (var item in errCodes) {
            switch (item) {
                case 1:
                    GUI.Label(new Rect(5, y, errWin.width - 10, 20), "Too Few Arguments");
                    break;
                case 2:
                    GUI.Label(new Rect(5, y, errWin.width - 10, 20), "Input must be a sim File");
                    break;
                case 3:
                    GUI.Label(new Rect(5, y, errWin.width - 10, 20), "Starting frame cannot be less than 0");
                    break;
                case 4:
                    GUI.Label(new Rect(5, y, errWin.width - 10, 20), "End frame cannot be less than 0");
                    break;
                case 5:
                    GUI.Label(new Rect(5, y, errWin.width - 10, 20), "Framerate must be greater than 0");
                    break;
                case 6:
                    GUI.Label(new Rect(5, y, errWin.width - 10, 20), "Start Frame cannot be greater than End Frame");
                    break;
                default:
                    break;
            }
            y += 25;
        }

        if (GUI.Button(new Rect(5, errWin.height - 25, errWin.width - 10, 20), "Exit Application")) {
            Application.Quit();
        }
    }

	// Update is called once per frame
	void Update () {
        if (errCodes.Count == 0) {
            err = false;
        } else {
            err = true;
        }
    }
}
