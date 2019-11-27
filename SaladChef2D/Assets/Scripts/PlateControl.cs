using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef2D.UI
{
    public class PlateControl : MonoBehaviour
    {
        #region Variables
        public PlayerControl playerData;
        VegDataController oneVegetable;
        public TextMesh PlateContentText;

        private bool plateFull = false;
        #endregion
        void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject == playerData.gameObject)
            {
                if(playerData.playerStatus == PlayerStatus.RAWVEG || playerData.playerStatus == PlayerStatus.EMPTY)
                {
                    if (!plateFull)
                    {
                        oneVegetable = playerData.RemoveRawVegetableFromBag();
                        PlateContentText.text = oneVegetable.Data.VegName;
                        plateFull = true;
                    }
                    else
                    {
                        bool AddingVeg = false;
                        AddingVeg = playerData.AddVegetableToBag(oneVegetable, playerData.playerStatus);
                        if (AddingVeg)
                        {
                            PlateContentText.text = oneVegetable.Data.VegName;
                        }
                        else
                        {
                            Debug.Log("Player Bag Full");
                            StartCoroutine(playerData.ShowWarning("Player Bag Full"));
                        }
                    }
                }
                else
                {
                    StartCoroutine(playerData.ShowWarning("Can't Place Salads!"));
                    Debug.Log("Can't Place");
                }
            }
        }
    }
}
