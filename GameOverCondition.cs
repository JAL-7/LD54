using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class GameOverCondition {
    public int minDay, maxDay;
    public List<string> requiredIDs, conflictingIDs;
    public string message;
}
