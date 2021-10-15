using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;
    public GameObject[] obstaclePrefabs;
    public GameObject[] zombiePrefabs;
    public Transform[] lanes;
    public float min_ObstacleDelay = 10f, max_ObstacleDelay = 40f;
    private float halfGroundsize;
    private BaseController playerController;


    private Text score_Text;
    private int zombie_Kill_Count;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]

    private GameObject gameover_Panel;
    [SerializeField]
    private Text final_Score;
    private void Awake()
    {
        MakeInstance();
    }
    void Start()
    {
        halfGroundsize = GameObject.Find("GroundBlock Main").GetComponent<GroundScript>().halfLength;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseController>();
        StartCoroutine("GenerateObstacles");

        score_Text = GameObject.Find("ScoreText").GetComponent<Text>();

    }

    // Update is called once per frame

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }
    IEnumerator GenerateObstacles()
    {
        float timer = Random.Range(min_ObstacleDelay, max_ObstacleDelay) / playerController.speed.z;
        yield return new WaitForSeconds(timer);
        CreateObstacles(playerController.gameObject.transform.position.z + halfGroundsize);
        StartCoroutine("GenerateObstacles");
    }
    void CreateObstacles(float zPos)
    {
        int r = Random.Range(0, 10);
        if (0 <= r && r < 7)
        {
            int obstacleLane = Random.Range(0, lanes.Length);
            AddObstacle(new Vector3(lanes[obstacleLane].transform.position.x,0f,zPos),Random.Range(0,obstaclePrefabs.Length));
            int zombieLane = 0;
            if (obstacleLane == 0)
            {
                zombieLane = Random.Range(0, 2) == 1 ? 1 : 2;
                ;
            }
            else if (obstacleLane == 1)
            {
                zombieLane = Random.Range(0, 2) == 1 ? 0 : 2;

            }
            if (obstacleLane == 2)
            {
                zombieLane = Random.Range(0, 2) == 1 ? 1 : 0;

            }
            AddZombies(new Vector3(lanes[zombieLane].transform.position.x, 0f, zPos));
        }
    }
        void AddObstacle(Vector3 position, int type)
        {
            GameObject obstacle = Instantiate(obstaclePrefabs[type], position, Quaternion.identity);
            bool mirror = Random.Range(0, 2) == 1;
            switch (type)
            {
                case 0:
                    obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -20 : 20, 0f);
                    break;
                case 1:
                    obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -20 : 20, 0f);
                    break;
                case 2:
                    obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -1 : 1, 0f);
                    break;
                case 3:
                    obstacle.transform.rotation = Quaternion.Euler(0f, mirror ? -170 : 170, 0f);
                    break;
            }
            obstacle.transform.position = position;
        }
    
    void AddZombies(Vector3 pos)
    {
        int count = Random.Range(0, 3) + 1;
        int i;
        for (i = 0; i < count; i++)
        {
            Vector3 shift = new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(1f, 10f) * i);
            Instantiate(zombiePrefabs[Random.Range(0, zombiePrefabs.Length)],
            pos + shift * i, Quaternion.identity);
    


        } 



    }
    public void IncreaseScore()
    {
        zombie_Kill_Count++;
        score_Text.text = zombie_Kill_Count.ToString();
    }
    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;

    }

        public void ResumeGame()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    public void ExitGame()
    {
        
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameover_Panel.SetActive(true);
        final_Score.text = "Killed:"+zombie_Kill_Count;
    }
    public void Restart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Gameplay");

    }

}
