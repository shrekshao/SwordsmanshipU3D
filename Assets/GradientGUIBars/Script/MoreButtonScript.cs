using UnityEngine;
using System.Collections;

public class MoreButtonScript : MonoBehaviour {


	public string MoreButton = "More ->";
	public Vector2 Position;

	public GameObject B1;
	public GameObject B2;
	public GameObject B3;
	public GameObject B4;
	public GameObject B5;
	public GameObject B6;
	public GameObject B7;
	public GameObject B8;
	public GameObject B9;

	//Stanard OnGUI Method
	void OnGUI()
	{
		
		if (GUI.Button(new Rect(Position.x,Position.y, 100, 25),MoreButton ))
		{
			if (MoreButton == "More ->")
			{
				MoreButton = "<- Back";
				B1.SetActive(false);
				B2.SetActive(false);
				B3.SetActive(false);
				B4.SetActive(false);
				B5.SetActive(false);
				B6.SetActive(false);
				B7.SetActive(true);
				B8.SetActive(true);
				B9.SetActive(true);
			}
			else
			{
				MoreButton = "More ->";
				B1.SetActive(true);
				B2.SetActive(true);
				B3.SetActive(true);
				B4.SetActive(true);
				B5.SetActive(true);
				B6.SetActive(true);
                B7.SetActive(false);
                B8.SetActive(false);
                B9.SetActive(false);
            }
            
        }
    }

	
}
