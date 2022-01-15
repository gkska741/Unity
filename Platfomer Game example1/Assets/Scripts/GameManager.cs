using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //게임매니저 : 점수/스테이지 관리
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public GameObject[] Stages;
    public PlayerMove player;

    public UnityEngine.UI.Image[] UIhealth;
    public UnityEngine.UI.Text UIpoint;
    public UnityEngine.UI.Text UIstage;
    public GameObject RestartBtn;



    // Start is called before the first frame update

    void Update()
    {
        UIpoint.text = (totalPoint + stagePoint).ToString();
        UIstage.text = "STAGE " + (stageIndex + 1);
    }
    public void NextStage()
    {
        Debug.Log(Stages.Length - 1);
        if (stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else
        {
            Time.timeScale = 0;
            RestartBtn.SetActive(true);
            UnityEngine.UI.Text btnText = RestartBtn.GetComponentInChildren<UnityEngine.UI.Text>();
            btnText.text = "Clear";
            ViewBtn();
        }

        totalPoint += stagePoint;
        stagePoint = 0;
    }
    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 1, 0.1f);
        }
        else
        {
            UIhealth[0].color = new Color(1, 0, 1, 0.1f);
            player.OnDie();
            Debug.Log("죽었습니다!");
            player.PlaySound("DIE");
            RestartBtn.SetActive(true);
        }
    }    

    // Update is called once per frame
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Player Reposition
            if (health > 1)
            {
                PlayerReposition();
            }
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(3, 6.46f, 0);
        player.VelocityZero();
    }
    void ViewBtn()
    {
        RestartBtn.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
