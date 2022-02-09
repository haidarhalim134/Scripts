using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Attributes.Abilities;
using Control.UI;

public class CardSorter
{
    public static List<AbilityContainer> SortByWord(List<AbilityContainer> list)
    {
        return list.OrderBy(cont=>cont.name).ToList();
        // return list.OrderByDescending(cont=>cont.name.Contains("")).ThenBy(cont=>cont.name).ToList();
    }
}
