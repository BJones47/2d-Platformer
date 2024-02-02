using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{

    public int offsetX = 2;                 // the offset so we dont get errors.

    // these are used for checking if we have to instantiate.
    public bool hasARightBuddy = false;     
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;       // used if the object is not tileable.

    private float spriteWidth = 0f;         // the width of our element.
    private Camera cam;
    private Transform myTransform;

    void Awake()  {
        cam = Camera.main;
        myTransform = transform;
    }


    // Start is called before the first frame update
    void Start()  {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()  {
        // Does it still need buddy's if not do nothing.
        if (hasALeftBuddy == false || hasARightBuddy == false) {
            // Calculate the cameras extend (half the width) of what the camera can see in world coordinates.
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            // Calculate the x position where the camera can see the edge of the sprite (element).
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            //  Checking if we can see the edge of our element then calling a new buddy if we can.
            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false) {
                MakeNewBuddy(1);
                hasARightBuddy = true;  
            }  else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false)  {
                MakeNewBuddy(-1);
                hasALeftBuddy = true; 
            }
        }
    }

    // A function that creates a new buddy on the side required.
    void MakeNewBuddy(int rightorLeft) {
        // Calculating the new position for our new buddy.
        Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightorLeft, myTransform.position.y, myTransform.position.z);
        // instantiating our new buddy and storing him in a variable. 
        Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;    
        // If not tileable let's reverse the x size of our objects to get rid of ugly scenes.
        if (reverseScale == true) {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (rightorLeft > 0) {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }   else {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;  
        }
    }


}
