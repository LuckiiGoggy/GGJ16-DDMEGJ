﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Base : MonoBehaviour {
	
	public Player m_Owner;
	public float m_BaseInvulnerableLength;

    public AudioClip m_BaseCompleteSFX;
    public AudioClip m_ItemGetSFX;


	public bool SacrificesCompleted { 
		get { 
			return m_Completed[0] && m_Completed[1] && m_Completed[2];
		}
	}
			

    public Spawner m_Spawner;
	public bool[] m_Completed;

	public GameObject bottomTotem_deactivated;
	public GameObject middleTotem_deactivated;
	public GameObject topTotem_deactivated;

	public GameObject bottomTotem_activated;
	public GameObject middleTotem_activated;
	public GameObject topTotem_activated;

	void Start()
	{
		m_Completed = new bool[3];
	}


    public void FixedUpdate()
    {
        if (m_Owner != null)
        {
            if (SacrificesCompleted)
            {
                m_Owner.GetComponent<Player>().GodModeOn();
                m_Spawner.StartSpawningShields();
            }
            else
            {
                m_Owner.GetComponent<Player>().GodModeOff();
                m_Spawner.StopSpawningShields();
            }
        }
	}

	public void AddSacrifice(GameObject sacrifice)
	{
		Debug.Log ("Do we have all sacrifices: " + SacrificesCompleted);

		m_Completed[(int) sacrifice.GetComponent<Sacrifice>().m_SacrificeType] = true;
		Destroy (sacrifice);

		ActivateRespectiveTotem ();

		if (!SacrificesCompleted) {
			m_Spawner.Spawn();

		}
	}

	void ActivateRespectiveTotem ()
	{
		if (m_Completed [1] == true)
        {
            if (bottomTotem_activated.activeInHierarchy == false) ;
            {
                //GetComponent<AudioSource>().PlayOneShot(m_BaseCompleteSFX);
                //GetComponent<AudioSource>().pitch++;
            }
			bottomTotem_deactivated.SetActive (false);
            bottomTotem_activated.SetActive(true);
		}
		if (m_Completed [0] == true) 
		{
            if (middleTotem_activated.activeInHierarchy == false) ;
            {
                //GetComponent<AudioSource>().PlayOneShot(m_BaseCompleteSFX);
                //GetComponent<AudioSource>().pitch++;
            }
			middleTotem_deactivated.SetActive (false);
            middleTotem_activated.SetActive(true);
		}
		if (m_Completed [2] == true) 
		{
            if (topTotem_activated.activeInHierarchy == false) ;
            {
                //GetComponent<AudioSource>().PlayOneShot(m_BaseCompleteSFX);
                //GetComponent<AudioSource>().pitch++;
            }
			topTotem_deactivated.SetActive (false);
            topTotem_activated.SetActive(true);
		}
	}

	void ResetTotems ()
	{
		Debug.Log ("Reset Totems");

		bottomTotem_deactivated.SetActive (true);
		bottomTotem_activated.SetActive (false);
		middleTotem_deactivated.SetActive (true);
		middleTotem_activated.SetActive (false);
		topTotem_deactivated.SetActive (true);
		topTotem_activated.SetActive (false);

		m_Owner.GetComponent<Player>().GodModeOff();
		m_Owner.GetComponentInChildren<Weapon>().m_IsGodWeapon = false;
	}

	public void RemoveSacrifice(GameObject sacrifice)
	{
		m_Completed[(int) sacrifice.GetComponent<Sacrifice>().m_SacrificeType] = false;
		Destroy (sacrifice);
		m_Spawner.Spawn();

		ResetTotems ();
	}

	void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Item")
        {
            GetComponent<AudioSource>().PlayOneShot(m_ItemGetSFX);
            m_Spawner.Spawn();
        }

        else if (coll.gameObject.tag == "Sacrifice")
        {
            GetComponent<AudioSource>().PlayOneShot(m_BaseCompleteSFX);
			if (!SacrificesCompleted)
				AddSacrifice (coll.gameObject);
			else 
			{
				RemoveSacrifice (coll.gameObject);
				ActivateRespectiveTotem ();
			}
				
		}


	}
}
