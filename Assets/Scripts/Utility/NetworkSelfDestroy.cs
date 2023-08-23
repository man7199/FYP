using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class NetworkSelfDestroy : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SelfDestroy", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SelfDestroy() {
        Runner.Despawn(GetComponent<NetworkObject>());
    }
}
