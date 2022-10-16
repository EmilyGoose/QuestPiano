using System;
using Minis;

public class KeyboardPress
    {
        // semitones between current note and note before
        private int _interval;

        public KeyboardPress(MidiNoteControl n)
        {
            NoteNumber = n.noteNumber;
            PressTime = DateTime.Now;
            _interval = 0;
        }
        
        // getters and setters start here
        public int NoteNumber { get; }

        public DateTime PressTime { get; }

        public int Interval
        {
            get => _interval;
            set => _interval = value;
        }

        public void SetInterval(KeyboardPress prev)
        {
            _interval = NoteNumber - prev.NoteNumber;
        }
    }