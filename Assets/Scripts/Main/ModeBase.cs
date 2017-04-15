using UnityEngine;
using UnityEngine.UI;

public class ModeBase : MonoBehaviourEX
{
    [HideInInspector]
    public CamController cam;
    
    [HideInInspector]
    public StatData stat;

    [HideInInspector]
    public DataDetail detail;

    [HideInInspector]
    public SelectorController selector;

    [HideInInspector]
    public Text logger;

    string m_lastLog;

    public void Log (string log, bool urgent = false) {
        if ((enabled || urgent) && log != m_lastLog)
            logger.text = m_lastLog = log;
    }
}