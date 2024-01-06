using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    float time;
    void Start()
    {
        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * Time.deltaTime;
        time += Time.deltaTime;
        if (time >= 0.3f)
        {
            transform.localScale -= Vector3.one * 3 * Time.deltaTime;
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
