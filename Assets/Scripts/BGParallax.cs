using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGParallax : MonoBehaviour
{

    public Transform cam;

    private Vector3 lastCameraPosition;

    public Vector2 parallaxMultiplier;

    private float textureUnitSizeY;

    public bool enableFollow;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        lastCameraPosition = cam.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeY = (texture.height / sprite.pixelsPerUnit) * transform.localScale.y;

        // texture_unit_size_x_ = (texture.width / sprite.pixelsPerUnit) * transform.localScale.x;
        // texture_unit_size_y_ = (texture.height / sprite.pixelsPerUnit) * transform.localScale.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaMovement = cam.position - lastCameraPosition;
        //float parallaxEffect = .6f;
        // transform.position += deltaMovement * parallaxEffect;
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier.x, deltaMovement.y * parallaxMultiplier.y);
        lastCameraPosition = cam.position;
        // Debug.Log(Mathf.Abs(cam.position.y - transform.position.y) + " >= " + textureUnitSizeY);
        if (Mathf.Abs(cam.position.y - transform.position.y) >= textureUnitSizeY)
        {
            if (enableFollow == true)
            {
                float offsetPositionY = (cam.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, cam.position.y + offsetPositionY, transform.position.z);
            }

        }
    }
}
