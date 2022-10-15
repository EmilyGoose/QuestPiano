using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Minis;
using Unity.Template.VR;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoodHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private KeyBuffer _keyBuffer;
    private Timer _intervalTimer;
    
    void Start()
    {
        _keyBuffer = new KeyBuffer();
        _intervalTimer = new Timer(2000);
        _intervalTimer.Start();
        _intervalTimer.Elapsed += _keyBuffer.processNotes;
        
        // Listen for MIDI device changes
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, velocity) =>
            {
                // Key pressed
                _keyBuffer.addKey(note);
            };

            midiDevice.onWillNoteOff += (note) =>
            {
                // Key released
                // nothing for now
            };
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
