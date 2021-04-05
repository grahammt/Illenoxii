using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using System;

public class ConsentToAnalytics : MonoBehaviour
{
    private bool analytics_enabled = false;
    // Start is called before the first frame update
    void Start()
    {
        Analytics.enabled = false;
        Analytics.deviceStatsEnabled = false;
        PerformanceReporting.enabled = false;
        
        EventBus.Subscribe<EnableAnalyticsEvent>(_enable);
    }

    void _enable(EnableAnalyticsEvent e) {
        Analytics.enabled = true;
        Analytics.deviceStatsEnabled = true;
        PerformanceReporting.enabled = true;
    }
}
