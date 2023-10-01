using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour {

    public int currentDay;

    public List<PossibleElement> possibleElements;

    public List<string> prevSelectedIDs;

    public Transform listOfOptions, timeline;

    public GameObject draggableElement;

    public TMP_Text dayOfWeek;

    public List<GameOverCondition> conditions;

    public GameObject gameOver, win;
    public TMP_Text lostText;

    void Start() {
        AdvanceDay();
    }

    public float HoursUsed() {
        float returnVal = 0;
        foreach (Transform child in timeline) {
            returnVal += child.GetComponent<PossibleElementHolder>().possibleElement.hoursRequired;
        }
        return returnVal;
    }
    
    public void AdvanceDay() {

        currentDay ++;
        UpdateDayOfWeekText();
    
        foreach (Transform child in listOfOptions) {
            Destroy(child.gameObject);
        }
        
        float hoursPassed = 0;
        int numberOfATs = 0;
        foreach (Transform child in timeline) {
            PossibleElement pe = child.GetComponent<PossibleElementHolder>().possibleElement;
            prevSelectedIDs.Add(pe.id);
            if (pe.id == "Australia" && hoursPassed < 8 && hoursPassed > 1) prevSelectedIDs.Add("AusBad");
            if (pe.id == "AT1" && hoursPassed > 8) numberOfATs ++;
            if (pe.id == "AT2" && hoursPassed > 8) numberOfATs ++;
            if (pe.id == "AT3" && hoursPassed > 2 && pe.id == "AT3" && hoursPassed < 6) numberOfATs ++;
            if (pe.id == "AT4" && hoursPassed > 8) numberOfATs ++;
            hoursPassed += pe.hoursRequired;
            Destroy(child.gameObject);
        }

        foreach (GameOverCondition condition in conditions) {
            bool isMet = true;
            foreach (string id in condition.requiredIDs) {
                if (!prevSelectedIDs.Contains(id)) {
                    isMet = false;
                }
                if (id == "2ATs" && numberOfATs < 2) {
                    isMet = false;
                }
            }
            foreach (string id in condition.conflictingIDs) {
                if (prevSelectedIDs.Contains(id)) {
                    isMet = false;
                }
            }
            foreach (string id in condition.requiredIDs) {
                if (id == "2ATs" && numberOfATs < 2) {
                    isMet = true;
                }
            }
            if (currentDay < condition.minDay || currentDay > condition.maxDay) {
                isMet = false;
            }
            if (isMet) {
                gameOver.SetActive(true);
                lostText.text = condition.message;
                return;
            }
        }

        if (currentDay > 7) {
            win.SetActive(true);
            return;
        }

        foreach (PossibleElement pe in possibleElements) {
            bool isUsable = true;
            if (prevSelectedIDs.Contains(pe.id)) {
                isUsable = false;
            }
            if (currentDay < pe.minDay || currentDay > pe.maxDay) {
                isUsable = false;
            }
            foreach (string id in pe.requiredIDs) {
                if (!prevSelectedIDs.Contains(id)) {
                    isUsable = false;
                }
            }
            foreach (string id in pe.conflictingIDs) {
                if (prevSelectedIDs.Contains(id)) {
                    isUsable = false;
                }
            }
            if (isUsable) {
                GameObject go = Instantiate(draggableElement, listOfOptions);
                go.GetComponent<ChildTexts>().header.text = pe.header;
                go.GetComponent<ChildTexts>().body.text = pe.body;
                go.GetComponent<PossibleElementHolder>().possibleElement = pe;
                go.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 65 * pe.hoursRequired);
            }
        }
        
    }

    void UpdateDayOfWeekText() {
        if (currentDay == 2) {
            dayOfWeek.text = "Tuesday";
        }
        if (currentDay == 3) {
            dayOfWeek.text = "Wednesday";
        }
        if (currentDay == 4) {
            dayOfWeek.text = "Thursday";
        }
        if (currentDay == 5) {
            dayOfWeek.text = "Friday";
        }
        if (currentDay == 6) {
            dayOfWeek.text = "Saturday";
        }
        if (currentDay == 7) {
            dayOfWeek.text = "Sunday";
        }
    }

    public void Reset() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit() {
        Application.Quit();
    }

}
