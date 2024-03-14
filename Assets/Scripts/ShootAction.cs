using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : CooldownAction
{
    private int shotsFired;
    private Light glow;
    private LineRenderer beam;

    void Start()
    {

    }

    void Update()
    {

    }

    protected override void DoAwake()
    {
        target.SetActive(false); // turn off light and line renderer
        glow = target.GetComponent<Light>();
        beam = target.GetComponent<LineRenderer>();
    }

    protected override void Triggered()
    {
        shotsFired++;
        PlaySFX(actionSFX);
        target.SetActive(true); // turn light on
        beam.enabled = true; // turn beam on

        // convert the position of the blaster from its local coordinate space to world space
        var origin = transform.TransformPoint(Vector3.zero);
        // adjust the coordinate by lifting the blaster up on the y axis
        var adjOrigin = new Vector3(origin.x, target.transform.position.y, origin.z);
        var player = GameObject.FindGameObjectWithTag("Player");

        beam.SetPosition(0, adjOrigin); // set first point
        beam.SetPosition(1, player.transform.position); // set 2nd point

        iTween.ValueTo(target, iTween.Hash(
            "from", 5f,
            "to", .5f,
            "time", .4,
            "onupdate", "OnTweenUpdate",
            "onupdatetarget", gameObject,
            "oncomplete", "OnTweenComplete",
            "oncompletetarget", gameObject
        ));
    }

    void OnTweenUpdate(float value)
    {
        glow.range = value; // set to current tweened value

        if (value < 2f)
        {
            beam.enabled = false; // a small visual touch
        }
    }

    void OnTweenComplete()
    {
        target.SetActive(false); // turn off blaster object

        if (shotsFired < 3)
        {
            Triggered(); // shoot again
        }
        else
        {
            Reset();
            OnEnded.Invoke();
        }

    }
}
