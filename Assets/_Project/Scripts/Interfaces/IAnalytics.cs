using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Asteroid.Services
{
    public interface  IAnalytics 
    {
        public UniTask<bool> Initialize();
        public void PushEvent <E> (string name, string parameterName, E parameterValue) where E : struct;
        public void PushUserProperty <P> (string name, P property) where P : struct;
        public void ResetAnalyticsData();
    }
}
