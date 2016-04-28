using UnityEngine;
using System.Collections;

public class CreateTeamWindow : MonoBehaviour
{
    public void OnCreate()
    {
        // TODO: Use data to make team.
    }

    public void OnCancel()
    {
        transform.localScale = Vector3.zero;
    }
}
