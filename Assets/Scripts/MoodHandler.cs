using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoodHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private KeyBuffer _keyBuffer;
    private bool _isListening;
    private float _timer;
    
    // Animals - Add in explorer
    public GameObject squirrel;

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
        if ((intervals[^1] == 2) && (intervals[^2] == 0) &&
            (intervals[^3] == 7) && (intervals[^4] == 0))
        {
            Debug.Log("Night");
        }
        //Play first five notes of Edvard Grieg's Morning Mood for day scene
        else if ((intervals[^1] == 2) && (intervals[^2] == 2) &&
                 (intervals[^3] == -2) && (intervals[^4] == -2))
        {
            Debug.Log("Day");
        }
        //Play five notes of ascending scale or glissando to grow a plant
        else if ((0 <= intervals[^1] && intervals[^1] <= 4) &&
                 (0 <= intervals[^2] && intervals[^2] <= 4) &&
                 (0 <= intervals[^3] && intervals[^3] <= 4))
        {
            Debug.Log("Plant");
        }
        //Play five notes of ascending scale or glissando for rain
        else if ((0 >= intervals[^1] && intervals[^1] >= -4) &&
                 (0 >= intervals[^2] && intervals[^2] >= -4) &&
                 (0 >= intervals[^3] && intervals[^3] >= -4))
        {
            Debug.Log("Rain");
        }
        //Play leaps of 1 octave or greater to get a butterfly
        else if (intervals.Max() >= 12)
        {
            Debug.Log("Butterfly");
        }
        //Play cluster chords (five notes within 2 octaves within 0.1 seconds) to undo previous action
        else if ((-12 <= intervals[^1] && intervals[^1] <= 12) &&
                 (-12 <= intervals[^2] && intervals[^2] <= 12) &&
                 (-12 <= intervals[^3] && intervals[^3] <= 12) &&
                 (_keyBuffer.GetTimeInMillis(timeDiffs.Count - 1) <= 100) && (_keyBuffer.GetTimeInMillis(timeDiffs.Count - 2) <= 100) &&
                 (_keyBuffer.GetTimeInMillis(timeDiffs.Count - 3) <= 100))
        {
            Debug.Log("SCAREY");
        }
        //Play high/middle/low notes quickly/slowly in any order for different animals and weather
        else
        {
            if (noteNums.Min() >= 72)
            {
                if (timeDiffs.Max().Milliseconds <= 250)
                {
                    Debug.Log("Bird");
                }
                else
                {
                    Debug.Log("Cloud");
                }
            }
            else if (noteNums.Max() < 60)
            {
                if (timeDiffs.Max().Milliseconds <= 500)
                {
                    Debug.Log("Fox time");
                }
                else
                {
                    Debug.Log("Deer time");
                }
            }
            else
            {
                if (timeDiffs.Max().Milliseconds <= 200)
                {
                    // squirrel go crazy
                    // middle notes quickly
                    squirrel.GetComponent<IdiotSquirrelMoveScript>().ChaChaSlide();
                }
                else
                {
                    //a rabbit crosses the screen
                    Debug.Log("Rabbit time");
                }
            }
        }
    }
}
