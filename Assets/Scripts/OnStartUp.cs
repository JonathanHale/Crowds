using UnityEngine;
using System.Collections;
using System;

public class OnStartUp : MonoBehaviour {
    public int agentNum, maxAgents;
    public int frameRate = 14;

    public string inputPath, outputPath;

    CaptureVideo cv;
    //WindowManager win;

    void Awake() {
        Matrix4x4 cMatrix = Camera.main.worldToCameraMatrix;
        cv = GetComponent("CaptureVideo") as CaptureVideo;
        //win = GetComponent("WindowManager") as WindowManager;

        string[] test;
        string[] args = Environment.GetCommandLineArgs();

        if (Application.isEditor) {
            //inputPath = "pedxing-seq1-annot.idl";
        } else {
            try {
                for (int index = 1; index < args.Length; index++) {
                    switch (args[index]) {
                        //Input
                        case "-i":
                            inputPath = args[++index];

                            test = inputPath.Split(new char[] { '.' });
                            break;
                        //Output
                        case "-o":
                            outputPath = args[++index];
                            break;
                        //Frame rate
                        case "-f":
                            int.TryParse(args[++index], out frameRate);
                            break;
                        //Matrix4x4
                        case "-m":
                            for(int i = 0; i < 4; i++) {
                                test = args[++index].Split(new char[] { ',' });

                                test[0] = test[0].TrimStart('(');
                                test[3] = test[3].TrimEnd(')');

                                Vector4 v = new Vector4();

                                float.TryParse(test[0], out v.x);
                                float.TryParse(test[1], out v.y);
                                float.TryParse(test[2], out v.z);
                                float.TryParse(test[3], out v.w);

                                cMatrix.SetRow(i, v);
                            }
                            break;
                        default:
                            break;
                    }
                }
                cv.setFolder(outputPath);
            } catch (Exception e) {
                Debug.Log(e.Message);
            }
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;

        Camera.main.worldToCameraMatrix = cMatrix;

        cv.isCapturing = true;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
