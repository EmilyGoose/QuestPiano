using UnityEngine;

public class TrackerTest : MonoBehaviour
{
    private GameObject cube;
    private GameObject pianoParent;
    private GameObject[] whiteKeys = new GameObject[52];

    void Start()
    {
        // Initiate parent GameObject for piano
        pianoParent = new GameObject("Piano");
        pianoParent.transform.position = new Vector3(0, 0, 0);

        // Create 52 white keys and parent them to GameObject
        for (int i = 0; i < 52; i++)
        {
            // Create the new key
            GameObject newKey = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            // x = wide, y = deep, z = long
            newKey.transform.localScale = new Vector3(0.0231F, 0.01F, 0.133F);
            
            // position time (keyboard scales along x axis)
            newKey.transform.position = new Vector3(0.0231F * i, 0, 0);
            
            // Parent cube to piano so they move together
            newKey.transform.parent = pianoParent.transform;

        }


        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0.5f, 0);
        cube.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
        cube.transform.position = controllerPosition;
    }
}