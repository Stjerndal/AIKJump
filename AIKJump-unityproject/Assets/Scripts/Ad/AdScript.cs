using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AdScript : MonoBehaviour {
	public GameObject myObject;
	public Texture2D[] adTextures;
	public RawImage rawImage;


	public bool ready;
	public bool showing;
	public bool showWhenReady;

	bool dataLoaded;
	string data;

	string[] imgUrls;
	float[] durations;
	int cur;
	float curTime;
		
	private const string DATA_URL = "http://perm.ly/erycr";
	string adUrl;

	private static AdScript instance = null;
	public static AdScript Instance {
		get { return instance; }
	}
	void Awake() {
		if (instance != null && instance != this) {
//			Debug.Log("Found AdManager.");
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(transform.gameObject);
		ready = false;
		showing = false;
		showWhenReady = false;
		HideAd();
	}


	void OnGUI() {
		if(showing) {
			curTime += Time.deltaTime;
			if(curTime > durations[cur]) {
				curTime = 0;
				if(++cur >= adTextures.Length)
					cur = 0;
				if(rawImage == null) {
					rawImage = myObject.GetComponentInChildren<RawImage>();
				}
				rawImage.texture = adTextures[cur];
			}
		}
	}

	public void LoadAndShow() {
		if(!ready) {
			LoadAd();
	//		ShowAd();
			showWhenReady = true;
		}
		else {
			ShowAd();
		}
	}

	public void LoadAd() {
//		Debug.Log("Loading..." + ready);
		if(!ready)
			StartCoroutine(fetchData ());
//		durations = new float[adTextures.Length];
//		for(int i = 0; i < adTextures.Length; i++)
//			durations[i] = 5f;
//		cur = 0;
//		rawImage.texture = adTextures[cur];
//		adUrl = "http://www.ticnet.se/event/FDSA90KN?language=sv-se&CAMEFROM=612friends&brand=se_friendsarena";
//		
//		ready = true;
	}

	void Update() {
		if(dataLoaded) {
			dataLoaded = false;
			handleData();
//			Debug.Log("Handled data, now img...");
			StartCoroutine(fetchImages());
		}
	}

	IEnumerator fetchData() {
		WWW www = new WWW(DATA_URL);
		yield return www;
		data = www.text;
		dataLoaded = true;
	}

	IEnumerator fetchImages() {
//		Debug.Log("fetching imgs");
		for(int i = 0; i < imgUrls.Length; i++) {
			WWW www = new WWW(imgUrls[i]);
			yield return www;
			adTextures[i] = new Texture2D(1,1);
			www.LoadImageIntoTexture(adTextures[i]);
//			Debug.Log("Got " + (i + 1) + " img(s)");
		}

		ready = true;
		if(showWhenReady) {
			ShowAd();
			showWhenReady = false;
		}
	}

	void handleData() {
		string[] dataParts = data.Split('*');
		string[] subPart;

//		float frequency = float.Parse(dataParts[1]); //NEED UPDATE

		subPart = dataParts[2].Split('\n');
		imgUrls = new string[subPart.Length-2];
		for(int i = 0; i < imgUrls.Length; i++) {
			imgUrls[i] = subPart[i+1];
//			Debug.Log("img: " + imgUrls[i]);
		}

		adTextures = new Texture2D[imgUrls.Length];
		durations = new float[imgUrls.Length];

		subPart = dataParts[3].Split('\n');
		for(int i = 0; i < adTextures.Length; i++) {
			durations[i] = float.Parse(subPart[i+1]);
//			Debug.Log("dur: " + durations[i]);
		}
		adUrl = dataParts[4].Trim ();
	}



	public void ShowAd() {
//		Debug.Log("Showing Ad..");
		if(ready) {
//			Debug.Log("Really ready for this!! SO PUMPD!");
			showing = true;
			myObject.SetActive(true);
		}
	}

	public void HideAd() {
		showing = false;
		myObject.SetActive(false);
	}

	public void AdClicked() {
//		Debug.Log("Opening URL: " + adUrl);
		Application.OpenURL(adUrl);
//		Application.OpenURL("http://www.ticnet.se/event/FDSA90KN?language=sv-se&CAMEFROM=612friends&brand=se_friendsarena");
	}
}
