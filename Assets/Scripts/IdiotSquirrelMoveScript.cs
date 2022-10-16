using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdiotSquirrelMoveScript : MonoBehaviour
{
    
    // Vars for making squirrel cha cha slide
    private float elapsedMoveTime = 0F;
    // Copied this from manual pos in unity editor lol
    private Vector3 bottomPosition;
    // This too
    private Vector3 topPosition;
    
    // Tracks which way the squirrel is moving
    private bool goingUp = false;
    
    // Time to transform
    private float totalMoveTime = 3F;

    void Start()
    {
        bottomPosition = gameObject.transform.position;
        // y value thru trial and error dont ask
        topPosition = bottomPosition + new Vector3(0, 0.4F, 0);
        ChaChaSlide();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedMoveTime += Time.deltaTime/totalMoveTime;
        if (goingUp)
        {
            transform.position = Vector3.Lerp(bottomPosition, topPosition, elapsedMoveTime);
        }
        else
        {
            transform.position = Vector3.Lerp(topPosition, bottomPosition, elapsedMoveTime);
        }
    }

    public void ChaChaSlide()
    {
        // Flip direction
        goingUp = !goingUp;
        elapsedMoveTime = 0;
    }
}