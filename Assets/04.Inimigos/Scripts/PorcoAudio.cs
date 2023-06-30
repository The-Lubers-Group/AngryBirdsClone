using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorcoAudio : MonoBehaviour
{
	[SerializeField] private AudioSource _aSource;
	[SerializeField] private AudioClip[] _aClips;
    // Start is called before the first frame update
    void Start()
    {
        _aSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		if(!_aSource.isPlaying)
		{
			StartCoroutine(Reproduzir());
		}
    }

	IEnumerator Reproduzir()
	{
		yield return new WaitForSeconds(1f);
		_aSource.clip = GetRandom();
		_aSource.Play();
	}
	AudioClip GetRandom()
	{
        return _aClips[Random.Range(0, _aClips.Length)];
	}

}
