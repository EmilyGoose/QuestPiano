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
        private List<TimeSpan> noteTimeDifferences;

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

        public List<TimeSpan> getNoteTimeDifferences()
        {
            return noteTimeDifferences;
        }

        List<int> calculateIntervals()
        {
            // i like functional programming and i wish C# had more of this
            return noteBuffer.Select(note => note.GetInterval()).ToList();
        }
        List<TimeSpan> calculateDifferences()
        {
            // i like functional programming and i wish C# had more of this
            List<DateTime> pressTimes = noteBuffer.Select(note => note.getPressTime()).ToList();
            List<TimeSpan> pressTimeDifferences = new List<TimeSpan>(); 
            
            // time difference between first element in array and nonexistent zeroth element is always 0
            pressTimeDifferences.Add(TimeSpan.Zero);
            
            for (int i = 1; i < pressTimes.Count; i++)
            {
                // the difference in time between the current element and the previous one
                pressTimeDifferences.Add(pressTimes[i] - pressTimes[i - 1]);
            }

            return pressTimeDifferences;
        }
        public List<int> calculateNoteNumbers()
        {
            Debug.Log("counting numbers");
            Debug.Log(noteBuffer.Count);
            return noteBuffer.Select(note => note.getNote().noteNumber).ToList();
        }

        public void clearNoteBuffer()
        {
            noteBuffer = new List<KeyboardPress>();
        }

        public void processNotes()
        {
            Debug.Log("notes processing");
            // get averages
            if (noteBuffer.Count != 0)
            {
                // set up arrays to be accessed
                noteNums = calculateNoteNumbers();
                noteIntervals = calculateIntervals();
                noteTimeDifferences = calculateDifferences();
            }
        }

    }