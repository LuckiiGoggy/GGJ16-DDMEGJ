﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public GameObject sacrifice;

	private bool mIsHoldingSacrafice;

	public void PickUpSacrifice(GameObject sacrafice) {
		print ("pick up sacrafice");
		Destroy (sacrafice);
		mIsHoldingSacrafice = true;
	}

	public void DropSacrifice() {
		if (mIsHoldingSacrafice) {
			print ("drop sacrafice");
			mIsHoldingSacrafice = false;

			Instantiate (sacrifice);
			// Instantiate (sacrifice, transform.position, Quaternion.identity);
		}
	}

	public void PlaceSacrifice() {
		if (mIsHoldingSacrafice) {
			print ("place sacrafice in box");
			mIsHoldingSacrafice = false;

			Instantiate (sacrifice);
		}
	}


    private float m_StunTimer;
    private float m_SlowTimer;

    public float m_MovementSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        TopDownControl control = GetComponent<TopDownControl>();

        if (m_StunTimer > 0)
        {
            control.m_MovementSpeed = 0;
            m_StunTimer = Mathf.Max(m_StunTimer - Time.fixedDeltaTime, 0);
        }

        if (m_SlowTimer > 0)
        {
            control.m_MovementSpeed = Mathf.Min(m_MovementSpeed * 0.5f, control.m_MovementSpeed);
            m_SlowTimer = Mathf.Max(m_SlowTimer - Time.fixedDeltaTime, 0);
        }

        if(m_StunTimer == 0 && m_SlowTimer == 0)
        {
            control.m_MovementSpeed = m_MovementSpeed;
        }
        
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
		print ("player collides with something");
		if (coll.gameObject.tag == "Weapon") {
			var weapon = coll.transform.GetComponent<Weapon> ();

			if (weapon != null && weapon != GetComponentInChildren<Weapon> ()) {
				if (weapon.m_IsAttacking) {
					m_StunTimer = weapon.m_StunLength;
				}
			}
		} else if (coll.gameObject.tag == "Sacrifice") {
			print ("Player collides Sacrifice");
			PickUpSacrafice (coll.gameObject);
		}
    }
}