﻿/*===============================================================================
Copyright (c) 2015-2016 PTC Inc. All Rights Reserved.
 
Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vuforia;

public class TrackableSettings : MonoBehaviour
{
    #region PROTECTED_MEMBERS
    protected bool mExtTrackingEnabled = false;
    #endregion //PROTECTED_MEMBERS


    #region PUBLIC_METHODS
    public bool IsExtendedTrackingEnabled()
    {
        return mExtTrackingEnabled;
    }

    /// <summary>
    /// Enabled/disabled Extended Tracking mode.
    /// </summary>
    /// <param name="ON"></param>
    /// 

   

    public string GetActiveDatasetName()
    {
        ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        List<DataSet> activeDataSets = tracker.GetActiveDataSets().ToList();
        if (activeDataSets.Count > 0)
        {
            string datasetPath = activeDataSets.ElementAt(0).Path;
            string datasetName = datasetPath.Substring(datasetPath.LastIndexOf("/") + 1);
            return datasetName.TrimEnd(".xml".ToCharArray());
        }
        else
        {
            return string.Empty;
        }
    }

    public void ActivateDataSet(string datasetName)
    {
        // ObjectTracker tracks ImageTargets contained in a DataSet and provides methods for creating and (de)activating datasets.
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        IEnumerable<DataSet> datasets = objectTracker.GetDataSets();

        IEnumerable<DataSet> activeDataSets = objectTracker.GetActiveDataSets();
        List<DataSet> activeDataSetsToBeRemoved = activeDataSets.ToList();

        // 1. Loop through all the active datasets and deactivate them.
        foreach (DataSet ads in activeDataSetsToBeRemoved)
        {
            objectTracker.DeactivateDataSet(ads);
        }

        // Swapping of the datasets should NOT be done while the ObjectTracker is running.
        // 2. So, Stop the tracker first.
        objectTracker.Stop();

        // 3. Then, look up the new dataset and if one exists, activate it.
        foreach (DataSet ds in datasets)
        {
            if (ds.Path.Contains(datasetName))
            {
                objectTracker.ActivateDataSet(ds);
            }
        }

        // 4. Finally, restart the object tracker.
        objectTracker.Start();
    }
    #endregion //PUBLIC_METHODS
}
