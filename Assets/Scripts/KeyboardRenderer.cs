using System;
using UnityEngine;

public class KeyboardRenderer : MonoBehaviour
{
    private GameObject cube;
    private GameObject pianoParent;
    public GameObject[] whiteKeys = new GameObject[52];
    public GameObject[] blackKeys = new GameObject[36];
    
    // Materials for keys
    public Material whiteKeyMaterial;
    public Material whiteKeyPressedMaterial;
    public Material blackKeyMaterial;

    void Start()
    {
        // Make materials
        whiteKeyMaterial = (Material)Resources.Load("White Piano Key", typeof(Material));
        whiteKeyPressedMaterial = (Material)Resources.Load("White Piano Key Pressed", typeof(Material));
        blackKeyMaterial = (Material)Resources.Load("Black Piano Key", typeof(Material));
        
        // Initiate parent GameObject for piano
        pianoParent = new GameObject("Piano");
        pianoParent.transform.position = new Vector3(0, 0, 0);

        // Create 52 white keys and parent them to GameObject
        for (int i = 0; i < 52; i++)
        {
            // Create the new key
            GameObject newWhiteKey = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            // x = wide, y = deep, z = long
            newWhiteKey.transform.localScale = new Vector3(0.0231F, 0.01F, 0.133F);
            
            // position time (keyboard scales along x axis)
            newWhiteKey.transform.position = new Vector3(0.0231F * i, 0, 0);
            
            // Apply the material
            newWhiteKey.GetComponent<MeshRenderer>().material = whiteKeyMaterial;
            
            // Parent cube to piano so they move together
            newWhiteKey.transform.parent = pianoParent.transform;
            
            // Add to the array
            whiteKeys[i] = newWhiteKey;

        }

        // Create 36 black keys and parent them to GameObject
        for (int i = 0; i < 36; i++)
        {
            // Create the new key
            GameObject newBlackKey = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            // x = wide, y = deep, z = long
            newBlackKey.transform.localScale = new Vector3(0.014F, 0.02F, 0.08F);
            
            if ((i % 5) == 0)
            {
                // position time (keyboard scales along x axis)
                newBlackKey.transform.position = new Vector3((0.1617F * (float)Math.Floor((i / 5F))) + 0.015F, 0.01F, 0.027F);
            }
            else if ((i % 5) == 1)
            {
                // position time (keyboard scales along x axis)
                newBlackKey.transform.position = new Vector3((float)((0.1617F * Math.Floor(i / 5F)) + 0.058F), 0.01F, 0.027F);
            }
            else if ((i % 5) == 2)
            {
                // position time (keyboard scales along x axis)
                newBlackKey.transform.position = new Vector3((float)((0.1617F * Math.Floor(i / 5F)) + 0.087F), 0.01F, 0.027F);
            }
            else if ((i % 5) == 3)
            {
                // position time (keyboard scales along x axis)
                newBlackKey.transform.position = new Vector3((float)((0.1617F * Math.Floor(i / 5F)) + 0.126F), 0.01F, 0.027F);
            }
            else if ((i % 5) == 4)
            {
                // position time (keyboard scales along x axis)
                newBlackKey.transform.position = new Vector3((float)((0.1617F * Math.Floor(i / 5F)) + 0.152F), 0.01F, 0.027F);
            }
            
            // Apply the material
            newBlackKey.GetComponent<MeshRenderer>().material = blackKeyMaterial;
            
            // Parent cube to piano so they move together
            newBlackKey.transform.parent = pianoParent.transform;
            
            // Add to the array
            blackKeys[i] = newBlackKey;

        }


        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0.5f, 0);
        cube.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        // Match piano position to controller
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
        pianoParent.transform.position = controllerPosition;
        
        // Rotate piano on y axis only
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LHand);
        float yRotation = controllerRotation.y;
        float whatTheHellIsW = controllerRotation.w;
        // offset by 25deg to be properly aligned
        Quaternion pianoRotation = new Quaternion(0F, yRotation, 0F, whatTheHellIsW);
        pianoParent.transform.rotation = pianoRotation;
    }
}