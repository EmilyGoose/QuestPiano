using Minis;
using System;
using System.Collections.Generic;
using System.Linq;

public class KeyBuffer
    {
        private List<KeyboardPress> _noteBuffer;
        private List<int> _noteNumbers;
        private List<int> _noteIntervals;
        private List<TimeSpan> _timeDifferences;

        public KeyBuffer()
        {
            _noteBuffer = new List<KeyboardPress>();
        }
        
        public void AddKey(MidiNoteControl note)
        {
            KeyboardPress newPress = new KeyboardPress(note);
            _noteBuffer.Add(newPress);
            
            // Debug.Log("new note added");
            // array is two or more -> set interval for the last 
            if (_noteBuffer.Count > 1)
            {
                _noteBuffer[^1].SetInterval(_noteBuffer[^2]);
            }
        }
        
        // getters and setters/generators
        public List<KeyboardPress> NoteBuffer => _noteBuffer;

        public List<int> NoteNumbers => _noteNumbers;

        public List<int> NoteIntervals => _noteIntervals;

        public List<TimeSpan> TimeDifferences => _timeDifferences;

        public int Count()
        {
            return _noteBuffer.Count;
        }

        // helper function, gets time in milliseconds because interval is 2 seconds
        public int GetTimeInMillis(int index)
        {
            return _timeDifferences[index].Milliseconds;
        }

        public void GenerateNoteNumbers()
        {
            _noteNumbers = _noteBuffer.Select(note => note.NoteNumber).ToList();
        }

        public void GenerateNoteIntervals()
        {
            _noteIntervals = _noteBuffer.Select(note => note.Interval).ToList();
        }

        public void GenerateTimeDifferences()
        {
            // clear just in case
            _timeDifferences.Clear();
            
            // set the first element to 0
            _timeDifferences[0] = TimeSpan.Zero;
            
            // the rest will actually be calculated
            for (var i = 1; i < 5; i++)
            {
                _timeDifferences[i] = _noteBuffer[i].PressTime - _noteBuffer[i - 1].PressTime;
            }
        }
        
        // actual helper functions
        public void CalculateArrays()
        {
            GenerateNoteIntervals();
            GenerateNoteNumbers();
            GenerateTimeDifferences();
        }

        public void ClearArrays()
        {
            _noteBuffer = new List<KeyboardPress>();
            _noteIntervals = new List<int>();
            _noteNumbers = new List<int>();
            _timeDifferences = new List<TimeSpan>();
        }

        // takes the first element out of the array, adds the newest to the end, 
        // then recalculates everything
        public KeyboardPress Enqueue(MidiNoteControl note)
        {
            KeyboardPress oldestNote = _noteBuffer[0];
            _noteBuffer = _noteBuffer.Skip(1).ToList();
            _noteBuffer[0].Interval = 0;
            AddKey(note);
            CalculateArrays();

            return oldestNote;
        }
    }