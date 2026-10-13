using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameData", menuName = "Universal Data")]
public class UniversalGM : ScriptableObject
{
    //THIS SCRIPTABLEOBJECT WILL SERVE AS A GAME MANAGER THAT CARRIES DATA ACROSS SCENES FROM MAIN MENU TO GAME SCENE


    //SAVES THE TEAM DATA//
    [SerializeField] public Dictionary<int, TeamChoice.Teams> UniversalTeamData = new Dictionary<int, TeamChoice.Teams>();



    //FUNCTION TO SAVE THE TEAMS//
    public void SaveTeams(int Playerindex, TeamChoice.Teams Teams)
    {

        if(!UniversalTeamData.ContainsKey(Playerindex))
        {
            
            UniversalTeamData.Add(Playerindex, Teams);



        }


        else
        {

            UniversalTeamData[Playerindex] = Teams;   


        }


    }


}
