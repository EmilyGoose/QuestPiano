using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject animalPrefab; // game object

    public void createAnimal(GameObject spawner){ // instantiates the object at a spawner
        Instantiate(animalPrefab, spawner.transform.position, Quaternion.identity);
    }

    public void deleteAnimal(){ // destroys the object when called
        Destroy(animalPrefab);
    }
}
