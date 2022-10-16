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

    // Spawn debounce
    private float _lastSpawn = 0;

    // Animals - Add in explorer
    public GameObject squirrel;
    public GameObject butterflyPrefab;

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

    bool canSpawn()
    {
        if (Time.time - _lastSpawn >= 3)
        {
            _lastSpawn = Time.time;
            return true;
        }
        else
        {
            return false;
        }
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
        TimeSpan[] timeDiffs = _keyBuffer.TimeDifferences;
        //Play first five notes of Twinkle Twinkle Little Star for night scene
        if ((intervals[^1] == 2) && (intervals[^2] == 0) &&
            (intervals[^3] == 7) && (intervals[^4] == 0))
        {
            if (canSpawn())
            {
                Debug.Log("Night");
            }
        }
        //Play first five notes of Edvard Grieg's Morning Mood for day scene
        else if ((intervals[^1] == 2) && (intervals[^2] == 2) &&
                 (intervals[^3] == -2) && (intervals[^4] == -2))
        {
            if (canSpawn())
            {
                Debug.Log("Day");
            }
        }
        //Play five notes of ascending scale or glissando to grow a plant
        else if ((0 <= intervals[^1] && intervals[^1] <= 4) &&
                 (0 <= intervals[^2] && intervals[^2] <= 4) &&
                 (0 <= intervals[^3] && intervals[^3] <= 4))
        {
            if (canSpawn())
            {
                Debug.Log("Plant");
            }
        }
        //Play five notes of ascending scale or glissando for rain
        else if ((0 >= intervals[^1] && intervals[^1] >= -4) &&
                 (0 >= intervals[^2] && intervals[^2] >= -4) &&
                 (0 >= intervals[^3] && intervals[^3] >= -4))
        {
            if (canSpawn())
            {
                Debug.Log("Rain");
            }
        }
        //Play leaps of 1 octave or greater to get a butterfly
        else if (intervals.Max() >= 12)
        {
            if (canSpawn())
            {
                Debug.Log("Butterfly");
                // Spawn butterfly
                Instantiate(butterflyPrefab, new Vector3(0, 2, 0), Quaternion.identity);
                
            }
        }
        //Play cluster chords (five notes within 2 octaves within 0.1 seconds) to undo previous action
        else if ((-12 <= intervals[^1] && intervals[^1] <= 12) &&
                 (-12 <= intervals[^2] && intervals[^2] <= 12) &&
                 (-12 <= intervals[^3] && intervals[^3] <= 12) &&
                 (_keyBuffer.GetTimeInMillis(4) <= 50) && (_keyBuffer.GetTimeInMillis(3) <= 50) &&
                 (_keyBuffer.GetTimeInMillis(4) <= 50))
        {
            if (canSpawn())
            {
                Debug.Log("SCAREY");
            }
        }
        //Play high/middle/low notes quickly/slowly in any order for different animals and weather
        else
        {
            if (noteNums.Min() >= 72)
            {
                if (timeDiffs.Max().Milliseconds <= 250)
                {
                    if (canSpawn())
                    {
                        Debug.Log("Bird");
                    }
                }
                else
                {
                    if (canSpawn())
                    {
                        Debug.Log("Cloud");
                    }
                }
            }
            else if (noteNums.Max() < 60)
            {
                if (timeDiffs.Max().Milliseconds <= 500)
                {
                    if (canSpawn())
                    {
                        Debug.Log("Fox time");
                    }
                }
                else
                {
                    if (canSpawn())
                    {
                        Debug.Log("Deer time");
                    }
                }
            }
            else
            {
                if (timeDiffs.Max().Milliseconds <= 200)
                {
                    if (canSpawn())
                    {
                        // squirrel go crazy
                        // middle notes quickly
                        Debug.Log("Squirrel");
                        squirrel.GetComponent<IdiotSquirrelMoveScript>().ChaChaSlide();
                    }
                }
                else
                {
                    if (canSpawn())
                    {
                        //a rabbit crosses the screen
                        Debug.Log("Rabbit time");
                    }
                }
            }
        }
    }
}