using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public float size = 1.0f; 
    //default, but can be modified, would be better if it was private, and you expose an api to update it
    //encapsulation is important to object orientated programming, information hiding
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 5.0f;
    public float maxLifetime = 30.0f;
    private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rigidBody;
    // Start is called before the first frame update
    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        //randomize        
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;

        _rigidBody.mass = this.size;
    }

    public void SetTrajectory(Vector2 direction){
        _rigidBody.AddForce(direction * this.speed);
        Destroy(this.gameObject, maxLifetime);
    }

    public void RandomizeAsteroidSize(){
        this.size = Random.Range(this.minSize, this.maxSize);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Bullet"){
            if((this.size * 0.5f) >= this.minSize){
                CreateSplit();
                CreateSplit();
            }

            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }
    }

    private void CreateSplit(){
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;

        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
    }
}
