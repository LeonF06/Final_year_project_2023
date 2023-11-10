using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class MulScript : MonoBehaviour
{
    private bool ActivateStar = true;
    public TMP_Text headerText;

    // Star images
    public Image L1S1;
    public Image L1S2;
    public Image L1S3;
    public Image L2S1;
    public Image L2S2;
    public Image L2S3;
    public Image L3S1;
    public Image L3S2;
    public Image L3S3;
    public Image L4S1;
    public Image L4S2;
    public Image L4S3;
    public Image L5S1;
    public Image L5S2;
    public Image L5S3;
    public Image L6S1;
    public Image L6S2;
    public Image L6S3;
    public Image L7S1;
    public Image L7S2;
    public Image L7S3;
    public Image L8S1;
    public Image L8S2;
    public Image L8S3;
    public Image L9S1;
    public Image L9S2;
    public Image L9S3;
    public Image L10S1;
    public Image L10S2;
    public Image L10S3;


    private StarManager starManager; // Add a reference to the StarManager
    private SpriteManager spriteManager; // Add a reference to the SpriteManager

    private void Awake()
    {
        // Find the existing StarManager instance in the scene
        starManager = FindObjectOfType<StarManager>();

        if (starManager == null)
        {
            Debug.LogError("StarManager not found in the scene!");
        }

        // Find the existing SpriteManager instance in the scene
        spriteManager = FindObjectOfType<SpriteManager>();

        if (spriteManager == null)
        {
            Debug.LogError("SpriteManager not found in the scene!");
        }
    }

    private void Start()
    {
        LoadStarImages();
    }

    public void OnL1Click()
    {
        // Initialize the race number
        spriteManager.InitializeRaceNumber(1);
        // Load the plus level 1 scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("First level mul");
    }

    public void OnL2Click()
    {
        // Initialize the race number
        spriteManager.InitializeRaceNumber(2);

        if (ActivateStar == true)
        {
            // If the highestStarCounts for the first race is greater than 0, then load the plus level 2 scene
            if (starManager.highestStarCountsMul[0] > 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Second level mul");
            }
            else
            {
                // Display error message on the header text
                headerText.text = "You must earn at least one star in the first race to unlock the next race.";
            }
        }
        else
        {
            // Load the plus level 2 scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Second level mul");
        }
    }

    public void OnL3Click()
    {
        // Initialize the race number
        spriteManager.InitializeRaceNumber(3);

        if (ActivateStar == true)
        {
            // If the higestStarCount for the second race is greater than 0, then load the plus level 3 scene
            if (starManager.highestStarCountsMul[1] > 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Third level mul");
            }
            else
            {
                // Display error message on the header text
                headerText.text = "You must earn at least one star in the second race to unlock the next race.";
            }
        }
        else
        {
            // Load the plus level 3 scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Third level mul");
        }
    }

    public void OnL4Click()
    {
        // Initialize the race number
        spriteManager.InitializeRaceNumber(4);

        if (ActivateStar == true)
        {
            // If the higestStarCount for the third race is greater than 0, then load the plus level 4 scene
            if (starManager.highestStarCountsMul[2] > 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Fourth level mul");
            }
            else
            {
                // Display error message on the header text
                headerText.text = "You must earn at least one star in the third race to unlock the next race";
            }
        }
        else
        {
            // Load the plus level 4 scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Fourth level mul");
        }
    }

    public void OnL5Click()
    {
        // Initialize the race number
        spriteManager.InitializeRaceNumber(5);

        if (ActivateStar == true)
        {
            // If the higestStarCount for the fourth race is greater than 0, then load the plus level 5 scene
            if (starManager.highestStarCountsMul[3] > 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Fifth level mul");
            }
            else
            {
                // Display error message on the header text
                headerText.text = "You must earn at least one star in the fourth race to unlock the next race.";
            }
        }
        else
        {
            // Load the plus level 5 scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Fifth level mul");
        }
    }

    public void OnL6Click()
    {
        // Initialize the race number
        spriteManager.InitializeRaceNumber(6);

        if (ActivateStar == true)
        {
            // If the higestStarCount for the fifth race is greater than 0, then load the plus level 6 scene
            if (starManager.highestStarCountsMul[4] > 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Sixth level mul");
            }
            else
            {
                // Display error message on the header text
                headerText.text = "You must earn at least one star in the fifth race to unlock the next race.";
            }
        }
        else
        {
            // Load the plus level 6 scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Sixth level mul");
        }
    }

    public void OnL7Click()
    {
        // Initialize the race number
        spriteManager.InitializeRaceNumber(7);

        if (ActivateStar == true)
        {
            // If the higestStarCount for the sixth race is greater than 0, then load the plus level 7 scene
            if (starManager.highestStarCountsMul[5] > 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Seventh level mul");
            }
            else
            {
                // Display error message on the header text
                headerText.text = "You must earn at least one star in the sixth race to unlock the next race.";
            }
        }
        else
        {
            // Load the plus level 7 scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Seventh level mul");
        }
    }

    public void OnL8Click()
    {
        // Initialize the race number
        spriteManager.InitializeRaceNumber(8);

        if (ActivateStar == true)
        {
            // If the higestStarCount for the seventh race is greater than 0, then load the plus level 8 scene
            if (starManager.highestStarCountsMul[6] > 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Eighth level mul");
            }
            else
            {
                // Display error message on the header text
                headerText.text = "You must earn at least one star in the seventh race to unlock the next race.";
            }
        }
        else
        {
            // Load the plus level 8 scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Eighth level mul");
        }
    }

    public void OnL9Click()
    {
        // Initialize the race number
        spriteManager.InitializeRaceNumber(9);

        if (ActivateStar == true)
        {
            // If the higestStarCount for the eighth race is greater than 0, then load the plus level 9 scene
            if (starManager.highestStarCountsMul[7] > 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Ninth level mul");
            }
            else
            {
                // Display error message on the header text
                headerText.text = "You must earn at least one star in the eighth race to unlock the next race.";
            }
        }
        else
        {
            // Load the plus level 9 scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ninth level mul");
        }
    }

    public void OnL10Click()
    {
        // Initialize the race number
        spriteManager.InitializeRaceNumber(10);
        
        if (ActivateStar == true)
        {
            // Loop through the highestStarCounts array and sum up the values
            int totalStars = 0;
            for (int i = 0; i < starManager.highestStarCountsMul.Length; i++)
            {
                totalStars += starManager.highestStarCountsMul[i];
            }

            if (totalStars != 27)
            {
                headerText.text = "You must earn 3 stars in each level to unlock the boss level.";
            }
            else
            {
                // Load the plus boss level scene
                UnityEngine.SceneManagement.SceneManager.LoadScene("Boss level mul");
            }
        }
        else
        {
            // Load the plus boss level scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("Boss level mul");
        }
    }

    private void LoadStarImages()
    {
        DisableAllStars();

        // First level
        if (starManager.highestStarCountsMul[0] == 1)
        {
            L1S1.enabled = true;
        }
        else if (starManager.highestStarCountsMul[0] == 2)
        {
            L1S1.enabled = true;
            L1S2.enabled = true;
        }
        else if (starManager.highestStarCountsMul[0] == 3)
        {
            L1S1.enabled = true;
            L1S2.enabled = true;
            L1S3.enabled = true;
        }

        // Second level
        if (starManager.highestStarCountsMul[1] == 1)
        {
            L2S1.enabled = true;
        }
        else if (starManager.highestStarCountsMul[1] == 2)
        {
            L2S1.enabled = true;
            L2S2.enabled = true;
        }
        else if (starManager.highestStarCountsMul[1] == 3)
        {
            L2S1.enabled = true;
            L2S2.enabled = true;
            L2S3.enabled = true;
        }

        // Third level
        if (starManager.highestStarCountsMul[2] == 1)
        {
            L3S1.enabled = true;
        }
        else if (starManager.highestStarCountsMul[2] == 2)
        {
            L3S1.enabled = true;
            L3S2.enabled = true;
        }
        else if (starManager.highestStarCountsMul[2] == 3)
        {
            L3S1.enabled = true;
            L3S2.enabled = true;
            L3S3.enabled = true;
        }

        // Fourth level
        if (starManager.highestStarCountsMul[3] == 1)
        {
            L4S1.enabled = true;
        }
        else if (starManager.highestStarCountsMul[3] == 2)
        {
            L4S1.enabled = true;
            L4S2.enabled = true;
        }
        else if (starManager.highestStarCountsMul[3] == 3)
        {
            L4S1.enabled = true;
            L4S2.enabled = true;
            L4S3.enabled = true;
        }

        // Fifth level
        if (starManager.highestStarCountsMul[4] == 1)
        {
            L5S1.enabled = true;
        }
        else if (starManager.highestStarCountsMul[4] == 2)
        {
            L5S1.enabled = true;
            L5S2.enabled = true;
        }
        else if (starManager.highestStarCountsMul[4] == 3)
        {
            L5S1.enabled = true;
            L5S2.enabled = true;
            L5S3.enabled = true;
        }

        // Sixth level
        if (starManager.highestStarCountsMul[5] == 1)
        {
            L6S1.enabled = true;
        }
        else if (starManager.highestStarCountsMul[5] == 2)
        {
            L6S1.enabled = true;
            L6S2.enabled = true;
        }
        else if (starManager.highestStarCountsMul[5] == 3)
        {
            L6S1.enabled = true;
            L6S2.enabled = true;
            L6S3.enabled = true;
        }

        // Seventh level
        if (starManager.highestStarCountsMul[6] == 1)
        {
            L7S1.enabled = true;
        }
        else if (starManager.highestStarCountsMul[6] == 2)
        {
            L7S1.enabled = true;
            L7S2.enabled = true;
        }
        else if (starManager.highestStarCountsMul[6] == 3)
        {
            L7S1.enabled = true;
            L7S2.enabled = true;
            L7S3.enabled = true;
        }

        // Eighth level
        if (starManager.highestStarCountsMul[7] == 1)
        {
            L8S1.enabled = true;
        }
        else if (starManager.highestStarCountsMul[7] == 2)
        {
            L8S1.enabled = true;
            L8S2.enabled = true;
        }
        else if (starManager.highestStarCountsMul[7] == 3)
        {
            L8S1.enabled = true;
            L8S2.enabled = true;
            L8S3.enabled = true;
        }

        // Ninth level
        if (starManager.highestStarCountsMul[8] == 1)
        {
            L9S1.enabled = true;
        }
        else if (starManager.highestStarCountsMul[8] == 2)
        {
            L9S1.enabled = true;
            L9S2.enabled = true;
        }
        else if (starManager.highestStarCountsMul[8] == 3)
        {
            L9S1.enabled = true;
            L9S2.enabled = true;
            L9S3.enabled = true;
        }

        //Debug.Log("MulScript StarManager.BossMul: " + starManager.BossMin);
        // Boss level
        if (starManager.BossMul == true)
        {
            L10S1.enabled = true;
            L10S2.enabled = true;
            L10S3.enabled = true;
        }
    }

    private void DisableAllStars()
    {
        L1S1.enabled = false;
        L1S2.enabled = false;
        L1S3.enabled = false;
        L2S1.enabled = false;
        L2S2.enabled = false;
        L2S3.enabled = false;
        L3S1.enabled = false;
        L3S2.enabled = false;
        L3S3.enabled = false;
        L4S1.enabled = false;
        L4S2.enabled = false;
        L4S3.enabled = false;
        L5S1.enabled = false;
        L5S2.enabled = false;
        L5S3.enabled = false;
        L6S1.enabled = false;
        L6S2.enabled = false;
        L6S3.enabled = false;
        L7S1.enabled = false;
        L7S2.enabled = false;
        L7S3.enabled = false;
        L8S1.enabled = false;
        L8S2.enabled = false;
        L8S3.enabled = false;
        L9S1.enabled = false;
        L9S2.enabled = false;
        L9S3.enabled = false;
        L10S1.enabled = false;
        L10S2.enabled = false;
        L10S3.enabled = false;
    }

    // Onclick event for back button
    public void OnBackClick()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Maps menu");
    }
}
