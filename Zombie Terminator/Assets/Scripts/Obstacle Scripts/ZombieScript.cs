using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public GameObject bloodFXPrefab;
    public float speed = 1f;
    private Rigidbody myBody;
    void Start()
    {
        myBody = GetComponent<Rigidbody>();
        myBody.velocity = new Vector3(0f, 0f, -speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10f)
        {
            gameObject.SetActive(false);
        }
    }
    void Die()
    {
        myBody.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        GetComponentInChildren<Animator>().Play("Idle");
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        transform.localScale = new Vector3(1f, 1f, 0.2f);
        transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
    }
    void DeactivateGameObject()
    {
        gameObject.SetActive(false);

    }
    private void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag == "Player" || target.gameObject.tag == "Bullet")
        {
            Instantiate(bloodFXPrefab, transform.position, Quaternion.identity);
            Invoke("DeactivateGameObject", 3f);
            GameplayController.instance.IncreaseScore();
            Die();
        }
    }
}
