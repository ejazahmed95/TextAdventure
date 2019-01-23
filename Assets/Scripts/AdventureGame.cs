using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour {

    [SerializeField] Text storyText;
    [SerializeField] Text actionsText;
    [SerializeField] Location firstLocation;

    Location location, nextLocation;

    Dictionary<int, Action.Status> actionStatuses = new Dictionary<int, Action.Status>();

    List<Action> currentActions;
    KeyCode[] actionKeys;
    // Use this for initialization
    void Start () {
        actionKeys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };
        location = firstLocation;
        nextLocation = firstLocation;
        storyText.text = location.GetLocationStory();
        currentActions = new List<Action>();
        FillActions(location);
	}
	
	// Update is called once per frame
	void Update () {
        ManageState();
	}

    private void ManageState() {

        nextLocation = location;
        // N, E, W, S directions
        if(Input.GetKeyDown(KeyCode.N)) {
            nextLocation = location.GetNorthLocation();
        } else if(Input.GetKeyDown(KeyCode.E)) {
            nextLocation = location.GetEastLocation();
        } else if(Input.GetKeyDown(KeyCode.W)) {
            nextLocation = location.GetWestLocation();
        } else if(Input.GetKeyDown(KeyCode.S)) {
            nextLocation = location.GetSouthLocation();
        }

        if (nextLocation != null && nextLocation != location) {
			Debug.Log("Location Changed");
            location = nextLocation;
            storyText.text = location.GetLocationStory();
            FillActions(location);
        } else if(nextLocation == null) {
            Debug.Log("You can't go that way");
            nextLocation = location;
        }

        for(int i=0;i<currentActions.Count;i++) {
            if(i>=5) {
                Debug.LogError("More than 5 actions are not supported");
                break;
            }

            if(Input.GetKeyDown(actionKeys[i])) {
                PerformAction(currentActions[i]);
                break;
            }
        }
        
    }

    private void PerformAction(Action action) {
		Debug.Log("Performing Action");
		if (!action.isRepetitive) {
			actionStatuses[action.GetInstanceID()] = Action.Status.Success;
		}
		Action[] abandonActions = action.GetActionsToAbandon();
		foreach(Action act in abandonActions) {
			actionStatuses[act.GetInstanceID()] = Action.Status.Abandoned;
		}
        FillActions(location);
    }

    private void FillActions(Location location) {
        currentActions.Clear();
        Action[] locActions = location.GetActions();
        Action.Status status;
        Action action;
        for(int i=0;i<locActions.Length;i++) {
            action = locActions[i];
            
            status = FindStatus(action);
            if(status == Action.Status.Open) {
                currentActions.Add(action);
            }
            if(!actionStatuses.ContainsKey(action.GetInstanceID())) {
                actionStatuses.Add(action.GetInstanceID(), status);
            } else {
                actionStatuses[action.GetInstanceID()] = status;
            }
        }
        SetActionText();
		Debug.Log("-------------------------------");
		foreach (int key in actionStatuses.Keys) {
			Debug.Log(key + " : " + actionStatuses[key]);
		}
		Debug.Log("-------------------------------");
	}

    private Action.Status FindStatus(Action action) {
        Action.Status status;
        Action[] succeededActions = action.GetSuccessfulActionsNeeded();
        Action[] failedActions = action.GetFailedActionsNeeded();

		if (actionStatuses.TryGetValue(action.GetInstanceID(), out status)) {
			if (status != Action.Status.Open && status != Action.Status.Hidden) {
				return status;
			}
		}
        
        
        foreach (Action act in succeededActions) {
			if (actionStatuses.TryGetValue(act.GetInstanceID(), out status)) {
				if (status != Action.Status.Abandoned && status != Action.Status.Success) {
					return Action.Status.Hidden;
				} else {
					Debug.Log("This action should have open status now");
				}
			} else {
				return Action.Status.Hidden;
			}
		}

        foreach (Action act in failedActions) {
            if (actionStatuses.TryGetValue(act.GetInstanceID(), out status)) {
                if (status != Action.Status.Abandoned && status != Action.Status.Failed) {
                    return Action.Status.Hidden;
                }
            } else return Action.Status.Hidden;
        }

        return Action.Status.Open;
    }

    private void SetActionText() {
        string text = "";
        for(int i=0; i<currentActions.Count;i++) {
            text = text + (i + 1).ToString() + ". ";
            text = text + currentActions[i].GetDescription();
            text = text + "\n";
        }
        actionsText.text = text;
    }

}
