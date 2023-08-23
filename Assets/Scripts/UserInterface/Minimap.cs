using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private Camera self;
    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void zoomin() {
        if(self.orthographicSize>100)
        self.orthographicSize -= 50;
    }

    public void zoomout() {
        if (self.orthographicSize < 500)
            self.orthographicSize += 50;
    }

}
