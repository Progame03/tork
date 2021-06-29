using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Adrenak.Tork;

public class SimpleSoundController : VehicleAddOn
{
    [Tooltip("Physical Entity of the Car")]
    public Rigidbody rb;
    public Vehicle vehicle;
    [Tooltip("Audio Source of the Car")]
    public AudioSource engine;
    [Tooltip("Maximum Speed of the Vehicle")]
    public float maxspeed = 150;
    [Tooltip("Maximum Pitch of the Vehicle")]
    public float maxpitch = 1.5f;
    [Tooltip("Ratio RPM : Pitch")]
    public float RPMFactor = 1000;
    [Tooltip("If Engine RPM should be included into Sound Simulation")]
    public bool useDrivetrainRPM = true;
    [Tooltip("1 is the maximum Speed and Pitch, using the Curve you can easily simulate gear shifting")]
    public AnimationCurve pitchCurve;
    private float currpitch;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody>();
        vehicle = GetComponentInParent<Adrenak.Tork.Vehicle>();
    }

    private void OnValidate()
    {
        if(engine != null)
           engine.loop = true;

        if(pitchCurve.keys.Length < 2)
        {
            pitchCurve.AddKey(0, 0);
            pitchCurve.AddKey(1, 1);
        } 
    }

    void Update()
    {
        currpitch = (pitchCurve.Evaluate((Mathf.Abs(rb.velocity.z)/maxspeed))) * maxpitch * ((useDrivetrainRPM)?vehicle.Motor.RPM/RPMFactor:1);
        engine.pitch = currpitch;
    }
}
