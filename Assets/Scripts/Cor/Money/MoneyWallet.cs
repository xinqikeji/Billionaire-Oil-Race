using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BlueStellar.Cor
{
    public class MoneyWallet : MonoBehaviour
    {
        #region Singelton

        public static MoneyWallet Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        [SerializeField] TextMeshProUGUI textAmmountMoney;
        [SerializeField] public int ammountMoney;

        public int Money()
        {
            return ammountMoney;
        }

        private void Start()
        {
            LoadSave();
            MoneyText(textAmmountMoney, ammountMoney);
        }

        public void MoneyPlus(int number)
        {
            ammountMoney += number;
            MoneyText(textAmmountMoney, ammountMoney);
            Save();
        }

        public void MoneyMinus(int number)
        {
            ammountMoney -= number;
            MoneyText(textAmmountMoney, ammountMoney);
            Save();
        }

        private void MoneyText(TextMeshProUGUI textMoney, int money)
        {
            if (textMoney == null)
                return;

            if (money < 10)
            {
                textMoney.text = money.ToString().Substring(0, 1);
                return;
            }

            if (money >= 10 && money < 100)
            {
                textMoney.text = money.ToString().Substring(0, 2);
                return;
            }

            if (money >= 100 && money < 1000)
            {
                textMoney.text = money.ToString().Substring(0, 3);
                return;
            }

            textMoney.text = GetSuffixValue(money);
        }

        string GetSuffixValue(float value)
        {
            int zero = 0;

            while (value >= 1000)
            {
                ++zero;

                value /= 1000;
            }

            string suffix = string.Empty;

            switch (zero)
            {
                case 0: suffix = ""; break;
                case 1: suffix = "K"; break;
                case 2: suffix = "M"; break;
                case 3: suffix = "B"; break;
                case 4: suffix = "T"; break;
                case 5: suffix = "Qd"; break;
                case 6: suffix = "Qn"; break;
                case 7: suffix = "Sx"; break;
                case 8: suffix = "Sp"; break;
                case 9: suffix = "Oc"; break;
            }

            return $"{value:0.##}{suffix}";
        }

        #region Load&Save

        private void LoadSave()
        {
            ammountMoney = ES3.Load("ammountMoney", ammountMoney);
        }

        private void Save()
        {
            ES3.Save("ammountMoney", ammountMoney);
        }

        #endregion
    }
}