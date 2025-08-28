using Cysharp.Threading.Tasks;
using LitMotion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Transform roopObj;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startButton.onClick.AddListener( async () =>
        {
            await UniTask.Delay(300);
            SceneManager.LoadScene("Game");
        });

        LMotion.Create(0f, 360f, 5).WithLoops(-1).Bind(x =>
        {
            roopObj.localRotation = Quaternion.Euler(0f, x, 0f);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
