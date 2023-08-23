using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpQuiz : MonoBehaviour
{
    public HealthSystem healthSystem;
    public float time = 120f;
    private List<int> numbers = new List<int>() { 0, 1, 2, 3, 4 };
    public TMP_Text question;
    public TMP_Text[] answerText;
    public TMP_Text popupText;
    private int mark;
    void Start()
    {        
        StartCoroutine(ActivateEveryTwoMinutes());
    }

    IEnumerator ActivateEveryTwoMinutes()
    {
        ChangeIndex(GenerateRandomNumber());
        yield return new WaitForSeconds(time);
        
        gameObject.SetActive(true);
        StartCoroutine(ActivateEveryTwoMinutes());
    }
    void ChangeIndex(int x) {
        index = x;
        question.SetText( questions[x]);
        for (int i = 0; i < 4; i++) {
            answerText[i].SetText( answers[i, x]);
        }
    }
    private string[] questions = { "Which of the following is a way to become healthy?", "Which of the following is a symptom of COVID-19","Which of the following is the function of platelet?"
            ,"Which of the following is a example of virus?","Which of the following you should do when you feel sick?"};
    public string[,] answers = new string[,]
    { { " Meditation","Fever","Healing wound" ,"Salmonella","Get less sleep"} ,
        { "Smoking","Strep Throat ","Stop bleeding","Lactobacillus","Ignore it"}, 
        { "Eating junk food","Allergies","Fighting infection on their own","Flu","Exercise"} ,
        { "Drinking alcohol","Asthma","Carrying oxygens", "HIV","Drink more water"}
    };
    public int[,] answerMark = new int[,] { 
        { 10,10,10,-10,-10 } ,
        {-10,-10,10,-10,-10}, 
        { -10,-10,-10,10,-10} ,
        {  -10,-10,-10,10,10} };
    private int index =0;

    public void Change1()
    {
        mark = answerMark[0, index];
        healthSystem.Change(answerMark[0,index]);
        gameObject.SetActive(false);
        Showmark();
    }

    public void Change2()
    {
        mark = answerMark[1, index];
        healthSystem.Change(answerMark[1,index]);
        gameObject.SetActive(false);
        Showmark();
    }

    public void Change3()
    {
        mark=answerMark[2, index];
        healthSystem.Change(answerMark[2,index]);
        gameObject.SetActive(false);
        Showmark();
    }

    public void Change4()
    {
        mark=answerMark[3, index];
        healthSystem.Change(answerMark[3,index]);
        gameObject.SetActive(false);
        Showmark();
    }
    void Showmark() {
        if (mark > 0) {
            Instantiate(popupText, transform.parent).SetText("Correct! Health Index+"+mark.ToString());
        }
        else
            Instantiate(popupText,  transform.parent).SetText("Incorrect! Health Index" + mark.ToString());
    }
    public int GenerateRandomNumber()
    {
        if (numbers.Count == 0)
        {
            Debug.Log("All numbers have been generated.");
            return -1;
        }

        int index = Random.Range(0, numbers.Count);
        int number = numbers[index];
        numbers.RemoveAt(index);

        return number;
    }
}
