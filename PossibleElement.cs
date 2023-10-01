using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class PossbleElement {
    public string id;
    public int minDay, maxDay;
    public List<string> requiredIDs, conflictingIDs;
    public string header, body, person;
    public float hoursRequired;
}
