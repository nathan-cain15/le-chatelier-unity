using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public GameController gameControllerScript;
    
    public TextMeshProUGUI ratioText;
    public TextMeshProUGUI aATomCount;
    public TextMeshProUGUI bAtomCount;
    public TextMeshProUGUI cAtomCount;
    
    public List<float> lastTenEqValues = new List<float>(){0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

    public float timeAtLastUpdate = 0;

    public float amountOfAAtom;
    public float amountOfBAtom;
    public float amountOfCAtom;

    void UpdateRatio()
    {
        var numOfAAtoms = (float)GameObject.FindGameObjectsWithTag("a atom").Length;
        var numOfBAtoms = (float)GameObject.FindGameObjectsWithTag("b atom").Length;
        var numOfCAtoms = (float)GameObject.FindGameObjectsWithTag("c atom").Length;
        
        amountOfAAtom = numOfAAtoms;
        amountOfBAtom = numOfBAtoms;
        amountOfCAtom = numOfCAtoms;
        
        var ratio = numOfCAtoms * numOfCAtoms / (numOfBAtoms * numOfAAtoms);
        
        lastTenEqValues.Add(ratio);
        lastTenEqValues.RemoveAt(0);
        
        //ratioText.text = ratio.ToString(CultureInfo.CurrentCulture);
    }

    void UpdateRatioAverageOver()
    {
        var averagedRatio = lastTenEqValues.Sum() / 10f;
        
        ratioText.text = averagedRatio.ToString(CultureInfo.CurrentCulture);

        //Debug.Log(averagedRatio);
    }

    void UpdateAtomAmountText()
    {
        aATomCount.text = "[" + amountOfAAtom.ToString(CultureInfo.CurrentCulture) + "]";
        bAtomCount.text = "[" + amountOfBAtom.ToString(CultureInfo.CurrentCulture) + "]";
        cAtomCount.text = "[" + amountOfCAtom.ToString(CultureInfo.CurrentCulture) + "]^2";
    }

    
    private void Start()
    {
        aATomCount.color = gameControllerScript.AtomColorDict["a atom"];
        bAtomCount.color = gameControllerScript.AtomColorDict["b atom"];
        cAtomCount.color = gameControllerScript.AtomColorDict["c atom"];

    }

    private void Update()
    {
        if (Time.fixedTime - timeAtLastUpdate >= 0.2)
        {
            UpdateRatio();
            UpdateAtomAmountText();
            UpdateRatioAverageOver();
            timeAtLastUpdate = Time.fixedTime;
        }
       
    }

  
    
}