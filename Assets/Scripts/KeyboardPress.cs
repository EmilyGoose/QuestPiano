using System;
using System.Collections.Generic;
using Minis;

    public class KeyboardPress
    {
        private MidiNoteControl note;
        private DateTime pressTime;
        // semitones between current note and note before
        private int interval;

        public KeyboardPress(MidiNoteControl n)
        {
            note = n;
            pressTime = DateTime.Now;
            interval = 0;
        }

        public int GetInterval()
        {
            return interval;
        }

        public void SetInterval(KeyboardPress prev)
        {
            interval = prev.note.noteNumber - this.note.noteNumber;
        }
        
        public MidiNoteControl Note { get; set; }
    }