using UnityEngine;

public static class Extensions
{
    public static void PlayIfNotPlaying(this AudioSource @this)
    {
        if (!@this.isPlaying)
            @this.Play();
    }
}