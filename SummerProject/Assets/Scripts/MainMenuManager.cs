using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Button startPlayButton , tutorialButton;
    [SerializeField] TextMeshProUGUI Title;
    [SerializeField] Text TutorialText;
    static public bool HeDied = false;
    bool inMenu;
    private void Start()
    {
       
        if (HeDied)
        {
            Title.text = "Play Again!";
            TutorialText.enabled = false;
        }
        inMenu = true;
        StartCoroutine(ChangingColors());
    }
    public void OnClickingTutorialButton() {

        tutorialButton.gameObject.SetActive( false);
        TutorialText.enabled = true;
    }
    public void OnClickingStartPlaybutton()
    {
        float newPosY = 100f;
        float animationTime =0.5f ;

        inMenu = false;
        StopCoroutine(ChangingColors());
        LeanTween.moveY(startPlayButton.gameObject, startPlayButton.transform.position.y - newPosY, animationTime);
        LeanTween.textAlpha(startPlayButton.image.rectTransform, 0, animationTime);
        LeanTween.color(startPlayButton.image.rectTransform, Color.clear, animationTime);
        

        StartCoroutine(DelayTillStart());


    }
    IEnumerator DelayTillStart()
    {
        float timeTillMoveToNextScene = 0.65f;
        yield return new WaitForSeconds(timeTillMoveToNextScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator ChangingColors()
    {
        float timerBetweenChanges = 1f;
        float r = 0, g = 0, b = 0;
        Color randomColor;
        while (inMenu)
        {
            
            if (Mathf.RoundToInt(Time.time) % 2 == 0)
            {
               
                Title.fontStyle = TMPro.FontStyles.Italic;
            }
            else
            {
                Title.fontStyle = TMPro.FontStyles.Normal;
                
            }
            r = Random.Range(0f, 1f);
            g = Random.Range(0f, 1f);
            b = Random.Range(0f, 1f);
            randomColor = new Color(r, g, b, 1);
           
            Title.color = randomColor;
            yield return new WaitForSeconds(timerBetweenChanges);
        }

    }


}
