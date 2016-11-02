using UnityEngine;
using System.Collections.Generic;
using System;

public class agent : ScriptableObject
{
    GameObject clone;

    OnStartUp os = new OnStartUp();

    Vector3[] coordinates;
    Vector3[] deltas;

    List<Vector3> catPoints = new List<Vector3>();
    List<Vector3> newCoords = new List<Vector3>();

    public int id;
    public bool isActive;

    int curFrame = 0;
    int scale = 1;

    private int startFrame = 0;
    private int duration = 0;

    float[] distances;

    public float alpha = 0.5f;

    public void setStart(int start)
    {
        startFrame = start;
    }

    public void setDur(int dur)
    {
        duration = dur;
    }

    public void setCoords(List<Vector3> coords)
    {
        coordinates = new Vector3[duration];
        coords.CopyTo(0, coordinates, 0, duration);

        for (int i = 0; i < coordinates.Length - 3; i++)
        {
            catmullRom(i);

            foreach (var item in catPoints)
            {
                newCoords.Add(item);
            }
        }

        duration = newCoords.Count;

        coordinates = new Vector3[duration];

        newCoords.CopyTo(coordinates);
    }

    public void setDeltas(List<Vector3> deltas)
    {
        this.deltas = new Vector3[duration];

        var v3Temp = coordinates[0];

        foreach (var item in coordinates)
        {
            deltas.Add(new Vector3(item.x - v3Temp.x, item.y - v3Temp.y, item.z - v3Temp.z));
        }
    }

    public void setDistance(List<float> distances)
    {
        this.distances = new float[duration];

        List<float> temp = new List<float>();

        var v3Temp = coordinates[0];
        foreach (var item in coordinates)
        {
            temp.Add(Vector3.Distance(item, v3Temp));
        }
        temp.CopyTo(0, this.distances, 0, duration);
    }

    public int getStart()
    {
        return startFrame;
    }

    public int getDur()
    {
        return duration;
    }

    public Vector3[] getCoords()
    {
        return coordinates;
    }

    public void create()
    {
        clone = Instantiate(Resources.Load("CrowdAgent", typeof(GameObject)), coordinates[curFrame] / scale, Quaternion.identity) as GameObject;

        clone.transform.position = coordinates[curFrame] / scale;

        isActive = true;
    }

    public void move()
    {
        Vector3 heading;
        Vector3 newDir;
        Vector3 relativePos;

        if (curFrame + 1 < duration)
        {
            heading = deltas[curFrame + 1];


                //Debug.Log(clone.transform.rotation);
                //newDir = Vector3.RotateTowards(clone.transform.forward, heading, step, 0.0F);

            newDir = coordinates[curFrame+1] - clone.transform.position;
            Quaternion Q = Quaternion.LookRotation(newDir);

            float dist = Vector3.Distance(coordinates[curFrame + 1], clone.transform.position);
            float step = 1.0f; //dist * Time.deltaTime;

            clone.transform.rotation = Quaternion.RotateTowards(clone.transform.rotation,Q,step);

                //Debug.Log(clone.transform.rotation);


            //Vector3 relativePos = target.position - transform.position;
            //Quaternion rotation = Quaternion.LookRotation(relativePos);
            //transform.rotation = rotation;

            //clone.transform.rotation = Quaternion.LookRotation(newDir);
            var anim = clone.GetComponent<Animator>();
            anim.SetFloat("Speed", step);
            anim.SetFloat("Heading", heading.x);

            Vector3 position;
            position = coordinates[curFrame++] / scale;
            clone.transform.position = Vector3.MoveTowards(clone.transform.position, position, step);
        }
        else {
            isActive = false;
            Destroy(clone);
        }
    }

    public void reset()
    {
        curFrame = 0;
    }

    void catmullRom(int i)
    {
        catPoints.Clear();

        Vector3 p0 = new Vector3(coordinates[i].x, coordinates[i].y, coordinates[i].z);
        Vector3 p1 = new Vector3(coordinates[i + 1].x, coordinates[i + 1].y, coordinates[i + 1].z);
        Vector3 p2 = new Vector3(coordinates[i + 2].x, coordinates[i + 2].y, coordinates[i + 2].z);
        Vector3 p3 = new Vector3(coordinates[i + 3].x, coordinates[i + 3].y, coordinates[i + 3].z);

        float t0 = 0.0f;
        float t1 = GetT(t0, p0, p1);
        float t2 = GetT(t1, p1, p2);
        float t3 = GetT(t2, p2, p3);

        for (float t = t1; t < t2; t += ((t2 - t1) / os.frameRate))
        {
            Vector3 A1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
            Vector3 A2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
            Vector3 A3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

            Vector3 B1 = (t2 - t) / (t2 - t0) * A1 + (t - t0) / (t2 - t0) * A2;
            Vector3 B2 = (t3 - t) / (t3 - t1) * A2 + (t - t1) / (t3 - t1) * A3;

            Vector3 C = (t2 - t) / (t2 - t1) * B1 + (t - t1) / (t2 - t1) * B2;

            catPoints.Add(C);
        }
    }

    float GetT(float t, Vector3 p0, Vector3 p1)
    {
        float a = (float)(Mathf.Pow((p1.x - p0.x), 2.0f) + Mathf.Pow((p1.y - p0.y), 2.0f) + Math.Pow(p1.z - p0.z, 2.0f));
        float b = Mathf.Pow(a, 0.5f);
        float c = Mathf.Pow(b, alpha);

        return (c + t);
    }
}
