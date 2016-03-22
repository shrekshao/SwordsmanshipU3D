using UnityEngine;
using System.Collections;

public class TextFlashScript : MonoBehaviour {

    float flashTime = 0.0f;
    //float MAX_FLASH_TIME = 100.0f;
    float SPEED = 0.1f;

    float sign = 1.0f;

    //float MAX_PAUSE_TIME = 10.0f;
    //float pause_time = 0.0f;

    CanvasRenderer canvasRenderer;
    // Use this for initialization
    void Start () {
        canvasRenderer = GetComponent<CanvasRenderer>();

        canvasRenderer.SetAlpha(0.0f);

        enabled = false;

        //tmp
        init();
	}
	
	// Update is called once per frame
	void Update () {

        canvasRenderer.SetAlpha(canvasRenderer.GetAlpha() + SPEED * sign);

        float a = canvasRenderer.GetAlpha();
        if (a >= 3.0f)
        {
            sign = -1.0f;
            //sign = 0.0f;
        }
        else if (a <= 0.0f)
        {
            sign = 1.0f;
            enabled = false;
            init();
        }


        
    }


    public void init()
    {
        this.enabled = true;
        flashTime = 0.0f;
        sign = 1.0f;
    }
    
}
