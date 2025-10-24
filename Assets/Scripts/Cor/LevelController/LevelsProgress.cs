using UnityEngine;
using UnityEngine.UI;

namespace BlueStellar.Cor
{
    public class LevelsProgress : MonoBehaviour
    {
        [SerializeField] Image[] barIcons;
        [SerializeField] Image arenaA;
        [SerializeField] Image arenaB;
        [SerializeField] Sprite[] arenasIcons;
        [SerializeField] Sprite lockIcon;
        [SerializeField] Sprite selectedIcon;
        [SerializeField] Sprite completedIcon;
        [SerializeField] private int indexProgress;
        
        public void CheckLevelsProgress()
        {
            LoadSave();

            for(int i = 0; i <= indexProgress; i++)
            {
                if(i < indexProgress)
                {
                    barIcons[i].sprite = completedIcon;
                }

                if(i == indexProgress)
                {
                    barIcons[i].sprite = selectedIcon;
                }

                if(i > indexProgress)
                {
                    barIcons[i].sprite = lockIcon;
                }

                if (indexProgress >= barIcons.Length)
                {
                    foreach(var k in barIcons)
                    {
                        k.sprite = lockIcon;
                    }
                    barIcons[0].sprite = selectedIcon;
                }
            }

            if(LevelController.Instance.LvlIndex() <= 5)
            {
                arenaA.sprite = arenasIcons[0];
                arenaB.sprite = arenasIcons[1];
            }

            if(LevelController.Instance.LvlIndex() > 5 
                && LevelController.Instance.LvlIndex() <= 10)
            {
                arenaA.sprite = arenasIcons[1];
                arenaB.sprite = arenasIcons[2];
            }

            if (LevelController.Instance.LvlIndex() > 10)
            {
                arenaA.sprite = arenasIcons[2];
                arenaB.sprite = arenasIcons[0];
            }
        }

        public void ProgressUp()
        {
            indexProgress++;
            if (indexProgress >= barIcons.Length)
            {
                indexProgress = 0;
            }
            Save();
        }

        #region Load&Save

        private void LoadSave()
        {
            indexProgress = ES3.Load("indexProgress", indexProgress);
        }

        private void Save()
        {
            ES3.Save("indexProgress", indexProgress);
        }

        #endregion
    }
}
