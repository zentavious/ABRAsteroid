/* CSci-5609 Support Code created by Prof. Dan Keefe, Fall 2023 */

using System.Collections.Generic;
using System.IO;
using UnityEngine;

using IVLab.ABREngine;

/// <summary>
/// This class makes it possible for DataImpressions designed with ABR Compose to be used with Time-Varying datasets.
/// The approach is to think of the visualization designed in ABR Compose as a "template".  On Startup(), the ABREngine
/// in Unity creates DataImpressions based on the specification from ABR Compose as usual using whatever KeyData is
/// specified for those DataImpressions in ABR Compose.  Meanwhile, this class loads a series of similar KeyData
/// objects from files that are numbered based on their timestep ID.  Then, on Update(), this class handles replacing
/// the original KeyData with timestep-specific KeyData to reflect the current timestep, which can be changed by
/// calling the SetTimestep() function.
///
/// For all of the functions in the class, the timestep number is based on the original numbering of the data files.
/// So, if you load 6 data files, but the last file was for timestep #74 in the simulation, you should access the
/// data using that original timestep number of 74, not 6.
/// </summary>
public class ABRTimestepSelector : MonoBehaviour
{
    [Header("Template DataImpression")]

    [Tooltip("This string identifies the DataImpression in ABR Compose that should be treated as as template. " +
        "In most cases, this will match a single DataImpression, but if multiple DataImpressions use the same " +
        "KeyData, the class will handle switching the data on all of them.")]
    public string dataPathForTemplateDataImpressions = "LANL/FireSim/KeyData/Wind";

    [Tooltip("This string identifies the DataImpression in ABR Compose that should be treated as as template. " +
        "In most cases, this will match a single DataImpression, but if multiple DataImpressions use the same " +
        "KeyData, the class will handle switching the data on all of them.")]
    public string dataPathForTemplateDataImpressions2 = "LANL/FireSim/KeyData/Wind";


    [Tooltip("This string identifies the DataImpression in ABR Compose that should be treated as as template. " +
        "In most cases, this will match a single DataImpression, but if multiple DataImpressions use the same " +
        "KeyData, the class will handle switching the data on all of them.")]
    public string dataPathForTemplateDataImpressions3 = "LANL/FireSim/KeyData/Wind";


    [Header("Timestep-Specific Data")]

    [Tooltip("This is the base string for the datapath where the time-varying versions of the KeyData can be found. " +
        "The filename for the timesteps is created by appending the timestep number to this string.")]
    public string baseDataPathForTimesteps = "LANL/FireSim/KeyData/Wind_";

    [Tooltip("This is the base string for the datapath where the time-varying versions of the KeyData can be found. " +
        "The filename for the timesteps is created by appending the timestep number to this string.")]
    public string baseDataPathForTimesteps2 = "LANL/FireSim/KeyData/Wind_";
    [Tooltip("This is the base string for the datapath where the time-varying versions of the KeyData can be found. " +
        "The filename for the timesteps is created by appending the timestep number to this string.")]
    public string baseDataPathForTimesteps3 = "LANL/FireSim/KeyData/Wind_";

    [Tooltip("The number of the first timestep. It is appended to baseDataPathForTimesteps to get the filename for " +
        "the KeyData.")]
    public int firstTimestepNumber = 4;

    [Tooltip("The number of the last timestep. It is appended to baseDataPathForTimesteps to get the filename for " +
        "the KeyData.")]
    public int lastTimestepNumber = 74;

    [Tooltip("Since the data are large, visualizations do not always display every timestep.  This is the amount to " +
        "increment the timestep number when iterating between first and last.  Set it to 1 to load every timestep, " +
        "2 to load every other timestep, etc.")]
    public int timestepNumberInc = 14;


    public bool renderFirstImpression = true;
    // private runtime only vars
    List<IDataImpression> dataImpressions;
    List<IDataImpression> dataImpressions2;
    List<IDataImpression> dataImpressions3;
    List<int> timestepNumbers;
    List<int> timestepNumbers2;
    List<int> timestepNumbers3;
    List<KeyData> timestepKeyData;
    List<KeyData> timestepKeyData2;
    List<KeyData> timestepKeyData3;
    string firstSimGroup;
    string secondSimGroup;
    string thirdSimGroup;
    // note: this is the index into the timestepNumbers and timestepKeyData arrays, NOT the current timestep number.
    int currentTimestepIndex;

