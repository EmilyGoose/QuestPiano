using UnityEngine;

public class KeyboardRenderer : MonoBehaviour
{
    private GameObject cube;
    private GameObject pianoParent;
    public GameObject[] whiteKeys = new GameObject[52];
    
    // Materials for keys
    public Material whiteKeyMaterial;
    public Material whiteKeyPressedMaterial;

    void Start()
    {
        // Make materials
        whiteKeyMaterial = (Material)Resources.Load("White Piano Key", typeof(Material));
        whiteKeyPressedMaterial = (Material)Resources.Load("White Piano Key Pressed", typeof(Material));
        
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
            
            // Apply the material
            newKey.GetComponent<MeshRenderer>().material = whiteKeyMaterial;
            
            // Parent cube to piano so they move together
            newKey.transform.parent = pianoParent.transform;
            
            // Add to the array
            whiteKeys[i] = newKey;

        }


        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0.5f, 0);
        cube.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LHand);
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LHand);
        pianoParent.transform.position = controllerPosition;
        pianoParent.transform.rotation = controllerRotation;
    }
}