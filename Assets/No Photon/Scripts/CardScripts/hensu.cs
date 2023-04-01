using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace hensu
{
    public enum Cardtype
    {
        Mirror,
        Magic,
        Monster,
        Instant,
        Field,
        Building,
        Leader,
    };

    [System.Serializable]
    public struct EffectSetting
    {
        [SerializeField]
        public string effectname;
        [SerializeField]
        public List<int> effectint;
    }
}