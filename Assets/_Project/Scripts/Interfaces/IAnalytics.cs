using System;
using UnityEngine;

namespace Asteroid.Services
{
    public interface  IAnalytics <E,P> where P : struct where E : struct
    {
        public void PushEvent(string name, string parameterName, E parameterValue);
        public void PushUserProperty(string name, P property);
        public void ResetAnalyticsData();

    }
}
