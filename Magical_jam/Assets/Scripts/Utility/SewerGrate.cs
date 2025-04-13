using System;
using UnityEngine;

namespace Utility
{
    public class SewerGrate : MonoBehaviour
    {
        [SerializeField] private Collider2D sewerGrateCollider;
        
        public Collider2D SewerGrateCollider => sewerGrateCollider;
    }
}