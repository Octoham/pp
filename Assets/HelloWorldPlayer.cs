using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class HelloWorldPlayer : NetworkBehaviour
{
    public float acceleration = 50f; // m/s²
    public float maxSpeed = 2f; // m/s
    public NetworkVariable<Color> color = new NetworkVariable<Color>();
    public Keybinds keybinds = Keybinds.defaultKeybinds();

    public Rigidbody2D rigidbody;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            randomizeColorServerRpc();
        }

    }

    [ServerRpc]
    public void randomizeColorServerRpc(ServerRpcParams rpcParams = default)
    {
        color.Value = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // handle movement input
        if(IsOwner)
        {
                Vector2 force = Vector2.zero;
                if (Input.GetKey(keybinds.left) && rigidbody.velocity.x >= -maxSpeed)
                {
                    force += (Vector2.left * acceleration * Time.deltaTime);
                }
                if (Input.GetKey(keybinds.right) && rigidbody.velocity.x <= maxSpeed)
                {
                    force += (Vector2.right * acceleration * Time.deltaTime);
                }
                rigidbody.AddForce(force);
        }

        // sync the values
        
        GetComponent<SpriteRenderer>().color = color.Value;
    }
}

// custom class for keybinds
[System.Serializable]
public class Keybinds
{
    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;

    public static Keybinds defaultKeybinds()
    {
        Keybinds keybinds = new Keybinds();
        keybinds.left = KeyCode.A;
        keybinds.right = KeyCode.D;
        keybinds.jump = KeyCode.W;
        return keybinds;
    }
}