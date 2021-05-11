using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerLogic : MonoBehaviour
{
    public int totalItem;
    public int stage;
    public Text TotalItemCount;
    public Text PlayerScore;

    private void Awake()
    {
        TotalItemCount.text = " / " + totalItem.ToString();
    }

    public void GetItem(int count)
    {
        PlayerScore.text = count.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(stage);
        }
    }
}
