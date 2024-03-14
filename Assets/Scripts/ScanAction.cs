using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanAction : CooldownAction
{
    public float turnTime = 1f; // seconds to make 1/4 circle turn

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Top");
    }

    protected override void DoUpdate()
    {
        RaycastHit hit; // object to store collision info

        if (Physics.Raycast(
            target.transform.position,
            target.transform.TransformDirection(Vector3.forward),
            out hit,
            Mathf.Infinity))
        {
            if (!(hit.collider.gameObject.tag == "Player")) return;

            iTween.StopByName("turning"); // interrupt tween, stop turning
            Reset();
            OnEnded.Invoke();
        }
    }

    protected override void Triggered()
    {
        PlaySFX(actionSFX);

        iTween.RotateBy(target, iTween.Hash(
            "name", "turning",
            "y", .25, // quarter turn, 25% of circle
            "time", turnTime,
            "easeType", "easeOutBack"
        ));
    }

    protected override void Ready()
    {
        // when cooled down, re-triggr (so, continuously scan)
        Trigger();
    }
}
