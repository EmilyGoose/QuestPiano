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
    private bool _isListening;
    private float _timer = 0.0f;

    void Start()
    {
        _keyBuffer = new KeyBuffer();
        
        // Listen for MIDI device changes
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, _) =>
            {
                // On key press:
                // if less than 5 keys, just regularly add a key
                // else, start actively listening and calculating statistic arrays
                if (!_isListening) 
                {
                    _keyBuffer.AddKey(note);
                }
                else
                {
                    _timer = 0.0f;
                    _keyBuffer.Enqueue(note);
                    ProcessMood();
                }
                
                // only happens once, initial change
                if (_keyBuffer.Count() != 5 || _isListening) return;
                _timer = 0.0f;
                _isListening = true;
                _keyBuffer.CalculateArrays();
                ProcessMood();
            };
        };
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        // check if timer has reached 2 seconds
        if (_timer > 2f)
        {
            // if so, stop listening for input
            _isListening = false;
            // and clear all arrays
            _keyBuffer.ClearArrays();
        }
    }
    
    

    void ProcessMood()
    {
        // call after 6 keys 
        // have it calculate things and then pop out first command
        // _keyBuffer.NoteIntervals; // distance in semitones from current - previous [1-4]
        // _keyBuffer.NoteNumbers; // the MIDI number of notes [1-4]
        // _keyBuffer.GetTimeInMillis(int index); // time interval between presses, in milliseconds
    }
    
}
