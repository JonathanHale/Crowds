using UnityEngine;
using System.Collections.Generic;
using System.Xml.Linq;

public class ReadFile
{
    public int maxAgents;

    public List<int> agentFrames = new List<int>(); //StartFrame,frameCount for each agent

    public List<float> distances = new List<float>(); //Distance between each frame for each agent

    public List<Vector3> agentCoords = new List<Vector3>(); //All coordinates for each agent
    public List<Vector3> deltas = new List<Vector3>(); //All delta(x,y,z) between each frame for each agent


    public void read(string input)
    {
        string[] split = input.Split(new char[] { '/', '\\' });

        //If only name of file is provided, assume it is in the assets folder.
        if (1 == split.GetLength(0))
        {
            input = input.Insert(0, "Assets/CrowdData/");
        }

        readXML(input);
    }

    //TODO: Make recursive
    private void readXML(string input)
    {
        int startFrame = 0, frame = 0;
        float x = 0, y = 0, z = 0;

        XElement xmlTree = XElement.Load(input);

        foreach (XElement node in xmlTree.Nodes())
        {
            if (node.Name.LocalName.Equals("agent"))
            {
                startFrame = int.MaxValue;

                foreach (XElement node2 in node.Nodes())
                {
                    //Start frame for each agent is in "initalConditions"
                    if (node2.Name.LocalName.Equals("initialConditions"))
                    {
                        maxAgents++;

                        foreach (XElement node3 in node2.Nodes())
                        {
                            if (node3.Name.LocalName.Equals("position"))
                            {
                                foreach (XElement node4 in node3.Nodes())
                                {
                                    switch (node4.Name.LocalName)
                                    {
                                        case "x":
                                        case "X":
                                            x = float.Parse(node4.Value);
                                            break;
                                        case "y":
                                        case "Y":
                                            y = float.Parse(node4.Value);
                                            break;
                                        case "z":
                                        case "Z":
                                            z = float.Parse(node4.Value);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                agentCoords.Add(new Vector3(x, y, z));
                            }

                        }
                    }
                    //All frame data is contained in "sim_real"
                    if (node2.Name.LocalName.Equals("sim_real"))
                    {
                        distances.Add(0.0f);

                        foreach (XElement node3 in node2.Elements())
                        {
                            if (node3.Name.LocalName.Equals("frame"))
                            {
                                foreach (XElement node4 in node3.Elements())
                                {
                                    switch (node4.Name.LocalName)
                                    {
                                        case "number":
                                            var temp = int.Parse(node4.Value);

                                            if (startFrame > temp)
                                            {
                                                startFrame = temp;
                                            }

                                            frame = temp + 1;
                                            break;
                                        case "x":
                                        case "X":
                                            x = float.Parse(node4.Value);
                                            break;
                                        case "y":
                                        case "Y":
                                            y = float.Parse(node4.Value);
                                            break;
                                        case "z":
                                        case "Z":
                                            z = float.Parse(node4.Value);
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                agentCoords.Add(new Vector3(x, y, z));
                            }
                        }

                        agentFrames.Add(startFrame);
                        agentFrames.Add(frame - startFrame);
                    }
                }
            }
        }

        var v3Temp = agentCoords[0];

        //calculate the distances and deltas for every frame
        foreach (var item in agentCoords)
        {
            deltas.Add(new Vector3(item.x - v3Temp.x, item.y - v3Temp.y, item.z - v3Temp.z));
            distances.Add(Vector3.Distance(item, v3Temp));
            v3Temp = item;
        }
    }
}
