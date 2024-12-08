using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Input.mousePosition;
        var worldPoint = cam.ScreenToWorldPoint(mousePosition);
        var direction = (worldPoint - cam.transform.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
