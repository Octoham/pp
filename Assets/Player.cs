using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float acceleration = 500f; // kg*m/s²
    public float airAcceleration = 100f; // kg*m/s²
    public float maxSpeed = 2f; // m/s
    public float maxAirSpeed = 3f; // m/s²
    public float jumpSpeed = 50f; // m/s²
    public NetworkVariable<Color> color = new NetworkVariable<Color>();
    public Keybinds keybinds = Keybinds.defaultKeybinds();
    public static Player localPlayer;

    public Rigidbody2D rigidbody;


    private bool isGrounded = false;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            randomizeColorServerRpc();
            localPlayer = this;
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
            // ground check
            if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1.05f), Vector2.down, 0.05f))
            {
                isGrounded = true;
                Debug.Log("Grounded");
            }
            else
            {
                isGrounded = false;
            }


            Vector2 force = Vector2.zero;
            Vector2 speed = rigidbody.velocity;

            // horizontal movement
            if (isGrounded)
            {
                if (Input.GetKey(keybinds.left) && rigidbody.velocity.x >= -maxSpeed)
                {
                    force += (acceleration * Vector2.left);
                }
                if (Input.GetKey(keybinds.right) && rigidbody.velocity.x <= maxSpeed)
                {
                    force += (acceleration * Vector2.right);
                }
            }
            else
            {
                if (Input.GetKey(keybinds.left) && rigidbody.velocity.x >= -maxAirSpeed)
                {
                    force += (airAcceleration * Vector2.left);
                }
                if (Input.GetKey(keybinds.right) && rigidbody.velocity.x <= maxAirSpeed)
                {
                    force += (airAcceleration * Vector2.right);
                }
            }

            // vertical movement
            if (Input.GetKeyDown(keybinds.jump) && isGrounded)
            {
                
                speed.y = jumpSpeed;

            }

            if(force != Vector2.zero)
            {
                Debug.Log(force);
            }

            rigidbody.velocity = speed;
            rigidbody.AddForce(force * Time.deltaTime);
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