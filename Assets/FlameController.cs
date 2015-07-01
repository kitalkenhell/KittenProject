using UnityEngine;
using System.Collections;

public class FlameController : MonoBehaviour
{
    void Start()
    {
        //GetComponent<Animator>().speed = Random.Range(1, 10);
        GetComponent<Animator>().SetFloat(Animator.StringToHash("offset"), Random.Range(0, 1.0f));
    }

    void Update()
    {

    }
}
