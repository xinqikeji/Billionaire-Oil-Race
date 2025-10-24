using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class Splash : MonoBehaviour
{
    //[SerializeField] private string PrivacyPolicyLink;
   // [SerializeField] private string TermsConditionsLink;
    //[SerializeField] private GameObject PanelPrivacy;
    [SerializeField] private GameObject Logo;
    [SerializeField] private Slider LoadingBar;
    [SerializeField] private string nextScene;
    private bool isLoading;
    private float timeWait = 1f; 
    void Start()
    {
        Advertisements.Instance.Initialize();

        //PanelPrivacy.SetActive(false);
        Logo.SetActive(false);
        LoadingBar.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("IsFirsttime", 0) == 0)
        {
            //PanelPrivacy.SetActive(true);
            Yes();
        }
        else
        {
            No();
        }
    }

    void Update()
    {
        if (isLoading == true)
        {
            LoadingBar.value += 1.0f / timeWait * Time.deltaTime;
        }    
    }
    private void StartGame()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void openPrivacyPolicy()
    {
        //Application.OpenURL(PrivacyPolicyLink);
    }

    public void openTermsConditions()
    {
        //Application.OpenURL(TermsConditionsLink);
    }
    public void Yes()
    {
       // PlayerPrefs.SetInt("IsFirsttime", 1);
        //PanelPrivacy.SetActive(false);
        Logo.SetActive(true);
        LoadingBar.gameObject.SetActive(true);
        isLoading = true;
        StartCoroutine(waitInit());
    }

    public void No()
    {
        //PanelPrivacy.SetActive(false);
        Logo.SetActive(true);
        LoadingBar.gameObject.SetActive(true);
        isLoading = true;
        StartCoroutine(waitInit());
    }

    IEnumerator waitInit()
    {
        yield return new WaitForSeconds(timeWait);
        StartGame();
    }
}
