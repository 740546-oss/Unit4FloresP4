using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup;
    private float powerupStrength = 15.0f;
    public GameObject powerupIndicator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb= GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed *  forwardInput);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            powerupIndicator.gameObject.SetActive(true);
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
            
            IEnumerator PowerupCountdownRoutine()
                { 
                yield return new WaitForSeconds(7);
                    hasPowerup = false;
                powerupIndicator.gameObject.SetActive(false);
                }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Debug.Log("Collid with " + collision.gameObject.name + " with powerup set to " + hasPowerup);

            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);

            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            Debug.Log("Player collided with " + collision.gameObject.name + " with powerup set to " + hasPowerup);
        }

    }
}
