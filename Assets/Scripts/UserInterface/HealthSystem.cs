using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Fusion;

public class HealthSystem : MonoBehaviour
{
    private GameLevel gamelevel;
    private int number = 5;
    private Sprite[] emoji = { null, null, null };
    private Image image;
    private TMP_Text text;
    private string string1 = "Health index:";
    private string string2 = "Your body is sick. Go to health center to make your body healthy.";
    private string string3 = "Your body is subhealth. You can do more to be healthy ";

    private string string4 = "Your body is healthy now! remember to keep your body healthy.";

    // Start is called before the first frame update
    void Start()
    {
        gamelevel = GameLevel.Instance;
        emoji[0] = Resources.Load<Sprite>("Arts/UI/health1");
        emoji[1] = Resources.Load<Sprite>("Arts/UI/health2");
        emoji[2] = Resources.Load<Sprite>("Arts/UI/health3");
        image = transform.GetChild(0).GetComponent<Image>();
        text = transform.GetChild(transform.childCount - 1).GetChild(0).GetComponent<TMP_Text>();
        StartCoroutine(Decrease());
        StartCoroutine(HealthIndexUpdate());
    }

    // Update is called once per frame
    void Update()
    {
    }

   

    public void Change(int value)
    {
        GameLevel.Instance.AddHealthIndex(value);
        if (GameLevel.Instance.HealthIndex > 100)
        {
            GameLevel.Instance.SetHealthIndex(100);
        }
        else if (GameLevel.Instance.HealthIndex < 0)
        {
            GameLevel.Instance.SetHealthIndex(0);
        }

        if (GameLevel.Instance.HealthIndex > 66)
        {
            image.sprite = emoji[2];
            text.SetText(string1 + GameLevel.Instance.HealthIndex.ToString() + "\n" + string4);
        }
        else if (GameLevel.Instance.HealthIndex < 33)
        {
            image.sprite = emoji[0];
            text.SetText(string1 + GameLevel.Instance.HealthIndex.ToString() + "\n" + string2);
        }
        else
        {
            image.sprite = emoji[1];
            text.SetText(string1 + GameLevel.Instance.HealthIndex.ToString() + "\n" + string3);
        }

        Pathogen[] pathogens = FindObjectsOfType<Pathogen>();
        foreach (Pathogen pathogen in pathogens)
        {
            int temp = pathogen.maxHP;
            pathogen.maxHP = (int)(pathogen.getInitialHP() * (((50f - GameLevel.Instance.HealthIndex) / 100f) + 1));
            pathogen.TakeDamageWithoutPlayerRef(temp - pathogen.maxHP);
        }
    }

    private IEnumerator Decrease()
    {
        yield return new WaitForSeconds(60);
        Change(-number);
        if (number < 30)
            number += 5;
        StartCoroutine(Decrease());
    }

    private IEnumerator HealthIndexUpdate()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(HealthIndexUpdate());
        if (GameLevel.Instance.HealthIndex > 66)
        {
            image.sprite = emoji[2];
            text.SetText(string1 + GameLevel.Instance.HealthIndex.ToString() + "\n" + string4);
        }
        else if (GameLevel.Instance.HealthIndex < 33)
        {
            image.sprite = emoji[0];
            text.SetText(string1 + GameLevel.Instance.HealthIndex.ToString() + "\n" + string2);
        }
        else
        {
            image.sprite = emoji[1];
            text.SetText(string1 + GameLevel.Instance.HealthIndex.ToString() + "\n" + string3);
        }
    }
}