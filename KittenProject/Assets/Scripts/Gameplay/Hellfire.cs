using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Hellfire : MonoBehaviour 
{
    const float speedAfterGameFinished = -20;

    public GameObject flame;
    public int flamesCount;
    public float flamesXOffset;
    public float killYOffset;
    public float speed;
    public PlayerLogic player;
    public Transform glow;

    LinkedList<GameObject> flames;
    float lastSwapPosition;

	void Start () 
    {
        if (flamesCount > 0)
        {
            flames = new LinkedList<GameObject>();

            flames.AddFirst(flame);
            flame.transform.SetPositionX(player.transform.position.x - flamesXOffset * flamesCount / 2.0f);

            for (int i = 1; i < flamesCount; i++)
            {
                GameObject newFlame = Instantiate(flame);
                newFlame.transform.parent = transform;
                newFlame.transform.position = flame.transform.position + Vector3.right * flamesXOffset * i;
                flames.AddLast(newFlame);
            }

            lastSwapPosition = player.transform.position.x;
        }

        PostOffice.victory += OnGameFinished;
    }

    void OnDestroy()
    {
        PostOffice.victory -= OnGameFinished;
    }
	
	void Update ()
    {
	    if (lastSwapPosition + flamesXOffset < player.transform.position.x)
        {
            lastSwapPosition += flamesXOffset;

            flame = flames.First.Value;
            flame.transform.position = flames.Last.Value.transform.position + Vector3.right * flamesXOffset;
            flames.RemoveFirst();
            flames.AddLast(flame);
        }
        else if (lastSwapPosition - flamesXOffset > player.transform.position.x)
        {
            lastSwapPosition -= flamesXOffset;

            flame = flames.Last.Value;
            flame.transform.position = flames.First.Value.transform.position - Vector3.right * flamesXOffset;
            flames.RemoveLast();
            flames.AddFirst(flame);
        }

        transform.SetPositionY(transform.position.y + speed * Time.deltaTime);

        glow.SetPositionX(player.transform.position.x);

        if (player.transform.position.y < transform.position.y + killYOffset)
        {
            player.DeathByHellfire();
        }
    }

    void OnGameFinished()
    {
        speed = speedAfterGameFinished;
    }
}
