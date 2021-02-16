using System.Collections.Generic;
using UnityEngine.Analytics;

public class AnalyticsController
{
    public void OnPlayerStartGame()
    {
        Analytics.CustomEvent("Start_Game");
    }
    
    public void OnQuitGame()
    {
        Analytics.CustomEvent("Stop_Game");
    }
    
    public void OnLaunchGame()
    {
        Analytics.CustomEvent("Launch_Game");
    }
    
    public void OnPlayerChangedLevel(int levelNumber)
    {
        Analytics.CustomEvent("Change_Level", new Dictionary<string, object>()
        {
            {"Level", levelNumber}
        });
    }
}
