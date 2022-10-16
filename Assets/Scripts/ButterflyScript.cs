using UnityEngine;

public class ButterflyScript : MonoBehaviour
{
    private long nextJump;
    private Vector3 targetPosition;
    private bool flyAround = true;
    private float speed;
    private bool scared = false;
    
    // set this in explorer
    public OVRCameraRig cameraRig;

    // Start is called before the first frame update
    void Start()
    {
        cameraRig = FindObjectOfType<OVRCameraRig>();
        SwitchTarget();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = gameObject.transform.position;
        if (!scared)
        {
            // Unit vector to target
            Vector3 dir = (targetPosition - position).normalized;

            // Move towards target
            position += dir * (speed * Time.deltaTime);
            gameObject.transform.position = position;

            gameObject.transform.rotation = Quaternion.LookRotation(dir);

            if (Time.time * 1000 >= nextJump || (targetPosition - position).magnitude < 0.1F)
            {
                SwitchTarget();
            }
        }
        else
        {
            // scared, run away
            Vector3 headsetPosition = cameraRig.centerEyeAnchor.position;
            // Unit vector away from player
            Vector3 dir = (position - headsetPosition).normalized;
            
            // Gotta go fast
            position += dir * (speed * Time.deltaTime);
            gameObject.transform.position = position;

            gameObject.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void SwitchTarget()
    {
        Vector3 headsetPosition = cameraRig.centerEyeAnchor.position;

        // Randomly determine when we next switch position
        nextJump = (long)(Random.Range(1000, 2500) + Time.time * 1000F);

        // Randomly offset from headset position by up to 3m
        targetPosition = headsetPosition + new Vector3(Random.Range(-300, 300) / 100F, Random.Range(5, 50) / 100F, Random.Range(-300, 300) / 100F);

        // Figure out what speed we want to get there at (2-3 m/s)
        speed = Random.Range(100, 200) / 100F;
    }

    public void toggleActive()
    {
        flyAround = !flyAround;

        gameObject.SetActive(flyAround);
    }

    public void RunAway()
    {
        scared = true;
    }
}