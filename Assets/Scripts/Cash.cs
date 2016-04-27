using UnityEngine;
using System.Collections;
using System;

public static class Cash
{
    public static string Format(double a_Amount)
    {
        string t_Return = String.Format("{0:C}", (decimal)(a_Amount));

        // TODO There must be a better way.. It probably uses locale.
        return t_Return.Replace('$', Settings.CashSign);
    }
}
