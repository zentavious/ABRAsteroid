using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IVLab.ABREngine;

public class DataFilter : MonoBehaviour
{
    public string dataPathForTemplateDataImpressions = "LANL/FireSim/KeyData/Wind";

    public string dataPathForTemplateDataImpressions2 = "LANL/FireSim/KeyData/Wind";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleData(bool toggle)
    {
        Debug.Log(toggle);
        if (toggle)
        {
            Debug.Log(ABREngine.Instance.Config.mediaPath);
            // image.sprite = colorSprite;
            Debug.Log(dataPathForTemplateDataImpressions);
            List<IDataImpression> dataImpressions = ABREngine.Instance.GetDataImpressions(dataPathForTemplateDataImpressions);
            if ((dataImpressions.Count == 0))
            {
                // frameCount == 1 so we only warn once
                Debug.LogWarning($"No DataImpressions reference the data path '{dataPathForTemplateDataImpressions}'.");
            }
            //Debug.Log(((DataImpression) di).Tags.ToString());


            //di = ABREngine.Instance.GetDataImpressions(dataPathForTemplateDataImpressions2)[0];
            //Debug.Log(((DataImpression)di).Tags.ToString());
        }
        else
        {
            // image.sprite = graySprite;
        }
    }
}
