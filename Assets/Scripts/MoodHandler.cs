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
            hasPressedButton = true;
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
        // _keyBuffer.getNoteTimeDifferences(); // difference in DateTime of when the key was pressed (ms)
        // _keyBuffer.getNoteIntervals(); // semitones between the current and last note
        noteNums = _keyBuffer.getNoteNums();
        intervals = _keyBuffer.getNoteIntervals();
        timeDiffs = _keyBuffer.getNoteTimeDifferences();
        //Play first five notes of Twinkle Twinkle Little Star for night scene
        if ((intervals[intervals.Count - 1] == 2) && (intervals[intervals.Count - 2] == 0) &&
            (intervals[intervals.Count - 3] == 7) && (intervals[intervals.Count - 4] == 0))
        {
            //change to starry night scene
        }
        //Play first five notes of Edvard Grieg's Morning Mood for day scene
        else if ((intervals[intervals.Count - 1] == 2) && (intervals[intervals.Count - 2] == 2) &&
                 (intervals[intervals.Count - 3] == -2) && (intervals[intervals.Count - 4] == -2))
        {
            //change to day scene
        }
        //Play five notes of ascending scale or glissando to grow a plant
        else if ((0 <= intervals[intervals.Count - 1] && intervals[intervals.Count - 1] <= 4) &&
                 (0 <= intervals[intervals.Count - 2] && intervals[intervals.Count - 2] <= 4) &&
                 (0 <= intervals[intervals.Count - 3] && intervals[intervals.Count - 3] <= 4))
        {
            //grow a plant
        }
        //Play five notes of ascending scale or glissando for rain
        else if ((0 >= intervals[intervals.Count - 1] && intervals[intervals.Count - 1] >= -4) &&
                 (0 >= intervals[intervals.Count - 2] && intervals[intervals.Count - 2] >= -4) &&
                 (0 >= intervals[intervals.Count - 3] && intervals[intervals.Count - 3] >= -4))
        {
            //rain
        }
        //Play leaps of 1 octave or greater to get a butterfly
        else if (intervals.Max >= 12)
        {
            //butterfly flies around
        }
        //Play cluster chords (five notes within 2 octaves within 0.1 seconds) to undo previous action
        else if ((-12 <= intervals[intervals.Count - 1] && intervals[intervals.Count - 1] <= 12) &&
                 (-12 <= intervals[intervals.Count - 2] && intervals[intervals.Count - 2] <= 12) &&
                 (-12 <= intervals[intervals.Count - 3] && intervals[intervals.Count - 3] <= 12) &&
                 (timeDiffs[timeDiffs.Count - 1] <= 100) && (timeDiffs[timeDiffs.Count - 2] <= 100) &&
                 (timeDiffs[timeDiffs.Count - 3] <= 100))
        {
            //animal leaves or plant dies?
        }
        //Play high/middle/low notes quickly/slowly in any order for different animals and weather
        else
        {
            if (noteNums.Min >= 72)
            {
                if (timeDiffs.Max <= 250)
                {
                    //a bird crosses the screen
                }
                else
                {
                    //a cloud crosses the screen
                }
            }
            else if (noteNums.Max < 60)
            {
                if (timeDiffs.Max <= 500)
                {
                    //a fox crosses the screen
                }
                else
                {
                    //a deer crosses the screen
                }
            }
            else
            {
                if (timeDiffs.Max <= 200)
                {
                    //a squirrel crosses the screen
                }
                else
                {
                    //a rabbit crosses the screen
                }
            }
        }
        // after scripts are called/logic is done then clear array and listen for button press again
        _keyBuffer.clearNoteBuffer();   
        hasPressedButton = false;
    }
    
}
