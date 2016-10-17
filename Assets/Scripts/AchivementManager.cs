using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class AchivementManager : MonoBehaviour
{
    /*
	void Start ()
	{
        Social.localUser.Authenticate(ProcessAuthentication);

        IAchievement achievement = Social.CreateAchievement();
        achievement.id = "Jumper";
        achievement.percentCompleted = 100.0;

	    SteamUserStats.SetAchievement("Jumper");
        SteamUserStats.ach
        SteamAPI.RunCallbacks();
	}

    void ProcessAuthentication(bool success)
    {
        if (success)
        {
            Debug.Log("Authenticated, checking achievements");

            Social.LoadAchievements(ProcessLoadedAchievements);
        }
        else
        {
            Debug.Log("Failed to authenticate");
        }
    }

    void ProcessLoadedAchievements(IAchievement[] achievements)
    {
        if (achievements.Length == 0)
            Debug.Log("Error: no achievements found");
        else
            Debug.Log("Got " + achievements.Length + " achievements");
        
        Social.ReportProgress("Jumper", 100.0, result => 
        {
            if (result)
            {
                Debug.Log("Successfully reported achievement progress");
            }
            else
            {
                Debug.Log("Failed to report achievement");
            }
        });
    }
    */
}
