using Fusion;
using UnityEngine;


public class Player : NetworkBehaviour
{
    public static Player Instance;
    public override void Spawned()
    {
        if (Instance != null)
        {
            Debug.LogError("There should be only one Player.cs .");
            return;
        }
        Instance = this;
    }

    public PlayerRef MyPlayerRef()
    {
        return Runner.LocalPlayer;
    }
}