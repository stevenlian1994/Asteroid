using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public int lives = 3;
    private float respawnTime = 3.0f;
    private float respawnInvulnerabilityTime = 3.0f;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;


    public int score = 0;
    public void AsteroidDestroyed(Asteroid asteroid){
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        // TODO: increase score
        if (asteroid.size < 0.75f) {
            score += 100;
        } else if(asteroid.size < 1.2f) {
            score += 50;
        } else {
            this.score += 25;
        }
    }
    public void PlayerDied(){
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();

        this.lives--;

        if(this.lives <= 0){
            GameOver();
        } else {
        Invoke(nameof(Respawn), this.respawnTime);
        }
    }

    private void Respawn(){
        Debug.Log("respawning..");
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
        this.player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollisions), respawnInvulnerabilityTime);
    }

    private void TurnOnCollisions(){
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver(){
        //TODO
        this.lives = 3;
        this.score = 0;

        Invoke(nameof(Respawn), this.respawnTime);
    }

    private void Update(){
        scoreText.text = "SCORE: " + Mathf.RoundToInt(score).ToString();
        livesText.text = "LIVES: " + Mathf.RoundToInt(lives).ToString();

    }
}
