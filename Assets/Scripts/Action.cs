using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action")]
public class Action : ScriptableObject {

    [TextArea(3, 5)] [SerializeField] string description;
    [TextArea(3, 5)] [SerializeField] string successText;
    [TextArea(3, 5)] [SerializeField] string failureText;

    public enum Status { Open, Hidden, Success, Failed, Abandoned };

    [SerializeField] Status status;

    public bool isRepetitive = false;

    // Required actions to be successfully completed
    [SerializeField] Action[] succeededActions;
    //Requried actions to have failed
    [SerializeField] Action[] failedActions;
    //Actions which should be made unavailable, includes self for actionsToAbandon
    [SerializeField] Action[] actionsToAbandon;

    public string GetDescription() {
        return this.description;
    }
    public string GetSuccessText() {
        return this.successText;
    }
    public string GetFailureText() {
        return this.failureText;
    }

    public Status GetStatus() {
        return this.status;
    }

    public Action[] GetSuccessfulActionsNeeded() {
        return this.succeededActions;
    }

    public Action[] GetFailedActionsNeeded() {
        return this.failedActions;
    }

    public Action[] GetActionsToAbandon() {
        return this.actionsToAbandon;
    }

    public void SetStatus(Status status) {
        this.status = status;
    }

}
