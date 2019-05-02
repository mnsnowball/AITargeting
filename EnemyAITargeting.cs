using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAITargeting : MonoBehaviour {

    public Transform player;
    public Rigidbody2D playerRb;
    public float projectileSpeed;
    public Transform projectileSpawn;
    public GameObject bullet;

    int delay = 0;
    int delayRate = 30;

	// Use this for initialization
	void Start () {
        playerRb = player.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        delay++;

        if (delay >= delayRate) {
            delay = 0;
			ShootBullet(GetAngle(player, playerRb.velocity));
        } 

        /*
         
         float theAngle = GetAngle(player, playerRb.velocity);
         ShootBullet(theAngle);
         */
	}

    // gets the player's predicted position and returns the angle between that and the current position
    float GetAngle(Transform playerLocation, Vector3 velocity) {
        Vector3 predictedPosition = GetPredictedPosition(playerLocation.position, velocity);

        Vector3 directionAngle = predictedPosition - transform.position; // vector for direction from the turret position to the predicted player position

		float predictedAngle = Vector3.SignedAngle(transform.right, directionAngle, Vector3.up);
        if (playerLocation.position.y < 0) {
            predictedAngle *= -1;
        }
            
		Debug.Log ("angle is " + predictedAngle.ToString ());
        return predictedAngle;
    }

    //Gets the Player's predicted position based on player speed
	Vector3 GetPredictedPosition(Vector3 playerLocation, Vector3 velocity) {// add  float predictedTimeElapsed

		Vector3 offset = new Vector3(0,0,0);
        int multiplier = 5;
		// case where player is travelling horizontally towards(x = -, y = 0)
		if(velocity.x < 0 && velocity.y == 0){
			// predicted position would be
			offset = new Vector3(-multiplier, 0,0);
		}
			 
		// case where travel is horizontally away (x = +, y = 0)
		if(velocity.x > 0 && velocity.y == 0){
			// predicted position would be
			offset = new Vector3(multiplier, 0,0);
		}

		// travelling up( x = 0, y= +)
		if(velocity.x == 0 && velocity.y > 0){
			// predicted position would be
			offset = new Vector3(0, multiplier, 0);
		}

		// travelling down( x = 0, y= -)
		if(velocity.x == 0 && velocity.y < 0){
			// predicted position would be
			offset = new Vector3(0, -multiplier, 0);
		}
            
		// x-velocity is positive(away), and y-velocity is positive(up)
		if(velocity.x > 0 && velocity.y > 0){
			// predicted position would be
			offset = new Vector3(multiplier, multiplier, 0);
		}

		// x-velocity is positive(away), and y-velocity is negative(down)
		if(velocity.x > 0 && velocity.y < 0){
			// predicted position would be
			offset = new Vector3(multiplier, -multiplier, 0);
		}

		// x-velocity is negative(towards), and y-velocity is positive(up)
		if(velocity.x < 0 && velocity.y > 0){
			// predicted position would be
			offset = new Vector3(-multiplier, multiplier, 0);
		}

		// x-velocity is negative(towards), and y-velocity is negative(down)
		if(velocity.x < 0 && velocity.y < 0){
			// predicted position would be
			offset = new Vector3(-multiplier, -multiplier, 0);
		}
		Debug.Log("Predicted position is " + (playerLocation + offset).x.ToString() + ", " +(playerLocation + offset).y.ToString() + ", 0");
		return playerLocation + offset;
    }

    //returns the distance between the player and the enemy
    float GetDistance() {
        float distance = Vector3.Distance(player.position, transform.position);
        Debug.Log("Distance is " + distance.ToString());
        return distance;
    }


    // shoots the bullet at the specified angle
    void ShootBullet(float angle) {
        //float rads = angle * Mathf.Deg2Rad; //convert to radians so we can use sin and cosine functions

        //float x_component = Mathf.Cos(rads) * projectileSpeed;
        //float y_component = Mathf.Sin(rads) * projectileSpeed;

        GameObject bullet_clone = Instantiate(bullet, projectileSpawn.position, Quaternion.identity); // get a reference to the object so we can add force to it
        bullet_clone.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        bullet_clone.GetComponent<Rigidbody2D>().velocity = bullet_clone.transform.right * projectileSpeed;

        Destroy(bullet_clone, 3f);
    }
 
}
