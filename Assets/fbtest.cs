using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FacebookGames;
using Facebook.Unity;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase;
public class fbtest : MonoBehaviour
{
    FirebaseAuth auth;
    public Text _fbUserName;
    public Image _fbDp;
    public RectTransform _fbDpTransform;
    public Text _loginInfo;
    public GameObject _loginButton;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(InitializeFB);
        }
        else
        {
            FB.ActivateApp();
        }
    }

    private void InitializeFB()
    {
       FB.ActivateApp();
    }

    public void FBLogin()
    {
        var permissions = new List<string>();/* { "Player Profile", "email" };*/
        permissions.Add("PlayerProfile");
        permissions.Add("email");
        FB.LogInWithPublishPermissions(permissions,AuthCallback);
    }

    void AuthCallback(ILoginResult loginResult)
    {
        if(FB.IsLoggedIn)
        {
            Debug.Log("Loggged in");
            //DealWithFbMenus();
        }
        else
        {
            Debug.Log("Not Loggged in");
        }
    }

    void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            _loginButton.SetActive(false);
            _loginInfo.text = "Logged In";
            FB.API("/me?fields=name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, GetPicture);
            FB.Mobile.RefreshCurrentAccessToken();
        }
    }

    void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            string fbname = result.ResultDictionary["name"].ToString();
            _fbUserName.text = fbname;
        }
    }

    private void GetPicture(IGraphResult result)
    {
        if (result.Error == null)
        {
            Image img = _fbDp.GetComponent<Image>();
            img.sprite = Sprite.Create(result.Texture, new Rect(0, 0, _fbDpTransform.rect.width, _fbDpTransform.rect.height), new Vector2());
        }
    }
}
