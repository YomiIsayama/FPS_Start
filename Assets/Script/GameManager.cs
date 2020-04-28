using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    //得分
    public int m_score = 0;
    //最高分
    public static int m_hiscore = 0;
    //弹药
    public int m_ammo = 100;
    //主角
    Player m_player;
    //UI文字
    Text txt_ammo;
    Text txt_hiscore;
    Text txt_life;
    Text txt_score;
    Button button_restart;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //获取主角
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //获取UI文字
        GameObject uicanvas = GameObject.Find("Canvas");
        foreach (Transform t in uicanvas.transform.GetComponentsInChildren<Transform>())
        {
            if (t.name.CompareTo("txt_ammo") == 0)
            {
                txt_ammo = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("txt_hiscore") == 0)
            {
                txt_hiscore = t.GetComponent<Text>();
                txt_hiscore.text = "High Score" + m_hiscore;
            }
            else if (t.name.CompareTo("txt_life") == 0)
            {
                txt_life = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("txt_score") == 0)
            {
                txt_score = t.GetComponent<Text>();
            }
            else if (t.name.CompareTo("Restart_Button") == 0)
            {
                button_restart = t.GetComponent<Button>();
                button_restart.onClick.AddListener(delegate ()
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                });
                button_restart.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    //更新分数
    public void SetScore(int Score)
    {
        m_score += Score;
        if (m_score > m_hiscore)
        {
            m_hiscore = m_score;
        }
        txt_score.text = "Score <color=yellow>" + m_score + "</color>";
        txt_hiscore.text = "High Score" + m_hiscore;
    }
    //更新弹药
    public void SetAmmo(int ammo)
    {
        m_ammo -= ammo;
        if (m_ammo <= 0)
        {
            m_ammo = 100 - m_ammo;
        }
        txt_ammo.text = m_ammo.ToString() + "/100";
    }
    //更新主角
    public void SetLife(int life)
    {
        txt_life.text = life.ToString();
        if (life <= 0)
        {
            button_restart.gameObject.SetActive(true);
        }
    }
}
