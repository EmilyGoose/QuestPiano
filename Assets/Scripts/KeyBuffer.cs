using Minis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;


    public class KeyBuffer
    {
        private List<KeyboardPress> noteBuffer;
        private List<int> noteNums;
        private List<int> noteIntervals;
        private List<DateTime> noteTimes;

        public KeyBuffer()
        {
            noteBuffer = new List<KeyboardPress>();
        }
        
        public void addKey(MidiNoteControl note)
        {
            KeyboardPress newPress = new KeyboardPress(note);
            noteBuffer.Add(newPress);
            // Debug.Log("new note added");
            // array is two or more -> set interval
            if (noteBuffer.Count > 1)
            {
                noteBuffer[noteBuffer.Count - 1].SetInterval(noteBuffer[noteBuffer.Count - 2]);
            }
        }

        public List<int> getNoteNums()
        {
            return noteNums;
        }

        public List<int> getNoteIntervals()
        {
            return noteIntervals;
        }

        public List<DateTime> getNoteTimes()
        {
            return noteTimes;
        }

        List<int> getIntervals()
        {
            // i like functional programming and i wish C# had more of this
            return noteBuffer.Select(note => note.GetInterval()).ToList();
        }
        List<DateTime> getTimestamps()
        {
            // i like functional programming and i wish C# had more of this
            return noteBuffer.Select(note => note.getPressTime()).ToList();
        }
        public List<int> getNoteNumbers()
        {
            Debug.Log("counting numbers");
            Debug.Log(noteBuffer.Count);
            return noteBuffer.Select(note => note.getNote().noteNumber).ToList();
        }

        public double getNoteAverage()
        {
            return noteNums.Average();
        }

        public int getNoteMax()
        {
            return noteNums.Max();
        }

        public int getNoteMin()
        {
            return noteNums.Min();
        }
        
        public void processNotes()
        {
            Debug.Log("notes processing");
            // get averages
            if (noteBuffer.Count != 0)
            {
                // set up arrays to be accessed
                noteNums = getNoteNumbers();
                noteIntervals = getIntervals();
                noteTimes = getTimestamps();

                // clear after all of that 
                noteBuffer = new List<KeyboardPress>();
            }
        }

    }