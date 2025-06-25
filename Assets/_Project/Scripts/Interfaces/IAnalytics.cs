using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Services
{
    public interface  IAnalytics 
    {
        public UniTask<bool> Initialize();
        public void PushEvent<E>(string name, string parameterName, E parameterValue = default);
        public void PushUserProperty <P> (string name, P property = default);
        public void ResetAnalyticsData();
        public bool AnalyticsEnabled { get; }
    }
}
