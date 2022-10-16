using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject animalPrefab;
    // Start is called before the first frame update
    /*void Start() {
       // Instantiate(animalPrefab)
    }*/

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) // only spawns animals when condition is true
        {
            Instantiate(animalPrefab, transform.position, Quaternion.identity); // animal object is instantitated at position of spawner and rotation null
        }    
    }
}
