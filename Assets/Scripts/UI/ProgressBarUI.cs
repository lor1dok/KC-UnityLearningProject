using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        hasProgress.OnProgressChange += HasProgress_OnProgressChange;
        barImage.fillAmount = 0;  
        Hide();
    }

    private void HasProgress_OnProgressChange(object sender, IHasProgress.OnProgressChangeEventArgs e) {
        barImage.fillAmount = e.progressNormalized;
        if(e.progressNormalized == 0f || e.progressNormalized >= 1f) {
            Hide();
        } else {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
