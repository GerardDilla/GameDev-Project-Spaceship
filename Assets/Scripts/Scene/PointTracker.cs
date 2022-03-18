using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTracker : MonoBehaviour
{

    public ShipControl ship;
    public bool Tracking;
    public Vector3 lastFramePosition;
    public float distanceToMove;
    public float points;
    public float progress;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        ship = gameManager.ship.GetComponent<ShipControl>();
        lastFramePosition = ship.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        distanceToMove = ship.transform.position.y - lastFramePosition.y;
        if (transform.position.y <= (transform.position.y + distanceToMove))
        {

            transform.position = new Vector3(transform.position.x, transform.position.y + distanceToMove, transform.position.z);
            lastFramePosition = ship.transform.position;

            if (Tracking == true)
            {
                progress += Time.deltaTime;
                points += Time.deltaTime;
                gameManager.TallyScore(points);
            }

        }
    }
    public void AddPoints(float point)
    {
        points += point;
    }
}
