using System;
using System.Collections.Generic;
using Minis;
using UnityEditor.Search;

public class KeyboardPress
    {
        private int _noteNumber;
        private DateTime _pressTime;
        // semitones between current note and note before
        private int _interval;

        public KeyboardPress(MidiNoteControl n)
        {
            _noteNumber = n.noteNumber;
            _pressTime = DateTime.Now;
            _interval = 0;
        }
        
        // getters and setters start here
        public int NoteNumber
        {
            get { return _noteNumber;  }
        }

        public DateTime PressTime
        {
            get { return _pressTime;  }
        }

        public int Interval
        {
            get { return _interval;  }
            set { _interval = value;  }
        }

        public void SetInterval(KeyboardPress prev)
        {
            _interval = this._noteNumber - prev.NoteNumber;
        }
    }