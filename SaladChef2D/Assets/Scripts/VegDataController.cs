using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef2D.UI
{
    /// <summary>
    /// Class keeps track of Vegetable
    /// </summary>
    public class VegDataController : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        public VegData Data;

        public TextMesh DisplayName;

        #endregion

        void Awake()
        {
            if (Data != null)
            {
                DisplayName.text = Data.VegName;
            }
        }

    }
}