using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles population of the lanes every time a new game or round starts
/// </summary>
public class OldLaneController : MonoBehaviour
{
    // Slow moving obstacle
    public GameObject slowObstacle;
    // Fast moving obstacle
    public GameObject fastObstacle;
    // Standard obstacle
    public GameObject normalObstacle;
    // Second variation of a standard obstacle
    public GameObject normalObstacleTwo;
    // Fastest obstacle
    public GameObject smoothObstacle;
    // Platform one
    public GameObject platformOne;
    // Platform two
    public GameObject platformTwo;
    // Platform three
    public GameObject platformThree;
    // Platform four
    public GameObject platformFour;
    // Platform five
    public GameObject platformFive;
    // Vertical position of the first obstacle lane one on the Y axis
    private int obstacleFirstLanePosition = -1;
    // Vertical position of the last obstacle lane on the Y axis
    private int obstacleLastLanePosition = -5;
    // Vertical position of the first platform lane on the Y axis
    private int platformFirstLanePosition = 1;
    // Vertical position of the last platform lane on the Y axis
    private int platformLastLanePosition = 5;
    // Horizontal maximum position for obstacle or platform lane
    private float laneSize;
    // Variable to calculate the width of the lane
    private float currLaneDist;
    // Integer used to select random object type from the object type list
    private int randomLaneTypeSelection;
    // Number of objects in the lane
    private int lanePop;
    // Horizontal position on the X axis of the next object in lane to be placed
    private float laneNextPos;
    // List of obstacles to be spawned
    private List<GameObject> obstacleList;
    // List of platforms to be spawned
    private List<GameObject> platformList;
    // Boolean flag for obstacles for generation purposes
    private bool obstacleFlag = true;
    // Boolean flag for platforms for generation purposes
    private bool platformFlag = false;
    // Current game object that is being placed in the lane
    private GameObject currLaneObj;
    // Instance of the Move Cycle script used by the currently placed game object
    private MoveCycle currObjMoveCycle;
    // Instance of the unique game object that is used to populate the lanes
    private readonly List<GameObject> instantObj = new();
    // Vector representative of the left screen border
    [SerializeField] private Vector3 leftEdge;
    // Vector representative of the right screen border
    [SerializeField] private Vector3 rightEdge;

    /// <summary>
    /// Invokes method to fill lanes with obstacles and platforms
    /// </summary>
    public void FillLanes()
    {
        CalculateLaneSize();
        CreateLists();
        GenerateObjects(obstacleList, obstacleFirstLanePosition, obstacleLastLanePosition, obstacleFlag);
        GenerateObjects(platformList, platformFirstLanePosition, platformLastLanePosition, platformFlag);
    }

    /// <summary>
    /// Calculates lane size from the positions of the left and right edges of the camera view on the X axis
    /// </summary>
    private void CalculateLaneSize()
    {
        laneSize = System.Math.Abs(leftEdge.x) + System.Math.Abs(rightEdge.x);
    }

    /// <summary>
    /// Populate lists 
    /// </summary>
    private void CreateLists()
    {
        obstacleList = new List<GameObject>(5)
        {
            slowObstacle,
            fastObstacle,
            normalObstacle,
            normalObstacleTwo,
            smoothObstacle
        };

        platformList = new List<GameObject>(5)
        {
            platformOne,
            platformTwo,
            platformThree,
            platformFour,
            platformFive
        };
    }

    /// <summary>
    /// Invokes methods to either create obstacles or platforms depending on the boolean flag
    /// </summary>
    private void GenerateObjects(List<GameObject> laneList, int firstLane, int lastLane, bool obstacle)
    {
        var objList = laneList;

        if (obstacle)
        {
            // Loop that goes through each obstacle lane and creates obstacle objects in them
            for (var i = firstLane; i >= lastLane; i--)
            {
                Debug.Log("Populating obstacles");
                PopulateLane(objList, i, true);
            }
        }
        else
        {
            // Loop that goes through each platform lane and creates platform objects in them
            for (var i = firstLane; i <= lastLane; i++)
            {
                Debug.Log("Populating platforms");
                PopulateLane(objList, i, false);
            }
        }
    }

    /// <summary>
    /// Selects a random object from the object list, removes the object from the object list, and creates the objects 
    /// </summary>
    private void PopulateLane(List<GameObject> objList, int lane, bool obstacle)
    {
        Debug.Log("Size of the object list: " + objList.Count);
        randomLaneTypeSelection = Random.Range(0, objList.Count);
        Debug.Log("Random Lane Selected:" + randomLaneTypeSelection);
        currLaneObj = objList[randomLaneTypeSelection];
        currObjMoveCycle = currLaneObj.GetComponent<MoveCycle>();

        objList.Remove(currLaneObj);

        // Check if creating an obstacle or platform
        if (obstacle)
        {
            // Check if obstacle is large or small
            // If obstacle is large then fewer objects are generated
            lanePop = Random.Range(2, currObjMoveCycle.size > 1 ? 3 : 5);
        } else
        {
            // Check if platform is large
            // If platform is large then fewer platforms are generated
            lanePop = currObjMoveCycle.size >= 6 ? Random.Range(2, 3) : Random.Range(3, 4);
        }

        CreateObject(lane, currObjMoveCycle.size, lanePop, currObjMoveCycle.direction.x);
    }

    /// <summary>
    /// Fills the lane with instances of the selected object
    /// </summary>
    private void CreateObject(int laneHeight, float objectLength, int objectCount, float moveDirection)
    {
        var maxLaneSize = laneSize + objectLength;
        float sizeModifier = 2;

        // Depending on the movement direction start creating objects from the opposite side
        if (moveDirection > 0)
        {
            laneNextPos = leftEdge.x - (objectLength / sizeModifier);
        }
        else
        {
            laneNextPos = rightEdge.x + (objectLength / sizeModifier);
        }

        currLaneDist = maxLaneSize / objectCount;

        // Create clones of the object and move the pointer for the next object
        for (int j = 0; j < lanePop; j++)
        {
            GameObject clone = Instantiate(currLaneObj, new Vector3(laneNextPos, laneHeight), Quaternion.identity);
            instantObj.Add(clone);
            
            // If object is moving to the right then place next object to the right, if the object is moving to the left then place the object on the left
            if (moveDirection > 0)
            {
                laneNextPos += currLaneDist;
            }
            else
            {
                laneNextPos -= currLaneDist;
            }
        }
    }

    /// <summary>
    /// Method used to delete objects between rounds
    /// </summary>
    public void EndGame()
    {
        // Delete every clone that was created
        foreach (GameObject obj in instantObj)
        {
            Destroy(obj);
        }

        instantObj.Clear();
    }
}
