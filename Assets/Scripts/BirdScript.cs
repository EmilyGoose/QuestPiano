
using UnityEngine;

public class BirdScript : MonoBehaviour
{

    private GameObject target;
    private const int SPEED = 8;
    
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("BirdTarget");
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 position = gameObject.transform.position;
        // Unit vector to target
        Vector3 dir = (target.transform.position - position).normalized;

        // Move towards target
        position += dir * (SPEED * Time.deltaTime);
        gameObject.transform.position = position;

        gameObject.transform.rotation = Quaternion.LookRotation(dir);
    }
}
