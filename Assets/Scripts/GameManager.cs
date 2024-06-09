using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private GameObject[] Balls;
    [SerializeField] private GameObject FirePoint;
    [SerializeField] private float Power;
    int ActiveBallIndex;
    [SerializeField] private Animator BallShooter;
    [SerializeField] private ParticleSystem BallEffect;
    [SerializeField] private ParticleSystem[] BallEffects;
    int ActiveBallEffectýndex;
    [SerializeField] private AudioSource[] BallSounds;
    int ActiveBallSoundýndex;
    [Header("Level Settings")]
    [SerializeField] private int TargetBallCount; // oyunu kazanmak için gereken sayý
    [SerializeField] private int CurrentBallCount; // oyun baþýnda oluþacak toplam top sayýsý 
    int SuccessBallCount;
    [SerializeField] private Slider LevelSlider;
    [SerializeField] private TextMeshProUGUI RemainingBalls;
    [Header("Panel Settings")]
    [SerializeField] private GameObject[] Panels;
    [SerializeField] private TextMeshProUGUI StarText;
    [SerializeField] private TextMeshProUGUI WinLevelCount;
    [SerializeField] private TextMeshProUGUI LoseLevelCount;
    [Header("Other Settings")]
    [SerializeField] private Renderer TransparentBucket;
    float BucketInitialValue;
    float BucketStepValue;
    [SerializeField] private AudioSource[] OtherSound;

    string LevelName;

    private void Start()
    {
        ActiveBallIndex = 0;
        ActiveBallSoundýndex = 0;
        LevelName = SceneManager.GetActiveScene().name;
        //kovanýn ilerleme görsel iþlemleri
        BucketInitialValue = 0.5f;
        BucketStepValue = .25f / TargetBallCount;

        LevelSlider.maxValue = TargetBallCount;
        RemainingBalls.text = CurrentBallCount.ToString();
    }
    public void BallEntered()
    {
        SuccessBallCount++;
        LevelSlider.value = SuccessBallCount;
        BucketInitialValue -= BucketStepValue;

        TransparentBucket.material.SetTextureScale("_MainTex", new Vector2(1f, BucketInitialValue));

        BallSounds[ActiveBallSoundýndex].Play();
        ActiveBallSoundýndex++;
        if (ActiveBallSoundýndex == BallSounds.Length - 1)
        {
            ActiveBallSoundýndex = 0;
        }

        if (SuccessBallCount == TargetBallCount)
        { 
            Time.timeScale = 0;
            OtherSound[1].Play();
            PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("Star", PlayerPrefs.GetInt("Star") + 10);
            StarText.text = PlayerPrefs.GetInt("Star").ToString();
            WinLevelCount.text = SceneManager.GetActiveScene().name;
            Panels[1].SetActive(true);
        }
        int number=0;
        foreach (var item in Balls )
        {
            if (item.activeInHierarchy)
            {
                 number++;
            }
        }
        if(number == 0)
        {
            if (CurrentBallCount == 0 && SuccessBallCount != TargetBallCount)
            {
                Lose();
            }
            if ((CurrentBallCount + SuccessBallCount) < TargetBallCount)
            {
                Lose();
            }
        }
    }
    public void BallNotEntered()
    {
        int number = 0;
        foreach (var item in Balls)
        {
            if (item.activeInHierarchy)
            {
                number++;
            }
        }
        if (number == 0)
        {
            if (CurrentBallCount == 0)
            {
                Lose();
            }
            if ((CurrentBallCount + SuccessBallCount) < TargetBallCount)
            {
                Lose();
            }
        }
    }
    private void Update()
    {
        if (Time.timeScale!=0)
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                CurrentBallCount--;
                RemainingBalls.text = CurrentBallCount.ToString();
                BallShooter.Play("BallShooter");
                BallEffect.Play();
                OtherSound[2].Play();
                Balls[ActiveBallIndex].transform.SetPositionAndRotation(FirePoint.transform.position, FirePoint.transform.rotation);
                Balls[ActiveBallIndex].SetActive(true);
                Balls[ActiveBallIndex].GetComponent<Rigidbody>().AddForce(Balls[ActiveBallIndex].transform.TransformDirection(90, 90, 0) * Power, ForceMode.Force);

                if (Balls.Length - 1 == ActiveBallIndex)
                {
                    ActiveBallIndex = 0;
                }
                else
                {
                    ActiveBallIndex++;
                }
            }
        }
    }
    public void Stop()
    {
        Time.timeScale = 0;
        Panels[0].SetActive(true);
    }
    public void PanelProcess(string Process)
    {
        switch (Process)
        {
            case "Resume":
                Time.timeScale = 1;
                Panels[0].SetActive(false);
                break;
            case "Quit":
                Application.Quit();
                break;
            case "Settings":

                break;
            case "Retry":
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case "Next":
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }
    public void ParcEffect(Vector3 position, Color color)
    {
        BallEffects[ActiveBallEffectýndex].transform.position = position;
        var main = BallEffects[ActiveBallEffectýndex].main;
        main.startColor = color;
        BallEffects[ActiveBallEffectýndex].gameObject.SetActive(true);
        ActiveBallEffectýndex++;
        if (ActiveBallEffectýndex == BallEffects.Length)
        {
            ActiveBallEffectýndex = 0;
        }
    }
    void Lose()
    {
        OtherSound[0].Play();
        Panels[2].SetActive(true);
        LoseLevelCount.text = "LEVEL : " + LevelName;
        Time.timeScale = 0;
    }
}
