using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource audio;

    public void Play(GameObject AudioSource)
    {
        GameObject playerObject = Instantiate(AudioSource);
        playerObject.transform.position = transform.position;
        playerObject.hideFlags = HideFlags.HideInHierarchy;
	    audio = playerObject.GetComponent<AudioSource>();
        audio.Play();
        Destroy(playerObject,3);
    }
}
