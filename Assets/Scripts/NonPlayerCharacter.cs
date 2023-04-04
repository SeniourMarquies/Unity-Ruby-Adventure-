using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;
    public GameObject MessageBox;
    float DisplayTimer;
    
    void Start()
    {
        MessageBox.SetActive(false);
        DisplayTimer = -1.0f;
    }

    void Update()
    {
        if (DisplayTimer >= 0)
        {
            DisplayTimer -= Time.deltaTime;
            if (DisplayTimer < 0)
            {
                MessageBox.SetActive(false);
            }
        }
    }
    
    public void DisplayMessageBox()
    {
        DisplayTimer = displayTime;
        MessageBox.SetActive(true);
    }
}
