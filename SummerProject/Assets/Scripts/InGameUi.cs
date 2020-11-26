
using UnityEngine;
using UnityEngine.UI;

public class InGameUi : MonoBehaviour
{
  internal  InGameUi instance;
    [SerializeField] Text arrowText, coinsText;



    [SerializeField] Image heartImage , arrowImage;

    [SerializeField] Sprite[] heartSprites;

    void Awake()
    {
        instance = this;
     
        
    }
    internal void UpdateUI(int _hearts, int _coins, int _arrows)
    {
        heartImage.sprite = heartSprites[_hearts];
        arrowText.text = _arrows.ToString();
        coinsText.text = _coins + " / 10";

    }
}
