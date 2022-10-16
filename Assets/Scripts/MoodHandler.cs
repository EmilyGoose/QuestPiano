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
    private bool hasPressedButton;
    
    void Start()
    {
        _keyBuffer = new KeyBuffer();
        
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
        if (!hasPressedButton)
        {
            StartCoroutine(processMood());
        }
    }
    
    

    IEnumerator processMood()
    {
        // wait two seconds
        yield return new WaitForSecondsRealtime(2);
        
        // generate data arrays
        _keyBuffer.processNotes();
        
        // add logic here
        // you can use these getters
        // _keyBuffer.getNoteNums(); // the corresponding MIDI numbers
        // _keyBuffer.getNoteTimeDifferences(); // difference in DateTime of when the key was pressed
        // _keyBuffer.getNoteIntervals(); // semitones between the current and last note
        
        // after scripts are called/logic is done then clear array and listen for button press again
        _keyBuffer.clearNoteBuffer();   
        hasPressedButton = false;
    }
    
}
