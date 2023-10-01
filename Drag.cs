using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

    public GameManager gameManager;
    public Transform canvas, list;
    public RectTransform target, targetList;
    public GameObject errorText;

    public void OnBeginDrag(PointerEventData eventData) {
        transform.SetParent(canvas);
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = eventData.position;
    }
    
    public void OnEndDrag(PointerEventData eventData) {
        Vector3[] v = new Vector3[4];
        target.GetWorldCorners(v);
        if (eventData.position.x > v[1].x && eventData.position.y < v[1].y && eventData.position.x < v[3].x && eventData.position.y > v[3].y) {
            if (gameManager.HoursUsed() + GetComponent<PossibleElementHolder>().possibleElement.hoursRequired > 11) {
                transform.SetParent(list);
                Transform t = Instantiate(errorText, canvas).transform;
                t.SetAsLastSibling();
                t.position = Input.mousePosition;
                t.GetChild(0).GetComponent<TMP_Text>().text = "Not enough space";
                return;
            }
            int i = 0;
            foreach (RectTransform child in targetList) {
                child.GetWorldCorners(v);
                if (eventData.position.y < ((v[3].y + v[1].y) / 2)) {
                    i ++;
                }
            }
            transform.SetParent(targetList);
            transform.SetSiblingIndex(i);
        }
        else {
            transform.SetParent(list);
        }
    }

}
