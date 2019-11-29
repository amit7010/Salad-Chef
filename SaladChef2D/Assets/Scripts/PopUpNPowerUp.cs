using SaladChef2D.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaladChef2D.UI
{
    public class PopUpNPowerUp : Singleton<PopUpNPowerUp>
    {
        #region Variables

        //PopUp Text
        public TextMesh popUp;
        //PowerUp Parent - if not given, the attached gameObject will be Parent
        public GameObject powerUpParent;
        public float PopUpTime = 1f;
        public GameObject[] thePowerUps;

        // the range of X
        [Header("X Spawn Range")]
        public float xMin;
        public float xMax;

        // the range of y
        [Header("Y Spawn Range")]
        public float yMin;
        public float yMax;

        #endregion

        #region Helper Functions 

        /// <summary>
        /// Function to Show PopUP
        /// </summary>
        /// <param name="isPositive"></param>
        /// <param name="point"></param>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public IEnumerator ShowPopup(bool isPositive,string pointMsg, string playerName)
        {
            popUp.gameObject.SetActive(true);
            if(isPositive)
            {
                popUp.color = Color.green;
                popUp.text = "+" + pointMsg + " for "+playerName;
            }                                    
            else                                 
            {                                    
                popUp.color = Color.red;         
                popUp.text = "-" + pointMsg + " for "+playerName;
            }
            yield return new WaitForSeconds(PopUpTime);
            popUp.gameObject.SetActive(false);
        }

        public IEnumerator ShowPowerUps()
        {
            yield return null;
            // Defines the min and max ranges for x and y
            Vector2 pos = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
            // Choose a new goods to spawn from the array (note I specifically call it a 'prefab' to avoid confusing myself!)
            GameObject powerUpPrefab = thePowerUps[Random.Range(0, thePowerUps.Length)];

            // Creates the random object at the random 2D position.
            GameObject newPower = Instantiate(powerUpPrefab, pos, transform.rotation);

            yield return new WaitForSeconds(2f);
            Destroy(newPower);
            // If I wanted to get the result of instantiate and fiddle with it, I might do this instead:
            //GameObject newGoods = (GameObject)Instantiate(goodsPrefab, pos)
            //newgoods.something = somethingelse;
        }

        #endregion
    }
}
