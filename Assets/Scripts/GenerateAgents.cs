using UnityEngine;
using System.Collections.Generic;
using System;

public class GenerateAgents : MonoBehaviour
{
    int agentNum, maxAgents, frameCount = 0;

    List<int> agentFrames = new List<int>();            /*A list containing the start and end frames for each agent*/
    List<Vector3> agentCoords = new List<Vector3>();    /*A list containing all locations for all agents. Populated by reading a file*/
    List<agent> agentList = new List<agent>();          /*A list of agents*/
    List<Vector3> deltas = new List<Vector3>();
    List<float> distances = new List<float>();

    agent a;
    OnStartUp startUp;
    string inputPath;

    void Start()
    {
        startUp = GameObject.Find("Main Camera").GetComponent("OnStartUp") as OnStartUp;

        inputPath = startUp.inputPath;

        ReadFile rf = new ReadFile();
        rf.read(inputPath);

        maxAgents = rf.maxAgents;

        agentFrames = rf.agentFrames;
        agentCoords = rf.agentCoords;
        deltas = rf.deltas;
        distances = rf.distances;

        agentNum = 0;

        /* Create all of the agents from the specified file. */
        for (int count = 0; count < agentFrames.Count; count += 2)
        {
            a = ScriptableObject.CreateInstance<agent>();

            a.setStart(agentFrames[count]);
            a.setDur(agentFrames[count + 1]);
            a.setCoords(agentCoords);
            a.setDeltas(deltas);
            a.setDistance(distances);
            a.id = agentNum;

            agentList.Add(a);

            int range = agentFrames[count + 1] + 1;
            if (range < agentCoords.Count)
            {
                agentCoords.RemoveRange(0, range);
                deltas.RemoveRange(0, range);
                distances.RemoveRange(0, range);
            }
            agentNum++;
        }

        agentNum = 0;
        while (agentNum < maxAgents)
        {

            if (0 == agentList[agentNum].getStart())
            {
                agentList[agentNum].create();
            }
            agentNum++;
        }
    }

    /* Called once per frame.*/
    void Update()
    {
        frameCount++;

        createAgent();

        moveAgent();

        var finalFrame = agentList[agentList.Count - 1].getStart() + agentList[agentList.Count - 1].getDur();

        if (finalFrame < frameCount)
        {
            frameCount = 0;
            Application.Quit();
        }
    }

    /* Name: createAgent
     * Param: none
     * return: void
     *
     * Instantiates all the agents at their starting positions.
     */
    void createAgent()
    {
        agentNum = 0;
        for (int count = 0; count < agentFrames.Count; count += 2)
        {
            if (agentList[agentNum].getStart() == frameCount)
            {
                agentList[agentNum].create();
                agentList[agentNum].id = agentNum;
            }
            agentNum++;
        }
    }

    /* Name: moveAgent
     * Param: none
     * return: void
     *
     * Updates all of the agents' positions.
     */
    void moveAgent()
    {
        agentNum = 0;
        for (int count = 0; count < agentFrames.Count; count += 2)
        {
            if (agentList[agentNum].isActive)
            {
                agentList[agentNum].move();
            }
            agentNum++;
        }
    }
}

