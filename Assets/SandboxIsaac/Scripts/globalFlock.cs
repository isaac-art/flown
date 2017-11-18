﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class globalFlock : MonoBehaviour {


	public GameObject birdObject;
	public GameObject goalPrefab;
	public static int sceneSize = 100;

	public int playerSuccess = 1;
	Vector3 playerPosition;

	public static int numOfBirds = 200;
	public static GameObject[] allBirds = new GameObject[numOfBirds];
	public static Vector3 headingPos = Vector3.zero;

	public bool switchDirectionX = false;
	public bool switchDirectionY = false;
	public bool switchDirectionZ = false;
	public float zPos = 0;
	public float xPos = 0;
	public float yPos = 6;

	public float headingMaxDistanceFromPlayer = 16;
	public bool waitingForPlayer = false;

	// Use this for initialization
	void Start () {
		// make the birds up to max num of birds
		for(int i = 0; i < numOfBirds; i++){
			//Generate a position around current player position
			Vector3 pos = new Vector3(0,0,0);
			// add bird to list of birds
			allBirds[i] = (GameObject) Instantiate(birdObject, pos, Quaternion.identity);
			// TODO : make bird animation state so it can fade in. maybe alpha = 0?
			// make the bird object inactive
			birdObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		// update visibilit of birds in flock based upon player score.
		playerSuccess = GameObject.Find("player").GetComponent<playerScriptIsaac>().successRating;
		birdVisibility();

		// how far away from, the heading is the player
		playerPosition = GameObject.Find("player").transform.position;
		float distanceFromPlayer = Vector3.Distance(playerPosition, headingPos);
		// If the player is close enough to the heading point of the flock then update
		// the heading. Otherwise stay still till player catches up.
		if(UnityEngine.Random.Range(0,20) < 1){
			if(waitingForPlayer){
				if(distanceFromPlayer < 2){
					waitingForPlayer = false;
				}
			}else{
				if(distanceFromPlayer < headingMaxDistanceFromPlayer){
					headingPos = new Vector3(xPos, yPos, zPos);
					goalPrefab.transform.position = headingPos;
					xPos += Random.Range(-0.1f,0.1f);
					yPos += Random.Range(-0.1f,0.1f);

					//keep above ground
					if(yPos <= 3){
						yPos += Random.Range(0.2f,0.4f);
					}

					// if(zPos >= sceneSize ||  zPos <= -sceneSize){
					// 	switchDirectionZ = !switchDirectionZ;
					// }
					zPos += 1f;
				}else{
					waitingForPlayer = true;
				}
			}
			
		}
		
	}

	void birdVisibility(){

		GameObject[] gos;
		gos = globalFlock.allBirds;
		//switch birds on up to count
		for(int i = 0; i < playerSuccess; i++){	
			// if bird isnt active make active
			if(!gos[i].active){
				// TODO: check the direction the player is facing. currently only places
				// behind if the player is facing  forward in space, if facing back the
				// new bird is positioned right in front of you
				gos[i].transform.position = new Vector3(
						UnityEngine.Random.Range(playerPosition.x-1, playerPosition.x),
						UnityEngine.Random.Range(playerPosition.y-1, playerPosition.y),
						UnityEngine.Random.Range(playerPosition.z-2, playerPosition.z-1));
				gos[i].SetActive(true);
				// TODO : fade in bird 
			}
		}
		//switch birds off
		for(int i = playerSuccess; i < numOfBirds; i++){
			if(gos[i].active){
				gos[i].SetActive(false);
			}
		}

		
	}


}
