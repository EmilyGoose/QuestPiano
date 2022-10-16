using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private float _timer;

    void Start()
    {
        _keyBuffer = new KeyBuffer();
        _isListening = false;
        _timer = 0.0f; 
        
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
        if (!_isListening) return;
        _timer += Time.deltaTime;
        // check if timer has reached 2 seconds
        if (!(_timer > 2f)) return;
        // if so, stop listening for input
        _isListening = false;
        // and clear all arrays
        _keyBuffer.ClearArrays();
    }



    void ProcessMood()
    {
        // call after 6 keys 
        // have it calculate things and then pop out first command
        // _keyBuffer.NoteIntervals; // distance in semitones from current - previous [1-4]
        // _keyBuffer.NoteNumbers; // the MIDI number of notes [1-4]
        // _keyBuffer.GetTimeInMillis(int index); // time interval between presses, in milliseconds
        List<int> noteNums = _keyBuffer.NoteNumbers;
        List<int> intervals = _keyBuffer.NoteIntervals;
        List<TimeSpan> timeDiffs = _keyBuffer.TimeDifferences;
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
        else if (intervals.Max() >= 12)
        {
            //butterfly flies around
        }
        //Play cluster chords (five notes within 2 octaves within 0.1 seconds) to undo previous action
        else if ((-12 <= intervals[intervals.Count - 1] && intervals[intervals.Count - 1] <= 12) &&
                 (-12 <= intervals[intervals.Count - 2] && intervals[intervals.Count - 2] <= 12) &&
                 (-12 <= intervals[intervals.Count - 3] && intervals[intervals.Count - 3] <= 12) &&
                 (_keyBuffer.GetTimeInMillis(timeDiffs.Count - 1) <= 100) && (_keyBuffer.GetTimeInMillis(timeDiffs.Count - 2) <= 100) &&
                 (_keyBuffer.GetTimeInMillis(timeDiffs.Count - 3) <= 100))
        {
            //animal leaves or plant dies?
        }
        //Play high/middle/low notes quickly/slowly in any order for different animals and weather
        else
        {
            if (noteNums.Min() >= 72)
            {
                if (timeDiffs.Max().Milliseconds <= 250)
                {
                    //a bird crosses the screen
                }
                else
                {
                    //a cloud crosses the screen
                }
            }
            else if (noteNums.Max() < 60)
            {
                if (timeDiffs.Max().Milliseconds <= 500)
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
                if (timeDiffs.Max().Milliseconds <= 200)
                {
                    //a squirrel crosses the screen
                }
                else
                {
                    //a rabbit crosses the screen
                }
            }
        }
    }
}
