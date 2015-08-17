using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Hellfire : MonoBehaviour 
{
    public GameObject flame;
    public int flamesCount;
    public float offset;
    public float speed;
    public Transform player;

    LinkedList<GameObject> flames;
    float lastSwapPosition;

	void Start () 
    {
        if (flamesCount > 0)
        {
            flames = new LinkedList<GameObject>();

            flames.AddFirst(flame);
            flame.transform.SetPositionX(player.position.x - offset * flamesCount / 2.0f);

            for (int i = 0; i < flamesCount - 1; i++)
            {
                GameObject newFlame = Instantiate(flame);
                newFlame.transform.parent = transform;
                newFlame.transform.position = flame.transform.position + Vector3.right * offset * i;
                flames.AddLast(newFlame);
            }

            lastSwapPosition = player.position.x;
        }
	}
	
	void Update ()
    {
	    if (lastSwapPosition + offset < player.position.x)
        {
            lastSwapPosition += offset;

            flame = flames.First.Value;
            flame.transform.position = flames.Last.Value.transform.position + Vector3.right * offset;
            flames.RemoveFirst();
            flames.AddLast(flame);
        }
        else if (lastSwapPosition - offset > player.position.x)
        {
            lastSwapPosition -= offset;

            flame = flames.Last.Value;
            flame.transform.position = flames.First.Value.transform.position - Vector3.right * offset;
            flames.RemoveLast();
            flames.AddFirst(flame);
        }

        transform.SetPositionY(transform.position.y + speed * Time.deltaTime);
	}
}
