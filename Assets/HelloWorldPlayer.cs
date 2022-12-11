using Unity.Netcode;
using UnityEngine;

public class HelloWorldPlayer : NetworkBehaviour
{
    public NetworkVariable<Vector2> Position = new NetworkVariable<Vector2>();
    public NetworkVariable<Color> color = new NetworkVariable<Color>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
            randomizeColorServerRpc();
        }

    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }
        else
        {
            SubmitPositionRequestServerRpc();
        }
    }
    [ServerRpc]
    public void randomizeColorServerRpc(ServerRpcParams rpcParams = default)
    {
        color.Value = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = GetRandomPositionOnPlane();
    }
    
    static Vector2 GetRandomPositionOnPlane()
    {
        return new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
    }
    void Update()
    {
        transform.position = Position.Value;
        GetComponent<SpriteRenderer>().color = color.Value;
    }
}