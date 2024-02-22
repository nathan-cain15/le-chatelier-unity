using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Atom : MonoBehaviour
{
    public GameController gameController;
    
    public float energy;
    public string atomType = " ";
    public bool hasChanged = false;

    public GameObject lastContact;
    
    
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("wall") || collision.gameObject == lastContact)
       {
           return;
       }
       
       var otherAtomType = collision.gameObject.GetComponent<Atom>().atomType;

       var firstObjectEnergy = gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude *
                               gameObject.GetComponent<Rigidbody2D>().mass;
       var secondObjectEnergy = collision.gameObject.GetComponent<Rigidbody2D>().velocity.sqrMagnitude *
                                collision.gameObject.GetComponent<Rigidbody2D>().mass;
       
       if (firstObjectEnergy + secondObjectEnergy < gameController.averageEnergyValue * 2)
       {
           return;
       }

       if ((atomType == "a atom" && otherAtomType == "b atom") || atomType == "b atom" && otherAtomType == "a atom")
        {
            gameController.ConvertToCAtom(gameObject);
            gameController.ConvertToCAtom(collision.gameObject);
            
            lastContact = collision.gameObject;
            collision.gameObject.GetComponent<Atom>().lastContact = gameObject;

        }
        else if (atomType == "c atom" && otherAtomType == "c atom")
        {
            gameController.ConvertToAAtom(gameObject);
            gameController.ConvertToBAtom(collision.gameObject);
            
            lastContact = collision.gameObject;
            collision.gameObject.GetComponent<Atom>().lastContact = gameObject;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        atomType = gameObject.tag;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Equals(gameController.testAtomList[0]))
        // {
        //     Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity.magnitude);
        // }
    }
}
