using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScript : MonoBehaviour {

	private MouseController player;

	public GameObject[] availableRooms;
	public GameObject[] availableRooms2;

	public List<GameObject> currentRooms;

	private float screenWidthInPoints;
	public GameObject[] availableObjects;    
	public List<GameObject> objects;

	public float objectsMinDistance = 5.0f;    
	public float objectsMaxDistance = 9.0f;

	public float objectsMinY = -1.4f;
	public float objectsMaxY = 1.4f;

	public float objectsMinRotation = -45.0f;
	public float objectsMaxRotation = 45.0f;

	public Button pauseButton;
	private int counter = 1;


	void Awake (){
		player = GetComponent<MouseController>();
	}

	// Use this for initialization
	void Start () {
		float height = 2.0f * Camera.main.orthographicSize;
		screenWidthInPoints = height * Camera.main.aspect;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () 
	{    
		GenerateRoomIfRequired();

		GenerateObjectsIfRequired();

	}

	void AddRoom(float farhtestRoomEndX)
	{
		//1
		int randomRoomIndex = Random.Range(0, availableRooms.Length);

		//2
		GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);

		//3
		float roomWidth = room.transform.Find("floor").localScale.x;

		//4
		float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;

		//5
		room.transform.position = new Vector3(roomCenter, 0, 0);

		//6
		currentRooms.Add(room); 

	}

	void AddRoom2(float farhtestRoomEndX)
	{
		//1
		int randomRoomIndex = Random.Range(0, availableRooms2.Length);

		//2
		GameObject room = (GameObject)Instantiate(availableRooms2[randomRoomIndex]);

		//3
		float roomWidth = room.transform.Find("floor").localScale.x;

		//4
		float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;

		//5
		room.transform.position = new Vector3(roomCenter, 0, 0);

		//6
		currentRooms.Add(room); 

	}

	void AddObject(float lastObjectX)
	{
		//1
		int randomIndex = Random.Range(0, availableObjects.Length);

		if (player != null) {
			if (player.distance < 100) {
				if (randomIndex > 10) {
					randomIndex -= 5; 	
				}		
			}
		}

		//2
		GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);

		if (obj.tag == "coin") {
			objectsMinY = 0.0f;
			objectsMaxY = 1.4f;
		} else if (obj.tag == "star") {
			objectsMinY = -3.05f;
			objectsMaxY = -3.05f;
		} else if (obj.tag == "rocket") {
			float playerY = transform.position.y;   
			objectsMinY = playerY-0.3f;
			objectsMaxY = playerY+0.3f;
		}
		else {
			objectsMinY = -2.4f;
			objectsMaxY = 2.4f;
		}
		//3
		float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
		float randomY = Random.Range(objectsMinY, objectsMaxY);
		obj.transform.position = new Vector3(objectPositionX,randomY,0); 

		//4
//		float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);
//		obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);

		//5
		objects.Add(obj);  



	}

	void GenerateObjectsIfRequired()
	{
		//1
		float playerX = transform.position.x;        
		float removeObjectsX = playerX - screenWidthInPoints;
		float addObjectX = playerX + screenWidthInPoints;
		float farthestObjectX = 0;

		//2
		List<GameObject> objectsToRemove = new List<GameObject>();

		foreach (var obj in objects)
		{
			if (obj == null)
				continue;
			//3
			float objX = obj.transform.position.x;

			//4
			farthestObjectX = Mathf.Max(farthestObjectX, objX);

			//5
			if (objX < removeObjectsX)            
				objectsToRemove.Add(obj);
		}

		//6
		foreach (var obj in objectsToRemove)
		{
			objects.Remove(obj);
			Destroy(obj);
		}

		//7
		if (farthestObjectX < addObjectX)
			AddObject(farthestObjectX);
		
	}

	void GenerateRoomIfRequired()
	{
		//1
		List<GameObject> roomsToRemove = new List<GameObject>();

		//2
		bool addRooms = true;        

		//3
		float playerX = transform.position.x;

		//4
		float removeRoomX = playerX - screenWidthInPoints;        

		//5
		float addRoomX = playerX + screenWidthInPoints;

		//6
		float farthestRoomEndX = 0;

		foreach(var room in currentRooms)
		{
			//7
			float roomWidth = room.transform.Find("floor").localScale.x;
			float roomStartX = room.transform.position.x - (roomWidth * 0.5f);    
			float roomEndX = roomStartX + roomWidth;                            

			//8
			if (roomStartX > addRoomX)
				addRooms = false;

			//9
			if (roomEndX < removeRoomX)
				roomsToRemove.Add(room);

			//10
			farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
		}

		//11
		foreach(var room in roomsToRemove)
		{
			currentRooms.Remove(room);
			Destroy(room);            
		}

		//12
		if (addRooms) {
			if (player != null) {
				if (player.level0) {
					AddRoom2 (farthestRoomEndX);	
				} else if (player.level1) {
					AddRoom (farthestRoomEndX);
				} else if (player.level2) {
					AddRoom2 (farthestRoomEndX);
				} else if (player.level3) {
					AddRoom (farthestRoomEndX);
				} else if (player.level4) {
					AddRoom2 (farthestRoomEndX);
				} else if (player.level5) {
					AddRoom (farthestRoomEndX);
				} else if (player.level6) {
					AddRoom2 (farthestRoomEndX);
				} else if (player.level7) {
					AddRoom (farthestRoomEndX);
				} else if (player.level8) {
					AddRoom2 (farthestRoomEndX);
				} else if (player.level9) {
					AddRoom (farthestRoomEndX);
				} else if (player.level10) {
					AddRoom2 (farthestRoomEndX);
				} else if (player.level11) {
					AddRoom (farthestRoomEndX);
				} else if (player.level12) {
					AddRoom2 (farthestRoomEndX);
				} else if (player.level13) {
					AddRoom (farthestRoomEndX);
				} else if (player.level14) {
					AddRoom2 (farthestRoomEndX);
				} else if (player.level15) {
					AddRoom (farthestRoomEndX);
				} else if (player.level16) {
					AddRoom2 (farthestRoomEndX);
				} else if (player.level17) {
					AddRoom (farthestRoomEndX);
				} else if (player.level18) {
					AddRoom2 (farthestRoomEndX);
				} else if (player.level19) {
					AddRoom (farthestRoomEndX);
				} else if (player.level20) {
					AddRoom2 (farthestRoomEndX);
				} else {
					AddRoom (farthestRoomEndX);
				}

			}
		}
	}
}
