using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;


    public float size = 2.0f;       //le kene private-olni


    private void Start()
    {
        //valami valami random sprite

        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);    //megforgatja random szogben
        this.transform.localScale = Vector3.one * this.size;

        
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Irany(Vector2 dir, float meret)
    {
        if (meret == 2.0f)           //mekkora a meret, nagyobb a sebesseg
        {
            rigidBody.AddForce(dir * Random.Range(5.0f, 10.0f));
        }
        else if (meret == 1.0f)
        {
            rigidBody.AddForce(dir * Random.Range(10.0f, 15.0f));
        }
        else
        {
            rigidBody.AddForce(dir * Random.Range(15.0f, 20.0f));
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            if((this.size / 2) >= 0.5f)
            {
                Vector2 pos = this.transform.position;
                pos = pos + (Random.insideUnitCircle / 2);

                Asteroid miniAsteroid1 = Instantiate(this, pos, this.transform.rotation);
                Asteroid miniAsteroid2 = Instantiate(this, pos, this.transform.rotation);

                miniAsteroid1.size = this.size / 2;
                miniAsteroid2.size = this.size / 2;

                miniAsteroid1.Irany(Random.insideUnitCircle.normalized * 5.0f, miniAsteroid1.size);
                miniAsteroid2.Irany(Random.insideUnitCircle.normalized * 5.0f, miniAsteroid2.size);
            }
            Destroy(this.gameObject);
        }
    }
}
