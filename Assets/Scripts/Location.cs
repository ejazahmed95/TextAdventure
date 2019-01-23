using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Location")]
public class Location : ScriptableObject {

    [TextArea(10,14)][SerializeField] string storyText;
    [SerializeField] Location northLocation;
    [SerializeField] Location eastLocation;
    [SerializeField] Location westLocation;
    [SerializeField] Location southLocation;

    [SerializeField] Action[] actions;

    public string GetLocationStory() {
        return this.storyText;
    }

    // Getters for direction Locations;
    public Location GetNorthLocation() {
        return this.northLocation;
    }
    public Location GetEastLocation() {
        return this.eastLocation;
    }
    public Location GetWestLocation() {
        return this.westLocation;
    }
    public Location GetSouthLocation() {
        return this.southLocation;
    }


    public Action[] GetActions() {
        return this.actions;
    }
}
