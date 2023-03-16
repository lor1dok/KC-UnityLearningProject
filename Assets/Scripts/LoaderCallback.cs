using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour {

    private bool IsFirstFrame = true;

    private void Update() {
        if(IsFirstFrame) {
            IsFirstFrame = false;

            Loader.LoaderCallback();
        }
    }
}
