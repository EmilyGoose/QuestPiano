using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MidiHandler : MonoBehaviour
{
    public KeyboardRenderer keyboardRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Get keyboard renderer script
        keyboardRenderer = gameObject.GetComponent<KeyboardRenderer>();
        
        // Listen for MIDI device changes
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;

            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;

            midiDevice.onWillNoteOn += (note, velocity) =>
            {
                // Note that you can't use note.velocity because the state
                // hasn't been updated yet (as this is "will" event). The note
                // object is only useful to specify the target note (note
                // number, channel number, device name, etc.) Use the velocity
                // argument as an input note velocity.
                Debug.Log(string.Format(
                    "Note On #{0} ({1}) ({5}) vel:{2:0.00} ch:{3} dev:'{4}'",
                    note.noteNumber,
                    note.shortDisplayName,
                    velocity,
                    (note.device as Minis.MidiDevice)?.channel,
                    note.device.description.product,
                    note.displayName
                ));

                // White keys go from A0 to C8 with 2 char shortDisplayName
                if (note.shortDisplayName.Length == 2)
                {
                    int octavePos = "CDEFGAB".IndexOf(note.shortDisplayName[0]);
                    int octaveMultiplier = Int32.Parse(note.shortDisplayName[1].ToString());

                    // Find note (0-51 should be i hope)
                    int noteIndex = ((octaveMultiplier - 1) * 7) + octavePos + 2;

                    // A and B are the only ones in 0 lol
                    // Likewise for C8
                    if (note.shortDisplayName.Equals("A0"))
                    {
                        noteIndex = 0;
                    } else if (note.shortDisplayName.Equals("B0"))
                    {
                        noteIndex = 1;
                    } else if (note.shortDisplayName.Equals("C8"))
                    {
                        noteIndex = 51;
                    }

                    Debug.Log($"Note index {noteIndex}");
                    
                    keyboardRenderer.whiteKeys[noteIndex].GetComponent<MeshRenderer>().material =
                        keyboardRenderer.whiteKeyPressedMaterial;
                }
            };

            midiDevice.onWillNoteOff += (note) =>
            {
                Debug.Log(string.Format(
                    "Note Off #{0} ({1}) ch:{2} dev:'{3}'",
                    note.noteNumber,
                    note.shortDisplayName,
                    (note.device as Minis.MidiDevice)?.channel,
                    note.device.description.product
                ));

                // White keys go from A0 to C8 with 2 char shortDisplayName
                if (note.shortDisplayName.Length == 2)
                {
                    int octavePos = "CDEFGAB".IndexOf(note.shortDisplayName[0]);
                    int octaveMultiplier = Int32.Parse(note.shortDisplayName[1].ToString());

                    // Find note (0-52 should be i hope)
                    int noteIndex = ((octaveMultiplier - 1) * 7) + octavePos + 2;
                    
                    // A and B are the only ones in 0 lol
                    if (note.shortDisplayName.Equals("A0"))
                    {
                        noteIndex = 0;
                    } else if (note.shortDisplayName.Equals("B0"))
                    {
                        noteIndex = 1;
                    } else if (note.shortDisplayName.Equals("C8"))
                    {
                        noteIndex = 51;
                    }

                    keyboardRenderer.whiteKeys[noteIndex].GetComponent<MeshRenderer>().material =
                        keyboardRenderer.whiteKeyMaterial;
                }
            };
        };
    }
}