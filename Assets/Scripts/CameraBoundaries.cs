using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{

     public Camera MainCamera;
     private Rect screenBounds;
     public float objectWidth = 1;
     public float objectHeight = 1;
     
     void Start() {

         float cameraHeight = MainCamera.orthographicSize * 2;
         float cameraWidth = cameraHeight * MainCamera.aspect;
         Vector2 cameraSize = new Vector2(cameraWidth, cameraHeight);
         Vector2 cameraCenterPosition = MainCamera.transform.position;
         Vector2 cameraBottomLeftPosition = cameraCenterPosition - (cameraSize / 2);
         screenBounds = new Rect(cameraBottomLeftPosition , cameraSize);
        //  objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        //  objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
     }

    //  void Update(){
         

    //  }
     
     void LateUpdate() {

        Vector3 pos = transform.position;
        //  pos.x += Input.GetAxis("Horizontal") * maxSpeed * Time.deltaTime;
        //  pos.y += Input.GetAxis("Vertical") * maxSpeed * Time.deltaTime;
        transform.position = pos;
        screenBounds.position = (Vector2)MainCamera.transform.position - (screenBounds.size / 2);
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x + screenBounds.width - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectHeight, screenBounds.y + screenBounds.height - objectHeight);
        transform.position = viewPos;
     }
}
