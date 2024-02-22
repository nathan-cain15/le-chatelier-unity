using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public GameObject wall;
    public GameObject atom;
    public GameObject molecule;
    public GameObject buttonLogic;
    public GameObject atomDisplay;

    public Vector2 bottomLeft;
    public Vector2 topRight;

    public int energy = 15;
    public bool calculated = false;
    public List<Atom> testAtomList = new List<Atom>();

    public Dictionary<string, float> AtomMassDict = new Dictionary<string, float>
    {
        { "a atom", 1.25f },
        { "b atom", 1.25f },
        { "c atom", 1.5f }
    };

    public Dictionary<string, Color> AtomColorDict = new Dictionary<string, Color>()
    {
        { "a atom", Color.magenta },
        { "b atom", Color.green },
        { "c atom", Color.yellow }
    };

    public Dictionary<string, float> AtomSizeDict = new Dictionary<string, float>()
    {
        { "a atom", 2f },
        { "b atom", 2f },
        { "c atom", 2.5f }
    };

    public float averageEnergyValue;
    public float lastTimeEnergyUpdate = 0;
    public float lastTimeAverageEnergyUpdate = 0;


    // Start is called before the first frame update

    public void SpawnWalls()
    {
        var height = topRight.y - bottomLeft.y;
        var length = topRight.x - bottomLeft.x;

        var leftVerticalWall =
            Instantiate(wall, new Vector3(bottomLeft.x, bottomLeft.y + height / 2), Quaternion.identity);
        leftVerticalWall.transform.localScale = new Vector3(4, height + 4);
        var rightVerticalWall =
            Instantiate(wall, new Vector3(topRight.x, bottomLeft.y + height / 2), Quaternion.identity);
        rightVerticalWall.transform.localScale = new Vector3(4, height + 4);
        var bottomHorizontalWall =
            Instantiate(wall, new Vector3(bottomLeft.x + length / 2, bottomLeft.y), Quaternion.identity);
        bottomHorizontalWall.transform.localScale = new Vector3(length, 4);
        var topHorizontalWall =
            Instantiate(wall, new Vector3(bottomLeft.x + length / 2, topRight.y), Quaternion.identity);
        topHorizontalWall.transform.localScale = new Vector3(length, 4);

        buttonLogic.GetComponent<ButtonLogic>().leftVerticalWall = leftVerticalWall;
        buttonLogic.GetComponent<ButtonLogic>().rightVerticalWall = rightVerticalWall;
        buttonLogic.GetComponent<ButtonLogic>().topHorizontalWall = topHorizontalWall;
        buttonLogic.GetComponent<ButtonLogic>().bottomHorizontalWall = bottomHorizontalWall;

    }

    public void SpawnDisplayAtoms()
    {
        var aAtomText = GameObject.FindGameObjectWithTag("a atom text");
        
        var aAtom = Instantiate(atomDisplay, aAtomText.transform.position, quaternion.identity);
        aAtom.GetComponent<SpriteRenderer>().color = Color.magenta;
        

        var bAtom = Instantiate(atomDisplay, new Vector3(-32f, 68.04f), quaternion.identity);
        bAtom.GetComponent<SpriteRenderer>().color = Color.green;

        var cAtom = Instantiate(atomDisplay, new Vector3(2.48f, 68.04f), quaternion.identity);
        cAtom.GetComponent<SpriteRenderer>().color = Color.yellow;
        
    }

    public void AddAtoms(int amount, string atomName, float energy)
    {
        for (int i = 0; i < amount; i++)
        {
            var randX = Random.Range(bottomLeft.x, topRight.x);
            var randY = Random.Range(bottomLeft.y, topRight.y);
            var createdAtom = Instantiate(atom, new Vector3(randX, randY), quaternion.identity);
            createdAtom.name = atomName;

            createdAtom.GetComponent<Rigidbody2D>().mass = AtomMassDict[atomName];
            createdAtom.AddComponent<Atom>();
            createdAtom.GetComponent<Atom>().energy = energy;
            createdAtom.GetComponent<Atom>().gameController = gameObject.GetComponent<GameController>();

            createdAtom.GetComponent<SpriteRenderer>().color = AtomColorDict[atomName];
            createdAtom.transform.localScale = new Vector3(AtomSizeDict[atomName], AtomSizeDict[atomName]);

            createdAtom.tag = atomName;

            Vector2 randVec = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            createdAtom.GetComponent<Rigidbody2D>().AddForce(randVec * energy, ForceMode2D.Impulse);
            testAtomList.Add(createdAtom.GetComponent<Atom>());
        }
    }

    public void SpawnMolecules(int amount, string moleculeName, float energy)
    {
        for (int i = 0; i < amount; i++)
        {
            var randX = Random.Range(bottomLeft.x, topRight.x);
            var randY = Random.Range(bottomLeft.y, topRight.y);
            var createdMolecule = Instantiate(molecule, new Vector3(randX, randY), quaternion.identity);
            createdMolecule.name = moleculeName;

            //createdAtom.GetComponent<Rigidbody2D>().mass = AtomMassDict[atomName];
            createdMolecule.AddComponent<Atom>();
            createdMolecule.GetComponent<Atom>().energy = energy;

            Vector2 randVec = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            createdMolecule.GetComponentInChildren<Rigidbody2D>().AddForce(randVec * energy);
        }
    }

    public void DeleteAtoms(int amount, string atomName)
    {
        var aAtoms = GameObject.FindGameObjectsWithTag(atomName);

        for (int i = 0; i < amount - 1; i++)
        {
            Destroy(aAtoms[i]);
        }

    }

    public void CalculateAverageEnergy()
    {
        var aAtoms = GameObject.FindGameObjectsWithTag("a atom");
        var bAtoms = GameObject.FindGameObjectsWithTag("b atom");
        var cAtoms = GameObject.FindGameObjectsWithTag("c atom");

        var energyTotal = 0f;

        foreach (var atom in aAtoms)
        {
            energyTotal += 0.5f * AtomMassDict["a atom"] * atom.GetComponent<Rigidbody2D>().velocity.sqrMagnitude;
        }
        foreach (var atom in bAtoms)
        {
            energyTotal += 0.5f * AtomMassDict["b atom"] * atom.GetComponent<Rigidbody2D>().velocity.sqrMagnitude;
        }
        foreach (var atom in cAtoms)
        {
            energyTotal += 0.5f * AtomMassDict["c atom"] * atom.GetComponent<Rigidbody2D>().velocity.sqrMagnitude;
        }

        var amountOfAtoms = aAtoms.Length + bAtoms.Length + cAtoms.Length;
        var averageEnergy = energyTotal / amountOfAtoms;

        averageEnergyValue = averageEnergy;
    }

    public void RemoveEnergy(float energyAmount)
    {
        var aAtoms = GameObject.FindGameObjectsWithTag("a atom");
        var bAtoms = GameObject.FindGameObjectsWithTag("b atom");
        var cAtoms = GameObject.FindGameObjectsWithTag("c atom");
        
        var amountOfAtoms = aAtoms.Length + bAtoms.Length + cAtoms.Length;
        var averageEnergyToRemove = energyAmount / amountOfAtoms;
        
        
        foreach (var atom in aAtoms)
        {
            atom.GetComponent<Rigidbody2D>().velocity = ChangeVelocityBasedOnEnergyChange(atom, AtomMassDict["a atom"], averageEnergyToRemove );
        }

        foreach (var atom in bAtoms)
        {
            atom.GetComponent<Rigidbody2D>().velocity = ChangeVelocityBasedOnEnergyChange(atom, AtomMassDict["b atom"], averageEnergyToRemove );
        }

        foreach (var atom in cAtoms)
        {
            atom.GetComponent<Rigidbody2D>().velocity = ChangeVelocityBasedOnEnergyChange(atom, AtomMassDict["c atom"], averageEnergyToRemove );
        }
    }

    public Vector2 ChangeVelocityBasedOnEnergyChange(GameObject atom, float mass, float energyToRemove)
    {
        var atomVelocity = atom.GetComponent<Rigidbody2D>().velocity;
        
        var atomEnergyAmount = 0.5f * mass * atomVelocity.sqrMagnitude;

        if (atomEnergyAmount - energyToRemove <= 0)
        {
            return atomVelocity;
        }
            
        var targetEnergy = atomEnergyAmount - energyToRemove;
        var targetVelocitySquared = 2 * targetEnergy /mass;

        var velocityFraction =
            Math.Sqrt(targetVelocitySquared / atomVelocity.sqrMagnitude);
    
        
        return new Vector2((float)(atomVelocity.x * velocityFraction),
            (float)(atomVelocity.y * velocityFraction));
    }

    public void ConvertToAAtom(GameObject atomGameObject)
    {
        atomGameObject.GetComponent<Rigidbody2D>().velocity = ConservedEnergyVelocity(AtomMassDict["a atom"], atomGameObject.GetComponent<Rigidbody2D>().mass,  atomGameObject.GetComponent<Rigidbody2D>().velocity);

        
        atomGameObject.transform.localScale = new Vector3(AtomSizeDict["a atom"], AtomSizeDict["a atom"]);
        atomGameObject.GetComponent<Rigidbody2D>().mass = AtomMassDict["a atom"];
        atomGameObject.GetComponent<SpriteRenderer>().color = AtomColorDict["a atom"];
        atomGameObject.tag = "a atom";
        atomGameObject.GetComponent<Atom>().atomType = "a atom";

    }

    public void ConvertToBAtom(GameObject atomGameObject)
    {
        atomGameObject.GetComponent<Rigidbody2D>().velocity = ConservedEnergyVelocity(AtomMassDict["b atom"], atomGameObject.GetComponent<Rigidbody2D>().mass,  atomGameObject.GetComponent<Rigidbody2D>().velocity);

        
        atomGameObject.transform.localScale = new Vector3(AtomSizeDict["b atom"], AtomSizeDict["b atom"]);
        atomGameObject.GetComponent<Rigidbody2D>().mass = AtomMassDict["b atom"];
        atomGameObject.GetComponent<SpriteRenderer>().color = AtomColorDict["b atom"];
        atomGameObject.tag = "b atom";
        atomGameObject.GetComponent<Atom>().atomType = "b atom";

        
    }

    public void ConvertToCAtom(GameObject atomGameObject)
    {
        atomGameObject.GetComponent<Rigidbody2D>().velocity = ConservedEnergyVelocity(AtomMassDict["c atom"], atomGameObject.GetComponent<Rigidbody2D>().mass,  atomGameObject.GetComponent<Rigidbody2D>().velocity);
        
        atomGameObject.transform.localScale = new Vector3(AtomSizeDict["c atom"], AtomSizeDict["c atom"]);
        atomGameObject.GetComponent<Rigidbody2D>().mass = AtomMassDict["c atom"];
        atomGameObject.GetComponent<SpriteRenderer>().color = AtomColorDict["c atom"];
        atomGameObject.tag = "c atom";
        atomGameObject.GetComponent<Atom>().atomType = "c atom";
        
        
    }

    public Vector2 ConservedEnergyVelocity(float newMass, float oldMass, Vector2 oldVelocity)
    {
        var newVelMagSquared = oldMass * oldVelocity.sqrMagnitude / newMass;
        var fraction = Math.Sqrt(newVelMagSquared / oldVelocity.sqrMagnitude);
        Vector2 newVelocity = new Vector2((float)(oldVelocity.x * fraction), (float)(oldVelocity.y * fraction));

        return newVelocity;
    }

    void Start()
    {
        SpawnWalls();
        // AddAtoms(500, "a atom", energy);
        // AddAtoms(500, "b atom", energy);
        AddAtoms(1000, "c atom", energy);
        
        //Debug.Log(CalculateTotalEnergy());
    }


    // Update is called once per frame
    void Update()
    {
        
        if (Time.fixedTime - lastTimeEnergyUpdate >= 5f)
        {
            RemoveEnergy(400f);
            
            lastTimeEnergyUpdate = Time.fixedTime;
        }

        if (Time.fixedTime - lastTimeAverageEnergyUpdate >= 1f)
        {
            CalculateAverageEnergy();

            lastTimeAverageEnergyUpdate = Time.fixedTime;
        }
        
    }
}