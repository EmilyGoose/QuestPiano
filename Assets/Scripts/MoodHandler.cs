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
    private const int SPAWN_COOLDOWN = 2;

    // Animals - Add in explorer
    public GameObject squirrel;
    public GameObject butterflyPrefab;
    public GameObject deer;
    public GameObject birdPrefab;

    // Vars for bird spawn/target - also in explorer
    public GameObject birdSpawn;

    // Keep track of butterflies to scare them
    private List<GameObject> butterflyList = new List<GameObject>();

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
        if (Time.time - _lastSpawn >= SPAWN_COOLDOWN)
        {
            _lastSpawn = Time.time;
            return true;
        }

        return false;
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
        else if ((0 <= intervals[^1] && intervals[^1] <= 2) &&
                 (0 <= intervals[^2] && intervals[^2] <= 2) &&
                 (0 <= intervals[^3] && intervals[^3] <= 2) &&
                 (0 <= intervals[^4] && intervals[^4] <= 2))
        {
            if (canSpawn())
            {
                Debug.Log("Plant");
            }
        }
        //Play five notes of ascending scale or glissando for rain
        else if ((0 >= intervals[^1] && intervals[^1] >= -2) &&
                 (0 >= intervals[^2] && intervals[^2] >= -2) &&
                 (0 >= intervals[^3] && intervals[^3] >= -2) &&
                 (0 >= intervals[^4] && intervals[^4] >= -2))
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
                GameObject newButterfly = Instantiate(butterflyPrefab, new Vector3(0, 2, 0), Quaternion.identity);
                butterflyList.Add(newButterfly);
            }
        }
        //Play cluster chords (five notes within 2 octaves within 0.1 seconds) to undo previous action
        else if ((noteNums.Max() <= 33) &&
                 (_keyBuffer.GetTimeInMillis(2) <= 250) && (_keyBuffer.GetTimeInMillis(3) <= 250) &&
                 (_keyBuffer.GetTimeInMillis(4) <= 250))
        {
            if (canSpawn())
            {
                Debug.Log("SCAREY");
                // Scare butterflies
                foreach (GameObject butterfly in butterflyList)
                {
                    butterfly.GetComponent<ButterflyScript>().RunAway();
                    // Debris time
                    Destroy(butterfly, 5F);
                }

                // Reset list to get rid of all the dead butterflies
                butterflyList = new List<GameObject>();
            }
        }
        //Play high/middle/low notes quickly/slowly in any order for different animals and weather
        else
        {
            if (noteNums.Min() >= 72)
            {
                if (canSpawn())
                {
                    Debug.Log("Bird");
                    // Make new bird prefab
                    GameObject bird = Instantiate(birdPrefab, birdSpawn.transform.position, Quaternion.identity);
                    // Kill bird after 10s
                    Destroy(bird, 10F);
                }
            }
            else if (noteNums.Max() < 60)
            {
                if (canSpawn())
                {
                    Debug.Log("Deer time");
                    deer.GetComponent<DeerMover>().DoALap();
                }
            }
            else
            {
                if (canSpawn())
                {
                    // squirrel go crazy
                    // middle notes quickly
                    Debug.Log("Squirrel");
                    squirrel.GetComponent<IdiotSquirrelMoveScript>().ChaChaSlide();
                }
            }
        }
    }
}