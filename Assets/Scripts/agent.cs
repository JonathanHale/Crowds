using UnityEngine;
using System.Collections.Generic;
using System;

public class agent : ScriptableObject {
    GameObject clone;

    Vector3[] coordinates;
    Vector3[] deltas;

    public bool isActive;

    int curFrame = 0;
    int scale = 10;

    private int startFrame = 0;
    private int duration = 0;

    public int id;

    float[] distances;

    public void setStart(int start)
    {
        startFrame = start;
    }

    public void setDur(int dur) {
        duration = dur;
    }

    public void setCoords(List<Vector3> coords) {
        coordinates = new Vector3[duration];
        coords.CopyTo(0, coordinates, 0, duration);
    }

    public void setDeltas(List<Vector3> deltas) {
        this.deltas = new Vector3[duration];
        deltas.CopyTo(0, this.deltas, 0, duration);
    }

    public void setDistance(List<float> distances) {
        this.distances = new float[duration];
        distances.CopyTo(0, this.distances, 0, duration);
    }

    public int getStart() {
        return startFrame;
    }

    public int getDur() {
        return duration;
    }

    public Vector3[] getCoords() {
        return coordinates;
    }

    public void create() {
        clone = Instantiate(Resources.Load("CrowdAgent", typeof(GameObject)), coordinates[curFrame]/scale, Quaternion.identity) as GameObject;

        clone.transform.position = coordinates[curFrame]/scale;

        isActive = true;
    }

    public void move() {
        Vector3 heading;

        if (curFrame + 1 < duration) {
            heading = deltas[curFrame+1];

            float step = distances[curFrame] / Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(clone.transform.forward, heading, step, 0.0F);

            clone.transform.rotation = Quaternion.LookRotation(newDir);
            var anim = clone.GetComponent<Animator>();
            anim.SetFloat("Speed", step);
            anim.SetFloat("Heading", heading.x);

            Vector3 position;
            position = coordinates[curFrame++]/scale;
            clone.transform.position = Vector3.MoveTowards(clone.transform.position, position, step);
        } else {
            isActive = false;
            Destroy(clone);
        }
    }

    public void reset() {
        curFrame = 0;
    }
}
