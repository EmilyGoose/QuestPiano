using Minis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


    public class KeyBuffer
    {
        private List<KeyboardPress> noteCounter = new List<KeyboardPress>();
            
        public void addKey(MidiNoteControl note)
        {
            noteCounter.Add(new KeyboardPress(note));
            Debug.Log("new note added");
            // array is two or more -> set interval
            // if (noteCounter.Count > 1)
            // {
            //     noteCounter[-1].SetInterval(noteCounter[-2]);
            // }
        }

        List<int> getIntervals()
        {
            // i like functional programming and i wish C# had more of this
            return noteCounter.Select(note => note.GetInterval()).ToList();
        }

        List<int> getNoteNumbers()
        {
            return noteCounter.Select((note => note.Note.noteNumber)).ToList();
        }

        double getNoteAverage()
        {
            return getNoteNumbers().Average();
        }

        int getNoteMax()
        {
            return getNoteNumbers().Max();
        }

        int getNoteMin()
        {
            return getNoteNumbers().Min();
        }
        
        public void processNotes()
        {
            // get averages
            Debug.Log(getIntervals());
            Debug.Log(getNoteNumbers());
            Debug.Log(noteCounter.Count);
            Debug.Log(getNoteAverage());
            Debug.Log(getNoteMax());
            Debug.Log(getNoteMin());
            
            // clear after all of that 
            noteCounter.Clear();
        }

    }