using UnityEngine;

public class Currency : Pickup
{
    public int val;
    public AudioSource audioObject;
    public AudioClip coinSoundAudioClip;

    protected override void PickedUp(Player player)
    {
        player.money += val;

        AudioSource clone = Instantiate(audioObject);
        clone.clip = coinSoundAudioClip;
        clone.Play();
        clone.gameObject.hideFlags = HideFlags.HideInHierarchy;
        Destroy(clone.gameObject, 3);
    }
}
