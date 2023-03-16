using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] counterSelectedVisual;
    
     void Start()
    {
        Player.Instance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangeEventArgs e) {
        if(baseCounter == e.counterSelected) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        foreach(GameObject counter in counterSelectedVisual) {
            counter.SetActive(true);  
        }
    }

    private void Hide() {
        foreach (GameObject counter in counterSelectedVisual) {
            counter.SetActive(false);
        }
    }
}
