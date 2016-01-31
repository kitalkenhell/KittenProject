using UnityEngine;
using System.Collections;

public class MorningStar : MonoBehaviour
{
    public Transform sprite;
    public Rigidbody2D chainRoot;
    public HingeJoint2D chainLinkAPrefab;
    public HingeJoint2D chainLinkBPrefab;
    public int chainLinksCount;
    public float pushForce;

    Rigidbody2D body;

    void Awake()
    {
        HingeJoint2D link = (Instantiate(chainLinkAPrefab, chainRoot.position, Quaternion.identity) as HingeJoint2D);
        HingeJoint2D previousLink = link;
        HingeJoint2D hingeJoint = GetComponent<HingeJoint2D>();
        body = GetComponent<Rigidbody2D>();

        link.connectedBody = chainRoot;
        
        for (int i = 1; i < chainLinksCount; i++)
        {
            link = (Instantiate((i % 2 == 0) ? chainLinkAPrefab : chainLinkBPrefab, previousLink.transform.position + Vector3.up * previousLink.connectedAnchor.y, Quaternion.identity) as HingeJoint2D);
            link.connectedBody = previousLink.GetComponent<Rigidbody2D>();
            link.transform.SetPositionZ(sprite.position.z);
            link.transform.parent = transform.parent;
            previousLink = link;
        }

        transform.position = link.transform.position - hingeJoint.connectedAnchor.Vec3();
        hingeJoint.connectedBody = link.GetComponent<Rigidbody2D>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layers.Player)
        {
            body.AddForce((transform.position - collision.transform.position).normalized * pushForce);
        }
    }
}
