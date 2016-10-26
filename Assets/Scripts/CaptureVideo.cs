using UnityEngine;
using System.IO;
using System.Collections;

public class CaptureVideo : MonoBehaviour {
    private string folder;
    public bool isCapturing = false;

    OnStartUp startUp;

    void start()
    {
        startUp = GameObject.Find("Main Camera").GetComponent("OnStartUp") as OnStartUp;
        Time.captureFramerate = startUp.frameRate;
    }

    public void setFolder(string name) {
        folder = name;

        if (folder.Equals("")) {
            folder = Application.dataPath + "/Screenshots";
        }

        if (!Directory.Exists(folder)) {
            Directory.CreateDirectory(folder);
        }
    }

    // Update is called once per frame
    void Update() {
        if (!Application.isEditor && isCapturing) {
            string name = string.Format("{0}/frame_{1:D04}.png", folder, Time.frameCount);
            Application.CaptureScreenshot(name);
        }
    }
}