    /// <summary>
    /// Returns true if data are available for the timestep specified.
    /// </summary>
    public bool HasTimestep(int timestepNumber)
    {
        return timestepNumbers.Contains(timestepNumber);
    }

    /// <summary>
    /// Returns the timestep number for the data that are currently being displayed.
    /// </summary>
    public int CurrentTimestep()
    {
        if (timestepNumbers.Count > 0) {
            return timestepNumbers[currentTimestepIndex];
        } else {
            return -1;
        }
    }

    /// <summary>
    /// Updates DataImpressions to use the KeyData for the specified timestep.
    /// </summary>
    /// <param name="timestepNumber"></param>
    public void SetTimestep(int timestepNumber)
    {
        int closestIndex = timestepNumbers.IndexOf(timestepNumber);
        if ((closestIndex == -1) && (timestepNumbers.Count > 0)) {
            int closestDiff = Mathf.Abs(timestepNumber - timestepNumbers[0]);
            for (int i = 1; i < timestepNumbers.Count; i++) {
                int diff = Mathf.Abs(timestepNumber - timestepNumbers[i]);
                if (diff < closestDiff) {
                    closestIndex = i;
                    closestDiff = diff;
                }
            }
        }

        if ((closestIndex != -1) && (closestIndex != currentTimestepIndex)) {
            currentTimestepIndex = closestIndex;
        }
    }


    private void Start()
    {
        currentTimestepIndex = 0;
        timestepNumbers = new List<int>();
        timestepNumbers2 = new List<int>();
        timestepNumbers3 = new List<int>();
        timestepKeyData = new List<KeyData>();
        timestepKeyData2 = new List<KeyData>();
        timestepKeyData3 = new List<KeyData>();
        dataImpressions = new List<IDataImpression>();
        dataImpressions2 = new List<IDataImpression>();
        dataImpressions3 = new List<IDataImpression>();

        // Cheat by getting the first 8 chars from the datapath because that 
        firstSimGroup = dataPathForTemplateDataImpressions.Substring(0, 8);
        secondSimGroup = dataPathForTemplateDataImpressions2.Substring(0, 8);
        thirdSimGroup = dataPathForTemplateDataImpressions3.Substring(0, 8);
    }

