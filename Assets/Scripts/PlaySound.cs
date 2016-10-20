using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource audio;
    public GameObject AudioPlayer;

    public void Play(AudioClip sound)
    {
        GameObject playerObject = Instantiate(AudioPlayer);
        playerObject.transform.position = transform.position;
	    audio = playerObject.GetComponent<AudioSource>();
        audio.clip = sound;
        audio.Play();
        Destroy(playerObject,3);
    }
}
