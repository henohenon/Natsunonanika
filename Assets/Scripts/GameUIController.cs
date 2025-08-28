using Cysharp.Threading.Tasks;
using DefaultNamespace;
using LitMotion;
using UnityEngine;
using UnityEngine.UI;
using R3;
using UnityEngine.SceneManagement;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private AudioSource _successSound;
    [SerializeField] private AudioSource _limitSound;
    [SerializeField] private SherveFaseManager _sherveFaseManager;
    
    [SerializeField] private TMPro.TMP_Text bigText;
    [SerializeField] private TMPro.TMP_Text scoreText;
    [SerializeField] private TMPro.TMP_Text timeText;
    [SerializeField] private TMPro.TMP_Text resultText;

    [SerializeField] private UIPopController rotateContainer;
    [SerializeField] private UIPopController decoContainer;
    [SerializeField] private UIPopController completeContainer;
    [SerializeField] private UIPopController resultContainer;
    [SerializeField] private GameObject resultBackground;

    [SerializeField] private Button decoButton;
    [SerializeField] private Button completeButton;
    [SerializeField] private Button resultButton;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotateContainer.SetActive(false);
        decoContainer.SetActive(false);
        completeContainer.SetActive(false);
        resultContainer.SetActive(false);
        resultBackground.gameObject.SetActive(false);

        LMotion.Create(30f, 0f, 30).WithOnComplete(() =>
        {
            _limitSound.Play();
            _sherveFaseManager.enabled = false;
            resultBackground.gameObject.SetActive(true);
            resultContainer.Popup();
        }).Bind(x =>
        {
            timeText.text = x.ToString("0.0");
        });

        _sherveFaseManager.ShavedFirst.Subscribe(isFirst =>
        {
            Debug.Log($"SherveFaseManager.ShavedFirst is {isFirst}");
            if(isFirst) completeContainer.SetActive(false);
            else completeContainer.Popup();
        });
        
        completeButton.onClick.AddListener(() =>
        {
            _successSound.Play();
            AddScore();
            _sherveFaseManager.ReStart();
            completeContainer.Popout();
        });
        
        resultButton.onClick.AddListener(async () =>
        {
            await UniTask.Delay(300);
            SceneManager.LoadScene("Title");
        });

        StartRotate().Forget();
    }

    private async UniTaskVoid StartRotate()
    {
        rotateContainer.Popup();
        await UniTask.Delay(1000);
        rotateContainer.Popout();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int _score = 0;
    private void AddScore()
    {
        var current = _sherveFaseManager.GetScore();
        Debug.Log("AddScore"+ current);
        _score += current;
        scoreText.text = _score.ToString();
        resultText.text = _score.ToString();
    }
}
