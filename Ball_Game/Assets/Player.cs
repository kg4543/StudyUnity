using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    Rigidbody rigid;
    bool isJump;
    public float jumpPower;
    public GameManagerLogic manager;
    public int score;
    AudioSource audio;
    

    private void Awake()
    {
        isJump = false;
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            isJump = true;
            rigid.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 move = new Vector3(x, 0, y);
        rigid.AddForce(move, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isJump = false;
            jumpPower = 80;
        }
        else if(collision.gameObject.tag == "Power")
        {
            isJump = false;
            jumpPower = 200;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            //Item Item = other.GetComponent<Item>();
            score++;
            audio.Play();
            other.gameObject.SetActive(false);
            manager.GetItem(score);
        }
        else if (other.tag == "Attack")
        {
            SceneManager.LoadScene("Stage_" + manager.stage);
        }
        else if (other.tag == "Potal")
        {
            if (score == manager.totalItem)
            {
                if (manager.stage < 5)
                {
                    SceneManager.LoadScene("Stage_" + (manager.stage + 1));
                }
                else
                {
                    SceneManager.LoadScene("Stage_0");
                }
                
            }
            else
            {
                SceneManager.LoadScene("Stage_" + manager.stage);
            }
            
        }
    }
}