    private void Update()
    {
        // update impression groups incase they changed.
        firstSimGroup = dataPathForTemplateDataImpressions.Substring(0, 8);
        secondSimGroup = dataPathForTemplateDataImpressions2.Substring(0, 8);
        thirdSimGroup = dataPathForTemplateDataImpressions3.Substring(0, 8);

        if (dataImpressions.Count == 0) {
            dataImpressions = ABREngine.Instance.GetDataImpressions(dataPathForTemplateDataImpressions);
            if ((Time.frameCount == 1) && (dataImpressions.Count == 0)) {
                // frameCount == 1 so we only warn once
                Debug.LogWarning($"No DataImpressions reference the data path '{dataPathForTemplateDataImpressions}'.");
            }
        }
        if (dataImpressions2.Count == 0)
        {
            dataImpressions2 = ABREngine.Instance.GetDataImpressions(dataPathForTemplateDataImpressions2);
            if ((Time.frameCount == 1) && (dataImpressions2.Count == 0))
            {
                // frameCount == 1 so we only warn once
                Debug.LogWarning($"No DataImpressions reference the data path '{dataPathForTemplateDataImpressions2}'.");
            }
        }
        if (dataImpressions3.Count == 0)
        {
            dataImpressions3 = ABREngine.Instance.GetDataImpressions(dataPathForTemplateDataImpressions3);
            if ((Time.frameCount == 1) && (dataImpressions3.Count == 0))
            {
                // frameCount == 1 so we only warn once
                Debug.LogWarning($"No DataImpressions reference the data path '{dataPathForTemplateDataImpressions3}'.");
            }
        }


        if ((dataImpressions.Count != 0) && (timestepKeyData.Count == 0)) {
            for (int i = firstTimestepNumber; i <= lastTimestepNumber; i += timestepNumberInc) {
                string mediaDir = Path.GetFullPath(ABREngine.Instance.MediaPath);
                string dataPath = baseDataPathForTimesteps + i.ToString();
                string fullPath = Path.Combine(mediaDir, ABRConfig.Consts.DatasetFolder, dataPath) + ".json";
                FileInfo jsonFile = new FileInfo(fullPath);
                if (jsonFile.Exists) {
                    KeyData kd = ABREngine.Instance.Data.LoadData(dataPath);
                    timestepNumbers.Add(i);
                    timestepKeyData.Add(kd);
                } else {
                    Debug.LogWarning($"Skipping timestep {i}. Could not find data file at '{fullPath}'");
                }
            }
        }

        if ((dataImpressions2.Count != 0) && (timestepKeyData2.Count == 0))
        {
            for (int i = firstTimestepNumber; i <= lastTimestepNumber; i += timestepNumberInc)
            {
                string mediaDir = Path.GetFullPath(ABREngine.Instance.MediaPath);
                string dataPath2 = baseDataPathForTimesteps2 + i.ToString();
                string fullPath2 = Path.Combine(mediaDir, ABRConfig.Consts.DatasetFolder, dataPath2) + ".json";
                FileInfo jsonFile2 = new FileInfo(fullPath2);
                if (jsonFile2.Exists)
                {
                    KeyData kd2 = ABREngine.Instance.Data.LoadData(dataPath2);
                    timestepNumbers2.Add(i);
                    timestepKeyData2.Add(kd2);
                }
                else
                {
                    Debug.LogWarning($"Skipping timestep {i}. Could not find data file at '{fullPath2}'");
                }
            }
        }

        if ((dataImpressions3.Count != 0) && (timestepKeyData3.Count == 0))
        {
            for (int i = firstTimestepNumber; i <= lastTimestepNumber; i += timestepNumberInc)
            {
                string mediaDir = Path.GetFullPath(ABREngine.Instance.MediaPath);
                string dataPath3 = baseDataPathForTimesteps2 + i.ToString();
                string fullPath3 = Path.Combine(mediaDir, ABRConfig.Consts.DatasetFolder, dataPath3) + ".json";
                FileInfo jsonFile3 = new FileInfo(fullPath3);
                if (jsonFile3.Exists)
                {
                    KeyData kd3 = ABREngine.Instance.Data.LoadData(dataPath3);
                    timestepNumbers3.Add(i);
                    timestepKeyData3.Add(kd3);
                }
                else
                {
                    Debug.LogWarning($"Skipping timestep {i}. Could not find data file at '{fullPath3}'");
                }
            }
        }
        // The KeyData need to be updated whenever the timestep changes.
        // If using ABR Compose, a style change will also have the impact of resetting the keydata to whatever
        // is used in ABR Compose.  So, it is good to check every frame in case that change was made.
        foreach (IDataImpression di in dataImpressions) {
            // todo: add a TrySetKeyData() method to IDataImpression to avoid needing to loop through all the
            // possible types here.

            SimpleLineDataImpression lineDI = di as SimpleLineDataImpression;
            if ((lineDI != null) && (lineDI.keyData != timestepKeyData[currentTimestepIndex])) {
                lineDI.keyData = timestepKeyData[currentTimestepIndex];
                lineDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
            SimpleGlyphDataImpression glyphDI = di as SimpleGlyphDataImpression;
            if ((glyphDI != null) && (glyphDI.keyData != timestepKeyData[currentTimestepIndex])) {
                glyphDI.keyData = timestepKeyData[currentTimestepIndex];
                glyphDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
            SimpleSurfaceDataImpression surfDI = di as SimpleSurfaceDataImpression;
            if ((surfDI != null) && (surfDI.keyData != timestepKeyData[currentTimestepIndex])) {
                surfDI.keyData = timestepKeyData[currentTimestepIndex];
                surfDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
            SimpleVolumeDataImpression volDI = di as SimpleVolumeDataImpression;
            if ((volDI != null) && (volDI.keyData != timestepKeyData[currentTimestepIndex])) {
                volDI.keyData = timestepKeyData[currentTimestepIndex];
                volDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
        }

        DataImpressionGroup secondSim =
            ABREngine.Instance.GetDataImpressionGroup(secondSimGroup);
        secondSim.GroupRoot.transform.position = new Vector3(0, 0, 10000);

        foreach (IDataImpression di in dataImpressions2)
        {
            // todo: add a TrySetKeyData() method to IDataImpression to avoid needing to loop through all the
            // possible types here.

            SimpleLineDataImpression lineDI = di as SimpleLineDataImpression;
            if ((lineDI != null) && (lineDI.keyData != timestepKeyData2[currentTimestepIndex]))
            {
                lineDI.keyData = timestepKeyData2[currentTimestepIndex];
                lineDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
            SimpleGlyphDataImpression glyphDI = di as SimpleGlyphDataImpression;
            if ((glyphDI != null) && (glyphDI.keyData != timestepKeyData2[currentTimestepIndex]))
            {
                glyphDI.keyData = timestepKeyData2[currentTimestepIndex];
                glyphDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
            SimpleSurfaceDataImpression surfDI = di as SimpleSurfaceDataImpression;
            if ((surfDI != null) && (surfDI.keyData != timestepKeyData2[currentTimestepIndex]))
            {
                surfDI.keyData = timestepKeyData2[currentTimestepIndex];
                surfDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
            SimpleVolumeDataImpression volDI = di as SimpleVolumeDataImpression;
            if ((volDI != null) && (volDI.keyData != timestepKeyData2[currentTimestepIndex]))
            {
                volDI.keyData = timestepKeyData2[currentTimestepIndex];
                volDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
        }

        DataImpressionGroup thirdSim =
            ABREngine.Instance.GetDataImpressionGroup(thirdSimGroup);
        thirdSim.GroupRoot.transform.position = new Vector3(0, 10000, 10000);

        foreach (IDataImpression di in dataImpressions3)
        {
            // todo: add a TrySetKeyData() method to IDataImpression to avoid needing to loop through all the
            // possible types here.

            SimpleLineDataImpression lineDI = di as SimpleLineDataImpression;
            if ((lineDI != null) && (lineDI.keyData != timestepKeyData2[currentTimestepIndex]))
            {
                lineDI.keyData = timestepKeyData2[currentTimestepIndex];
                lineDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
            SimpleGlyphDataImpression glyphDI = di as SimpleGlyphDataImpression;
            if ((glyphDI != null) && (glyphDI.keyData != timestepKeyData2[currentTimestepIndex]))
            {
                glyphDI.keyData = timestepKeyData2[currentTimestepIndex];
                glyphDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
            SimpleSurfaceDataImpression surfDI = di as SimpleSurfaceDataImpression;
            if ((surfDI != null) && (surfDI.keyData != timestepKeyData2[currentTimestepIndex]))
            {
                surfDI.keyData = timestepKeyData2[currentTimestepIndex];
                surfDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
            SimpleVolumeDataImpression volDI = di as SimpleVolumeDataImpression;
            if ((volDI != null) && (volDI.keyData != timestepKeyData2[currentTimestepIndex]))
            {
                volDI.keyData = timestepKeyData2[currentTimestepIndex];
                volDI.RenderHints.DataChanged = true;
                ABREngine.Instance.Render();
            }
        }
    }

    public void TogglePressure(bool toggle)
    {
        Debug.Log(toggle);
        DataImpressionGroup firstSim =
            ABREngine.Instance.GetDataImpressionGroup(firstSimGroup);
        List<SimpleVolumeDataImpression> impressions = firstSim.GetDataImpressions<SimpleVolumeDataImpression>();
        impressions[2].RenderHints.Visible = toggle;

        renderFirstImpression = toggle;
        DataImpressionGroup secondSim =
            ABREngine.Instance.GetDataImpressionGroup(secondSimGroup);
        impressions = secondSim.GetDataImpressions<SimpleVolumeDataImpression>();
        impressions[2].RenderHints.Visible = toggle;

        renderFirstImpression = toggle;
        DataImpressionGroup thirdSim =
            ABREngine.Instance.GetDataImpressionGroup(thirdSimGroup);
        impressions = thirdSim.GetDataImpressions<SimpleVolumeDataImpression>();
        impressions[2].RenderHints.Visible = toggle;

        ABREngine.Instance.Render();
    }

    public void ToggleTemp(bool toggle)
    {
        Debug.Log(toggle);
        DataImpressionGroup firstSim =
            ABREngine.Instance.GetDataImpressionGroup(firstSimGroup);
        List<SimpleVolumeDataImpression> impressions = firstSim.GetDataImpressions<SimpleVolumeDataImpression>();
        impressions[1].RenderHints.Visible = toggle;

        renderFirstImpression = toggle;
        DataImpressionGroup secondSim =
            ABREngine.Instance.GetDataImpressionGroup(secondSimGroup);
        impressions = secondSim.GetDataImpressions<SimpleVolumeDataImpression>();
        impressions[1].RenderHints.Visible = toggle;

        renderFirstImpression = toggle;
        DataImpressionGroup thirdSim =
            ABREngine.Instance.GetDataImpressionGroup(thirdSimGroup);
        impressions = thirdSim.GetDataImpressions<SimpleVolumeDataImpression>();
        impressions[1].RenderHints.Visible = toggle;

        ABREngine.Instance.Render();
    }

    public void ToggleMeteor(bool toggle)
    {
        Debug.Log(toggle);
        DataImpressionGroup firstSim =
            ABREngine.Instance.GetDataImpressionGroup(firstSimGroup);
        List<SimpleSurfaceDataImpression> impressions = firstSim.GetDataImpressions<SimpleSurfaceDataImpression>();
        impressions[0].RenderHints.Visible = toggle;

        renderFirstImpression = toggle;
        DataImpressionGroup secondSim =
            ABREngine.Instance.GetDataImpressionGroup(secondSimGroup);
        impressions = secondSim.GetDataImpressions<SimpleSurfaceDataImpression>();
        impressions[0].RenderHints.Visible = toggle;

        renderFirstImpression = toggle;
        DataImpressionGroup thirdSim =
            ABREngine.Instance.GetDataImpressionGroup(thirdSimGroup);
        impressions = thirdSim.GetDataImpressions<SimpleSurfaceDataImpression>();
        impressions[0].RenderHints.Visible = toggle;

        ABREngine.Instance.Render();
    }

    public void ToggleWater(bool toggle)
    {
        Debug.Log(toggle);
        DataImpressionGroup firstSim =
            ABREngine.Instance.GetDataImpressionGroup(firstSimGroup);
        List<SimpleSurfaceDataImpression> impressions = firstSim.GetDataImpressions<SimpleSurfaceDataImpression>();
        impressions[1].RenderHints.Visible = toggle;

        renderFirstImpression = toggle;
        DataImpressionGroup secondSim =
            ABREngine.Instance.GetDataImpressionGroup(secondSimGroup);
        impressions = secondSim.GetDataImpressions<SimpleSurfaceDataImpression>();
        impressions[1].RenderHints.Visible = toggle;

        renderFirstImpression = toggle;
        DataImpressionGroup thirdSim =
            ABREngine.Instance.GetDataImpressionGroup(thirdSimGroup);
        impressions = thirdSim.GetDataImpressions<SimpleSurfaceDataImpression>();
        impressions[1].RenderHints.Visible = toggle;

        ABREngine.Instance.Render();


        Debug.Log(toggle);
        List<SimpleVolumeDataImpression> impressions2 = firstSim.GetDataImpressions<SimpleVolumeDataImpression>();
        impressions2[0].RenderHints.Visible = toggle;

        renderFirstImpression = toggle;
        impressions2 = secondSim.GetDataImpressions<SimpleVolumeDataImpression>();
        impressions2[0].RenderHints.Visible = toggle;

        renderFirstImpression = toggle;
        impressions2 = thirdSim.GetDataImpressions<SimpleVolumeDataImpression>();
        impressions2[0].RenderHints.Visible = toggle;

        ABREngine.Instance.Render();
    }

}
