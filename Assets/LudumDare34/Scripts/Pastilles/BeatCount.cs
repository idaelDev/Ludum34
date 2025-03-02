﻿using UnityEngine;
using System.Collections;
using System;

public class BeatCount : Singleton<BeatCount>{

	public float t = 0.0f;
    public float deltaTime;
    public float bpm = 0f;
    public float beat = 0.0f;
    public float beatTot = 0.0f;
    static bool rhythmAble = false;
    public AudioSource Mo;
    private float expectedTime;
    private float timeOffset;

	public delegate void BeatDelegate();
	public static event BeatDelegate BeatEvent;

    public bool canCountBeat = false;
	private bool waitCountBeat = true;
	// Use this for initialization
	void Start () {
        timeOffset = 60.0f / bpm;
        expectedTime = timeOffset;
		if (waitCountBeat)
			StartCoroutine(waitFirstBeat());
		End.EndEvent += EndGame;
	}

	IEnumerator waitFirstBeat()
	{
		yield return new WaitForSeconds(0.3f);
		waitCountBeat = false;
	}

	IEnumerator resetBeat()
	{
		expectedTime = 0f;
		waitCountBeat = true;
		yield return new WaitForSeconds(timeOffset);
		BeatEvent();
		yield return new WaitForSeconds(timeOffset);
		expectedTime = timeOffset;
		waitCountBeat = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (canCountBeat && !waitCountBeat)
        {
            t = Mo.time;

            if (t > expectedTime)
            {
                BeatEvent();
                expectedTime += timeOffset;
				if (expectedTime > Mo.clip.length)
				{
					
					//expectedTime = 0;
					StartCoroutine(resetBeat());
				}
            }
        }
	}

	public bool isOnBeat() {
		if (Mathf.Abs(Mo.time - expectedTime) < 0.15f
				|| Mathf.Abs(Mo.time - expectedTime + timeOffset) < 0.15f)
		{
			return true;
		} else

		return false;
	}

	public void EndGame() {
		canCountBeat = false;
	}

}
