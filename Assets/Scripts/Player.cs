using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;
    private bool _thrusting;

    private float _turnDirection;
    private Rigidbody2D _rigidBody;

    public float thrustSpeed = 1.0f;
    public float turnSpeed = 1.0f;


    private void Awake(){
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            _turnDirection = 1.0f;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            _turnDirection = -1.0f;
        } else {
            _turnDirection = 0.0f;
        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            Shoot();
        }
    }

    private void FixedUpdate() {
        if (_thrusting) {
            _rigidBody.AddForce(this.transform.up * this.thrustSpeed);
        }

        if (_turnDirection != 0.0){
            _rigidBody.AddTorque(_turnDirection * this.turnSpeed);
        }
    }

    private void Shoot(){
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Asteroid"){
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = 0.0f;

            this.gameObject.SetActive(false);

            // Invoke() PlayerDied method of GM
            // Invoke(nameof(_gameManager.PlayerDied), 0.0f);
            FindObjectOfType<GameManager>().PlayerDied();
        }
    }

}
