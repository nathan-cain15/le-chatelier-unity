using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{
    public GameObject gameController;
    public GameController gameControllerScript;

    public Button addAMoleculesButton;
    public Button deleteAMoleculesButton;
    public Button addBMoleculesButton;
    public Button deleteBMoleculesButton;
    public Button addCMoleculesButton;
    public Button deleteCMoleculesButton;
    public Button expandVolumeButton;
    public Button contractVolumeButton;
    public Button ReloadSceneButton;

    public GameObject leftVerticalWall;
    public GameObject rightVerticalWall;
    public GameObject bottomHorizontalWall;
    public GameObject topHorizontalWall;

    public int atomAmount = 100;
    private void Start()
    {
        addAMoleculesButton.onClick.AddListener(AddAMoleculesClicked);
        deleteAMoleculesButton.onClick.AddListener(DeleteAMoleculesClicked);
        addBMoleculesButton.onClick.AddListener(AddBMoleculesClicked);
        deleteBMoleculesButton.onClick.AddListener(DeleteBMoleculesClicked);
        addCMoleculesButton.onClick.AddListener(AddCMoleculesClicked);
        deleteCMoleculesButton.onClick.AddListener(DeleteCMoleculesClicked);
        expandVolumeButton.onClick.AddListener(ExpandVolumeClicked);
        contractVolumeButton.onClick.AddListener(ContractVolumeClicked);
        ReloadSceneButton.onClick.AddListener(ReloadSceneClicked);
        
        gameControllerScript = gameController.GetComponent<GameController>();

    }

    void AddAMoleculesClicked()
    {
        
        gameControllerScript.AddAtoms(atomAmount, "a atom", gameControllerScript.energy);
    }

    void DeleteAMoleculesClicked()
    {
        if (GameObject.FindGameObjectsWithTag("a atom").Length > atomAmount)
        {
            gameControllerScript.DeleteAtoms(atomAmount, "a atom");

        }
    }

    void AddBMoleculesClicked()
    {
        gameControllerScript.AddAtoms(atomAmount, "b atom", gameControllerScript.energy);
    }

    void DeleteBMoleculesClicked()
    {
        if (GameObject.FindGameObjectsWithTag("b atom").Length > atomAmount)
        {
            gameControllerScript.DeleteAtoms(atomAmount, "b atom");
        }
    }

    void AddCMoleculesClicked()
    {

        gameControllerScript.AddAtoms(atomAmount, "c atom", gameControllerScript.energy);
    }

    void DeleteCMoleculesClicked()
    {
        if (GameObject.FindGameObjectsWithTag("c atom").Length > atomAmount)
        {
            gameControllerScript.DeleteAtoms(atomAmount, "c atom");
        }
    }
    void ExpandVolumeClicked()
    {
        leftVerticalWall.transform.position +=  new Vector3(-1, 0);
        rightVerticalWall.transform.position += new Vector3(1, 0);
        bottomHorizontalWall.transform.position += new Vector3(0, -1);
        topHorizontalWall.transform.position += new Vector3(0, 1);

        leftVerticalWall.transform.localScale += new Vector3(0, 2);
        rightVerticalWall.transform.localScale += new Vector3(0, 2);
        bottomHorizontalWall.transform.localScale += new Vector3(2, 0);
        topHorizontalWall.transform.localScale += new Vector3(2, 0);

        gameControllerScript.bottomLeft += new Vector2(-1, -1);
        gameControllerScript.topRight += new Vector2(1, 1);

    }

    void ContractVolumeClicked()
    {
        leftVerticalWall.transform.position += new Vector3(1, 0);
        rightVerticalWall.transform.position += new Vector3(-1, 0);
        bottomHorizontalWall.transform.position += new Vector3(0, 1);
        topHorizontalWall.transform.position += new Vector3(0, -1);
        
        leftVerticalWall.transform.localScale += new Vector3(0, -2);
        rightVerticalWall.transform.localScale += new Vector3(0, -2);
        bottomHorizontalWall.transform.localScale += new Vector3(-2, 0);
        topHorizontalWall.transform.localScale += new Vector3(-2, 0);
        
        gameControllerScript.bottomLeft += new Vector2(1, 1);
        gameControllerScript.topRight += new Vector2(-1, -1);
    }

    void ReloadSceneClicked()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }
    
}