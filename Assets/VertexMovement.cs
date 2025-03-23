using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexMovement : MonoBehaviour
{
    public GameObject point;
    public float speed = 1f;
    
    private float alpha = 0f;
    public float randomizeFactor = 0f;

    void Start(){
        alpha = Random.Range(0f, 6.2830f) * randomizeFactor;

        float speedRandomize = .2f;
        speed += speed * Random.Range(-speedRandomize, speedRandomize);
    }

    // Update is called once per frame
    void Update()
    {
        point.transform.position =  gameObject.transform.localScale.x / 2 *  new Vector3(Mathf.Cos(alpha), Mathf.Sin(alpha), 0f) + gameObject.transform.position;
        alpha += 6.2830f / 1000 * speed;
    }
}
